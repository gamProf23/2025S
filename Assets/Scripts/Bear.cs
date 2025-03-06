using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Loading;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

//using UnityEngine.Windows;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Bear : MonoBehaviour
{





    [Header("Can Edit")]

    public int playerHP = 10;

    public int playerSpeedI; //5
    int playerSpeed;

    //In Seconds
    public float climbingTimerI; //2
    float climbingTimer; 

    public int jumpAmountI;
    int jumpAmount;

    public float rollMaxVelocity; //20

    public float clawSpeed; //20

    public float jumpForce; //400
    public float swimUpForce; //150

    public float slopeVelocityMult; //1.1

    public float waterGravity; //0.5
    public float outOfWaterUpForce; //100

    [Header("Controls")]

    //WASD or Arrow Keys

    public KeyCode jumpKey; //Space
    public KeyCode clawKey; // /
    public KeyCode rollKey; //Shift
    public KeyCode roarKey; //Period

    [Header("DO NOT TOUCH")]

    public GameObject playerObject;
    public Rigidbody2D playerRB;
    public Vector2 playerPosition;

    public float translationX;
    public float translationY;

    public bool isClimbing = false;
    public GameObject whatImClimbing;

    public bool isSwiming = false;

    public GameObject myRoar;
    bool isRoaring = false;

    public GameObject myBall;
    public bool isBall = false;

    public GameObject myClaw;
    bool isSwiping = false;

    bool isOnMGround = false;

    public bool isGrounded = false;
    public bool movingRight = true;

    List<Vector3> swipePointsL = new List<Vector3>();
    List<Vector3> swipePointsR = new List<Vector3>();

    public Animator myAnimations;

    float scaleX;
    float scaleXNeg;

    Vector3 ogScale;

    [Header("Slope Stuff: DO NOT TOUCH")]

    private float slopeCheckDistance = 3f;
    public bool isOnSlope = false;
    private float slopeSideAngle;
    private Vector2 slopeNormalPerp;
    private float slopeDownAngle;
    private float lastSlopeAngle;
    private float maxSlopeAngle = 90f;
    private bool canWalkOnSlope;
    Vector2 newVelocity = Vector2.zero;
    public PhysicsMaterial2D fullFriction;
    public PhysicsMaterial2D noFriction;

    




    private void Awake()
    {
        playerRB = GetComponent<Rigidbody2D>();
        jumpAmount = jumpAmountI;
        //myClaw.transform.position = new Vector2(transform.position.x + 0.5f, transform.position.y + 1f);
        //myRoar.transform.position = new Vector2(transform.position.x + (myRoar.GetComponent<SpriteRenderer>().size.x * 0.5f) + 2.5f, transform.position.y);
        playerSpeed = playerSpeedI;
        movingRight = true;
        climbingTimer = climbingTimerI;
        myAnimations = GetComponent<Animator>();
        scaleX = transform.localScale.x;
        scaleXNeg = transform.localScale.x * -1;

        transform.localScale = new Vector3(scaleXNeg, transform.localScale.y, 1);

        if (FindAnyObjectByType<SpawnPoint>() != null)
        {
            transform.position = FindAnyObjectByType<SpawnPoint>().transform.position;
        }
        else
        {
            transform.position = Vector2.zero;
        }
        
        DontDestroyOnLoad(gameObject);
    }


    void Start()
    {
        if (movingRight == false)
        {
            myClaw.transform.position = new Vector2(transform.position.x - 2.25f, transform.position.y + 1.75f);
            myRoar.transform.position = new Vector2(transform.position.x - (GetComponent<SpriteRenderer>().size.x * 0.5f) - 2.5f, transform.position.y);
        }
        else
        {
            myClaw.transform.position = new Vector2(transform.position.x + 2.25f, transform.position.y + 1.75f);
            myRoar.transform.position = new Vector2(transform.position.x + (GetComponent<SpriteRenderer>().size.x * 0.5f) + 2.5f, transform.position.y);
        }
    }

    void Update()
    {
        Physics2D.SyncTransforms();
        //Debug.Log(playerRB.linearVelocityX);
        playerPosition = transform.position;
        translationX = Input.GetAxis("Horizontal");
        translationY = Input.GetAxis("Vertical");

        /*if (Input.GetKeyUp(KeyCode.A))
        {
            movingLeftInExit = false;
        }

        if (Input.GetKeyUp(KeyCode.D))
        {
            movingRightInExit = false;
        }*/

        swipePointsL = new List<Vector3>() 
        {
            new Vector2(transform.position.x - 2.25f, transform.position.y + 1.75f),
            new Vector2(transform.position.x - 3f, transform.position.y),
            new Vector2(transform.position.x - 2.25f, transform.position.y - 1.75f)
        };

        swipePointsR = new List<Vector3>()
        {
            new Vector2(transform.position.x + 2.25f, transform.position.y + 1.75f),
            new Vector2(transform.position.x + 3f, transform.position.y),
            new Vector2(transform.position.x + 2.25f, transform.position.y - 1.75f)
        };

        //Debug.Log (playerRB.velocity);

        if (translationX < 0 && isBall == false)
        {
            if (isSwiping == false)
            {
                myClaw.transform.position = new Vector2(transform.position.x - 2.25f, transform.position.y + 1.75f);
                myRoar.transform.position = new Vector2(transform.position.x - (GetComponent<SpriteRenderer>().size.x * 0.5f) - 2.5f, transform.position.y);
                transform.localScale = new Vector3(scaleX, transform.localScale.y, 1);
            }

            movingRight = false;
        }
        else if (translationX > 0)
        {
            if (isSwiping == false)
            {
                myClaw.transform.position = new Vector2(transform.position.x + 2.25f, transform.position.y + 1.75f);
                myRoar.transform.position = new Vector2(transform.position.x + (GetComponent<SpriteRenderer>().size.x * 0.5f) + 2.5f, transform.position.y);
                transform.localScale = new Vector3(scaleXNeg, transform.localScale.y, 1);
            }

            movingRight = true;
        }

        //Roaring
        if (Input.GetKey(roarKey))
        {
            if (isBall == false)
            {
                myRoar.SetActive(true);
                isRoaring = true;
            }
            else
            {
                myRoar.SetActive(false);
                isRoaring = false;
            }
            
        }
        
        if (Input.GetKeyUp(roarKey))
        {
            myRoar.SetActive(false);
            isRoaring = false;
        }

        if (isBall == false)
        {
            if (isClimbing == false)
            {
                
                Vector2 checkPos = transform.position - (Vector3)new Vector2(0.0f, GetComponent<CapsuleCollider2D>().size.y / 2);
                SlopeCheckHorizontal(checkPos);
                SlopeCheckVertical(checkPos);

                if (isOnSlope == true && isGrounded == true)
                {
                    newVelocity.Set(playerSpeed * slopeNormalPerp.x * -translationX, playerSpeed * slopeNormalPerp.y * -translationX);
                    
                    if (isGrounded == false)
                    {
                        playerRB.linearVelocity = newVelocity;
                    }
                    else
                    {
                        playerRB.linearVelocity = new Vector2(playerRB.linearVelocity.x, 0);
                    }
                    
                    transform.Translate(newVelocity * Time.deltaTime * 2);
                }
                else
                {
                    transform.Translate(new Vector2(translationX, 0) * Time.deltaTime * playerSpeed);
                    playerRB.linearVelocity = new Vector2(translationX * (playerSpeed / 2), playerRB.linearVelocity.y);
                }
            }
            else
            {
                if (climbingTimer > 0)
                {
                    climbingTimer = climbingTimer - Time.deltaTime;

                    if (transform.position.x < whatImClimbing.transform.position.x)
                    {
                        transform.position = new Vector2(whatImClimbing.transform.position.x - GetComponent<Collider2D>().bounds.size.x, transform.position.y);
                    }
                    else if (transform.position.x > whatImClimbing.transform.position.x)
                    {
                        transform.position = new Vector2(whatImClimbing.transform.position.x + GetComponent<Collider2D>().bounds.size.x, transform.position.y);
                    }

                    playerRB.Sleep();

                    transform.Translate(new Vector2(0, translationY) * Time.deltaTime * playerSpeed);

                    if (transform.position.y > whatImClimbing.transform.position.y + whatImClimbing.GetComponent<SpriteRenderer>().bounds.size.y * 0.5f)
                    {
                        playerRB.WakeUp();
                        playerRB.AddForce(Vector2.up * jumpForce);
                        whatImClimbing = null;
                        isClimbing = false;
                    }
                    else if (transform.position.y < whatImClimbing.transform.position.y - whatImClimbing.GetComponent<SpriteRenderer>().bounds.size.y * 0.5f)
                    {
                        playerRB.WakeUp();
                        whatImClimbing = null;
                        isClimbing = false;
                    }
                }
                else
                {
                    playerRB.WakeUp();
                    whatImClimbing = null;
                    isClimbing = false;
                }
                
            }
            

            //Swipe Attack
            if (Input.GetKeyDown(clawKey) && isSwiping == false && isClimbing == false)
            {
                isSwiping = true;

                if(movingRight == false)
                {
                    StartCoroutine(ClawSwipe("Left"));
                    myAnimations.SetBool("AmSwiping", true);
                    myAnimations.Play("BearSwipe");
                }
                else
                {
                    StartCoroutine(ClawSwipe("Right"));
                    myAnimations.SetBool("AmSwiping", true);
                    myAnimations.Play("BearSwipe");
                }

                if (isSwiming == true)
                {
                    playerRB.linearVelocity = new Vector2(playerRB.linearVelocity.x, 0);
                    playerRB.AddForce(Vector2.up * swimUpForce);
                }
            }

        }

        if (myAnimations.GetBool("AmJumping") == true)
        {
            myAnimations.SetBool("AmJumping", false);
        }

        //Jump
        if (Input.GetKeyDown(jumpKey) && ((jumpAmount > 0) || (isClimbing == true)))
        {
            if (isClimbing == false)
            {
                if (isOnSlope == false)
                {
                    if (isOnMGround == false)
                    {
                        playerRB.AddForce(Vector2.up * jumpForce);
                    }
                    else
                    {
                        StartCoroutine("JumpOffMGround");
                    }

                    

                }
                else
                {
                    GetComponent<CapsuleCollider2D>().enabled = false;
                    playerRB.AddForce(Vector2.up * jumpForce);
                    GetComponent<CapsuleCollider2D>().enabled = true;
                }

                myAnimations.SetBool("AmJumping", true);
                myAnimations.Play("BearJump");
                jumpAmount = jumpAmount - 1;
                isGrounded = false;
            }
            else
            {
                if (transform.position.x < whatImClimbing.transform.position.x)
                {
                    playerRB.WakeUp();
                    playerRB.AddForce(new Vector2(-1,1) * jumpForce);
                    whatImClimbing = null;
                    isClimbing = false;
                }
                else if (transform.position.x > whatImClimbing.transform.position.x)
                {
                    playerRB.WakeUp();
                    playerRB.AddForce(new Vector2(1, 1) * jumpForce);
                    whatImClimbing = null;
                    isClimbing = false;
                }
            }
            
            //GetComponent<Rigidbody2D>().velocity = new Vector2 (GetComponent<Rigidbody2D>().velocity.x / 2, GetComponent<Rigidbody2D>().velocity.y);
        }

        //Transforms bear in and out of ball form
        if (isSwiping == false)
        {
            if (Input.GetKeyDown(rollKey))
            {
                playerRB.linearVelocity = new Vector2(playerRB.linearVelocity.x * 2f, playerRB.linearVelocity.y);
                playerRB.mass = 1;
                GetComponent<CapsuleCollider2D>().enabled = false;
                playerRB.constraints = RigidbodyConstraints2D.None;
                GetComponent<Renderer>().enabled = false;
                isBall = true;
                myBall.SetActive(true);
            }

            if (Input.GetKeyUp(rollKey))
            {
                GetComponent<CapsuleCollider2D>().enabled = true;
                playerRB.rotation = 0;
                transform.rotation = Quaternion.Euler(0, 0, 0);
                playerRB.mass = 1;
                playerRB.constraints = RigidbodyConstraints2D.FreezeRotation;
                GetComponent<Renderer>().enabled = true;
                isBall = false;
                myBall.SetActive(false);
            }
        }

        //Limits Speed in Ball Form
        if (isBall == true)
        {
            transform.position = myBall.transform.position;

            if (playerRB.linearVelocity.x > rollMaxVelocity)
            {
                playerRB.linearVelocity = new Vector2(rollMaxVelocity, playerRB.linearVelocity.y);
            }
            else if (playerRB.linearVelocity.x < rollMaxVelocity * -1)
            {
                playerRB.linearVelocity = new Vector2(rollMaxVelocity * -1, playerRB.linearVelocity.y);
            }
        }

        if (isClimbing == false)
        {
            WallTouching();
        }
        else
        {
            playerSpeed = playerSpeedI;
        }

        if ((translationX > 0.01f || translationX < -0.01f) && isBall == false && isGrounded == true)
        {
            myAnimations.SetBool("AmMoving", true);
        }
        else
        {
            myAnimations.SetBool("AmMoving", false);
        }

        if (isGrounded == false)
        {
            myAnimations.SetBool("AmInAir", true);
        }
        else
        {
            myAnimations.SetBool("AmInAir", false);
        }

        
       
    }

    IEnumerator JumpOffMGround()
    {
        GetComponent<Collider2D>().enabled = false;

        if (playerRB.linearVelocityY > 0)
        {
            
            playerRB.linearVelocityY = playerRB.linearVelocityY + 1;
        }
        else
        {
            playerRB.linearVelocityY = 0;
        }
        

        yield return new WaitForSeconds(0.1f);
        playerRB.AddForce(Vector2.up * (jumpForce + 100));

        GetComponent<Collider2D>().enabled = true;
    }

    IEnumerator ClawSwipe(string whichWay)
    {
        
        myClaw.GetComponent<Collider2D>().enabled = true;
        if (whichWay == "Left")
        {
            while (Vector2.Distance(myClaw.transform.position, swipePointsL[1]) > 0.1f)
            {
                myClaw.transform.position = Vector3.Slerp(myClaw.transform.position, swipePointsL[1], clawSpeed * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }

            while (Vector2.Distance(myClaw.transform.position, swipePointsL[2]) > 0.1f)
            {
                myClaw.transform.position = Vector3.Slerp(myClaw.transform.position, swipePointsL[2], clawSpeed * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }

            myClaw.transform.position = swipePointsL[0];
        }
        

        if (whichWay == "Right")
        {
            while (Vector2.Distance(myClaw.transform.position, swipePointsR[1]) > 0.1f)
            {
                myClaw.transform.position = Vector3.Slerp(myClaw.transform.position, swipePointsR[1], clawSpeed * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }

            while (Vector2.Distance(myClaw.transform.position, swipePointsR[2]) > 0.1f)
            {
                myClaw.transform.position = Vector3.Slerp(myClaw.transform.position, swipePointsR[2], clawSpeed * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }

            myClaw.transform.position = swipePointsR[0];

        }

        myClaw.GetComponent<Collider2D>().enabled = false;
        myAnimations.SetBool("AmSwiping", false);
        isSwiping = false;

        //StopAllCoroutines()
    }

    void WallTouching()
    {

        Vector2 startPos;
        Vector2 endPos;
        RaycastHit2D hit;

        if (movingRight == true)
        {
            startPos = gameObject.transform.position;
            endPos = gameObject.transform.right * 2f;
        }
        else
        {
            startPos = gameObject.transform.position;
            endPos = gameObject.transform.right * -2f ;
        }

        Debug.DrawRay(startPos, endPos);

        hit = Physics2D.Raycast(startPos, endPos, 2f);

        if (hit.transform != null && hit.transform.tag == "Ground")
        {
            //Debug.Log("Bruh");
            playerSpeed = 0;
        }
        else
        {
            playerSpeed = playerSpeedI;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Enemy")
        {
            playerHP = playerHP - 1;
            Debug.Log("Ouch! Player HP = " + playerHP);

            if (playerHP <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "EnemyBullet" || collision.gameObject.tag == "Trailer")
        {
            if (jumpAmount == 0)
            {

            }
            else
            {
                jumpAmount = jumpAmount - 1;
            }
        }

        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Slope" || collision.gameObject.tag == "OneWayPlatform")
        {
            isGrounded = false;
            playerSpeed = playerSpeedI;

            if (collision.transform.parent != null && collision.transform.parent.GetComponent<MovingGround>() != null)
            {
                isOnMGround = false;
            }

        }

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        foreach (ContactPoint2D hitPos in collision.contacts)
        {
            if ((collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Slope" || collision.gameObject.tag == "OneWayPlatform") && hitPos.normal.y > 0)
            {
                if (collision.gameObject.tag == "OneWayPlatform")
                {
                    if ((transform.position.y - GetComponent<Collider2D>().bounds.size.y / 2) > (collision.transform.position.y + collision.gameObject.GetComponent<Collider2D>().bounds.size.y / 2))
                    {
                        jumpAmount = jumpAmountI;
                        isGrounded = true;
                        climbingTimer = climbingTimerI;

                        if (collision.transform.parent.GetComponent<MovingGround>() != null && isBall == false)
                        {
                            GetComponent<Rigidbody2D>().MovePosition(Vector2.MoveTowards(transform.position, new Vector2(collision.transform.position.x, collision.transform.position.y + collision.gameObject.GetComponent<Collider2D>().bounds.size.y / 2), collision.transform.parent.GetComponent<MovingGround>().moveSpeed * Time.deltaTime));
                            isOnMGround = true;
                        }
                    }
                }
                else
                {
                    jumpAmount = jumpAmountI;
                    isGrounded = true;
                    climbingTimer = climbingTimerI;
                }
                
            }
            else
            {

            }
        }

        if (collision.gameObject.tag == "Slope" && isBall == true)
        {
            /*if (playerRB.linearVelocity.y >= 0)
            {
                
                playerRB.linearVelocity = new Vector2(playerRB.linearVelocity.x, 0);
            }*/

            if ((playerRB.linearVelocityX < 0 && playerRB.linearVelocityX > -0.7f) || (playerRB.linearVelocityX > 0 && playerRB.linearVelocityX < 0.7f))
            {
                if (collision.transform.position.x < transform.position.x)
                {
                    translationX = 0.25f;
                }
                else
                {
                    translationX = -0.25f;
                }

                Vector2 checkPos = transform.position - (Vector3)new Vector2(0.0f, GetComponent<CapsuleCollider2D>().size.y / 2);
                SlopeCheckHorizontal(checkPos);
                SlopeCheckVertical(checkPos);
                newVelocity.Set(playerSpeed * slopeNormalPerp.x * -translationX, playerSpeed * slopeNormalPerp.y * -translationX);
                transform.Translate(newVelocity * Time.deltaTime * 2);
            }

            if (playerRB.linearVelocityY < 0)
            {
                playerRB.linearVelocity = playerRB.linearVelocity * slopeVelocityMult;
            }
            
            //Debug.Log(playerRB.linearVelocityX);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Water")
        {
            playerRB.gravityScale = waterGravity;
            isSwiming = true;
            myAnimations.SetBool("AmSwiming", true);
        }
    }

    

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Water")
        {
            playerRB.gravityScale = 1;
            isSwiming = false;
            myAnimations.SetBool("AmSwiming", false);

            if (collision.gameObject.transform.position.y + (collision.GetComponent<SpriteRenderer>().bounds.size.y * 0.5f) < transform.position.y)
            {
                playerRB.AddForce(Vector2.up * outOfWaterUpForce);
            }
        }
    }

    

    private void SlopeCheckHorizontal(Vector2 checkPos)
    {
        RaycastHit2D slopeHitFront = Physics2D.Raycast(checkPos, transform.right, slopeCheckDistance);
        RaycastHit2D slopeHitBack = Physics2D.Raycast(checkPos, -transform.right, slopeCheckDistance);

        if (slopeHitFront)
        {
            isOnSlope = true;

            slopeSideAngle = Vector2.Angle(slopeHitFront.normal, Vector2.up);

        }
        else if (slopeHitBack)
        {
            isOnSlope = true;

            slopeSideAngle = Vector2.Angle(slopeHitBack.normal, Vector2.up);
        }
        else
        {
            slopeSideAngle = 0.0f;
            isOnSlope = false;
        }

    }



    private void SlopeCheckVertical(Vector2 checkPos)
    {
        RaycastHit2D hit = Physics2D.Raycast(checkPos, Vector2.down, slopeCheckDistance);

        if (hit)
        {

            slopeNormalPerp = Vector2.Perpendicular(hit.normal).normalized;

            slopeDownAngle = Vector2.Angle(hit.normal, Vector2.up);

            if (slopeDownAngle != lastSlopeAngle)
            {
                isOnSlope = true;
            }

            lastSlopeAngle = slopeDownAngle;

            Debug.DrawRay(hit.point, slopeNormalPerp, Color.blue);
            Debug.DrawRay(hit.point, hit.normal, Color.green);

        }

        if (slopeDownAngle > maxSlopeAngle || slopeSideAngle > maxSlopeAngle)
        {
            canWalkOnSlope = false;
        }
        else
        {
            canWalkOnSlope = true;
        }

        if (isOnSlope && canWalkOnSlope && translationX == 0.0f)
        {
            playerRB.sharedMaterial = fullFriction;
        }
        else
        {
            playerRB.sharedMaterial = noFriction;
        }

    }
}