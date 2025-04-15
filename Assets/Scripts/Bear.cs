using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Loading;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;

//using UnityEngine.Windows;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Bear : MonoBehaviour
{





    [Header("Can Edit")]

    public int playerHP = 10;

    public float playerSpeedI; //5
    float playerSpeed;

    //In Seconds
    public float climbingTimerI; //2
    

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


    [Header("Sound Stuff")]

   

    public List<AudioClip> roarSounds;
    public List<AudioClip> clawSounds;
    public List<AudioClip> walkSounds;
    public List<AudioClip> jumpSounds;
    public List<AudioClip> landingSounds;
    public List<AudioClip> splashSounds;
    public List<AudioClip> swimingSounds;
    public List<AudioClip> turningBallSounds;
    public List<AudioClip> rollingSounds;
    public List<AudioClip> climbingSounds;
    public List<AudioClip> collectionSounds;
    System.Random randomSoundInt;

    [Header("DO NOT TOUCH")]

    public float climbingTimer;
    public GameObject playerObject;
    public Rigidbody2D playerRB;
    public Vector2 playerPosition;

    public float translationX;
    public float translationY;

    public bool isClimbing = false;
    public GameObject whatImClimbing;

    public bool isSwiming = false;

    public GameObject myRoar;
    public bool isRoaring = false;

    public GameObject myBall;
    public bool isBall = false;

    public GameObject myClaw;
    public bool isSwiping = false;

    bool isOnMGround = false;

    public bool isGrounded = false;
    public bool movingRight = true;

    public bool isTalking = false;

    List<Vector3> swipePointsL = new List<Vector3>();
    List<Vector3> swipePointsR = new List<Vector3>();

    public Animator myAnimations;

    float scaleX;
    float scaleXNeg;

    Vector3 ogScale;

    float ogGrav;

    public int roarSwitch = 1;

    public List<KeyCode> keys = new List<KeyCode>();
    bool defaultKeysHaveBeenSet = false;
    CanvasThing canvasThing;

    public AudioSource track1;
    public AudioSource track2;
    public AudioSource track3;



    [Header("Slope Stuff: DO NOT TOUCH")]

    private float slopeCheckDistance = 0.5f;
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
        ogGrav = GetComponent<Rigidbody2D>().gravityScale;
        randomSoundInt = new System.Random();
        track1 = transform.GetChild(4).GetComponent<AudioSource>();
        track2 = transform.GetChild(5).GetComponent<AudioSource>();
        track3 = transform.GetChild(6).GetComponent<AudioSource>();
        

        transform.localScale = new Vector3(scaleXNeg, transform.localScale.y, 1);

        if (FindAnyObjectByType<SpawnPoint>() != null)
        {
            transform.position = FindAnyObjectByType<SpawnPoint>().transform.position;
        }
        else
        {
            transform.position = Vector2.zero;
        }

        foreach (KeyCode keycode in Enum.GetValues(typeof(KeyCode)))
        {
            if (keycode.ToString().Contains("Joy") == false)
            {
                keys.Add(keycode);
            }

        }

        DontDestroyOnLoad(gameObject);
    }


    void Start()
    {

        canvasThing = FindAnyObjectByType<CanvasThing>();
        if (movingRight == false)
        {
            myClaw.transform.position = new Vector2(transform.position.x - (2.25f/6), transform.position.y + (1.75f/6));
            myRoar.transform.position = new Vector2(transform.position.x - (GetComponent<SpriteRenderer>().size.x * 0.5f) - (2.5f/6), transform.position.y);
        }
        else
        {
            myClaw.transform.position = new Vector2(transform.position.x + (2.25f/6), transform.position.y + (1.75f/6));
            myRoar.transform.position = new Vector2(transform.position.x + (GetComponent<SpriteRenderer>().size.x * 0.5f) + (2.5f/6), transform.position.y);
        }
    }


    void Update()
    {
        
        Physics2D.SyncTransforms();
        playerPosition = transform.position;

        //Controls Stuff
        if (canvasThing.controlOptions.isActiveAndEnabled == true && defaultKeysHaveBeenSet == false)
        {
            canvasThing.jumpDrop.value = keys.IndexOf(jumpKey);
            canvasThing.rollDrop.value = keys.IndexOf(rollKey);
            canvasThing.roarDrop.value = keys.IndexOf(roarKey);
            canvasThing.clawDrop.value = keys.IndexOf(clawKey);
            defaultKeysHaveBeenSet = true;
        }
        else if (canvasThing.controlOptions.isActiveAndEnabled == true)
        {
            jumpKey = keys[canvasThing.jumpDrop.value];
            rollKey = keys[canvasThing.rollDrop.value];
            roarKey = keys[canvasThing.roarDrop.value];
            clawKey = keys[canvasThing.clawDrop.value];
        }

        if (isTalking == false)
        {
            translationX = Input.GetAxis("Horizontal");
            translationY = Input.GetAxis("Vertical");
        }
        else
        {
            translationX = 0;
            translationY = 0;
        }
        

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
            new Vector2(transform.position.x - (2.25f/6), transform.position.y + (1.75f/6)),
            new Vector2(transform.position.x - (3f/6), transform.position.y),
            new Vector2(transform.position.x - (2.25f/6), transform.position.y - (1.75f/6))
        };

        swipePointsR = new List<Vector3>()
        {
            new Vector2(transform.position.x + (2.25f/6), transform.position.y + (1.75f/6)),
            new Vector2(transform.position.x + (3f/6), transform.position.y),
            new Vector2(transform.position.x + (2.25f/6), transform.position.y - (1.75f/6))
        };

        //Debug.Log (playerRB.velocity);

        if (translationX < 0 && isBall == false)
        {
            if ((track2.time == 0 || track2.time == track2.clip.length) && isGrounded == true && isBall == false)
            {
                track2.clip = walkSounds[randomSoundInt.Next(walkSounds.Count)];
                track2.Play();
            }
            else if (isGrounded == false)
            {
                track2.Stop();
            }

            if (isSwiping == false)
            {
                myClaw.transform.position = new Vector2(transform.position.x - (2.25f/6), transform.position.y + (1.75f/6));
                myRoar.transform.position = new Vector2(transform.position.x - (GetComponent<SpriteRenderer>().size.x * 0.5f) - (2.5f/6), transform.position.y + (1f/6));
                

                if (isClimbing == false)
                {
                    transform.localScale = new Vector3(scaleX, transform.localScale.y, 1);
                }
                
            }

            movingRight = false;
        }
        else if (translationX > 0 && isBall == false)
        {
            if ((track2.time == 0 || track2.time == track2.clip.length) && isGrounded == true && isBall == false)
            {
                track2.clip = walkSounds[randomSoundInt.Next(walkSounds.Count)];
                track2.Play();
            }
            else if (isGrounded == false)
            {
                track2.Stop();
            }

            if (isSwiping == false)
            {
                myClaw.transform.position = new Vector2(transform.position.x + (2.25f/6), transform.position.y + (1.75f/6));
                myRoar.transform.position = new Vector2(transform.position.x + (GetComponent<SpriteRenderer>().size.x * 0.5f) + (2.5f/6), transform.position.y + (1f/6));

                if (isClimbing == false)
                {
                    transform.localScale = new Vector3(scaleXNeg, transform.localScale.y, 1);
                }
                
            }

            movingRight = true;
        }

        if (isClimbing == true)
        {
            if ((track3.time == 0 || track3.time == track3.clip.length) && translationY != 0 && isBall == false)
            {
                track3.clip = climbingSounds[randomSoundInt.Next(climbingSounds.Count)];
                track3.Play();
            }
            else if (translationY == 0)
            {
                track3.Stop();
            }
        }

        if(isClimbing == false && climbingSounds.Contains(track3.clip))
        {
            track3.Stop();
        }

        //Roaring
        if (Input.GetKey(roarKey) && isTalking == false)
        {
            if (isRoaring == false)
            {
                myAnimations.Play("BearRoar");
            }
            
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

            if (movingRight == false)
            {
                myRoar.transform.GetChild(0).transform.gameObject.SetActive(true);
                myRoar.transform.GetChild(1).transform.gameObject.SetActive(false);
            }
            else
            {
                myRoar.transform.GetChild(0).transform.gameObject.SetActive(false);
                myRoar.transform.GetChild(1).transform.gameObject.SetActive(true);
            }
            
        }

        if (Input.GetKeyDown(roarKey) && isTalking == false)
        {
            track1.clip = roarSounds[randomSoundInt.Next(roarSounds.Count)];
            track1.Play();
            //GetComponent<AudioSource>().clip = null;

            if (roarSwitch == 1)
            {
                roarSwitch = 2;
            }
            else if (roarSwitch == 2)
            {
                roarSwitch = 1;
            }
        }
        
        if (Input.GetKeyUp(roarKey) && isTalking == false)
        {
            myRoar.SetActive(false);
            isRoaring = false;
        }


        if (isClimbing == false && isBall == false)
        {
            WallTouching();
        }
        else
        {
            playerSpeed = playerSpeedI;
        }

        if (isBall == false)
        {
            track1.pitch = 1;
            if (isClimbing == false)
            {
                
                Vector2 checkPos = transform.position - (Vector3)new Vector2(0.0f, GetComponent<CapsuleCollider2D>().size.y/ 2);
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
                        transform.localScale = new Vector3(scaleXNeg, transform.localScale.y, 1);
                    }
                    else if (transform.position.x > whatImClimbing.transform.position.x)
                    {
                        transform.position = new Vector2(whatImClimbing.transform.position.x + GetComponent<Collider2D>().bounds.size.x, transform.position.y);
                        transform.localScale = new Vector3(scaleX, transform.localScale.y, 1);
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
                track1.clip = jumpSounds[0];
                track1.Play();

                isSwiping = true;

                if (movingRight == false)
                {
                    StartCoroutine(ClawSwipe("Left"));
                    myAnimations.SetBool("AmSwiping", true);

                    if (translationX == 0)
                    {
                        myAnimations.Play("BearSwipe");
                    }
                    else
                    {
                        myAnimations.Play("BearSwipeMove");
                    }
                    
                }
                else
                {
                    StartCoroutine(ClawSwipe("Right"));
                    myAnimations.SetBool("AmSwiping", true);

                    if (translationX == 0)
                    {
                        myAnimations.Play("BearSwipe");
                    }
                    else
                    {
                        myAnimations.Play("BearSwipeMove");
                    }
                }

                if (isSwiming == true)
                {
                    playerRB.linearVelocity = new Vector2(playerRB.linearVelocity.x, 0);
                    
                    if (isGrounded == false)
                    {
                        playerRB.AddForce(Vector2.up * swimUpForce);
                    }
                    else
                    {
                        playerRB.AddForce(Vector2.up * (swimUpForce));
                    }
                    
                }
            }

        }

        if (myAnimations.GetBool("AmJumping") == true)
        {
            myAnimations.SetBool("AmJumping", false);
        }

        /*if (isOnFallingP == true)
        {
            playerRB.AddForceY(-10);
        }*/

        //Jump
        if (Input.GetKeyDown(jumpKey) && ((jumpAmount > 0) || (isClimbing == true)) && isTalking == false)
        {   
            track1.clip = jumpSounds[randomSoundInt.Next(jumpSounds.Count)];
            track1.Play();
            
            

            if (isClimbing == false)
            {
                if (isOnSlope == false)
                {
                    if (isOnMGround == false)
                    {

                        if (isSwiming == true)
                        {
                            playerRB.AddForce(Vector2.up * jumpForce/1.5f);
                        }
                        else if (isOnFallingP == true)
                        {
                            StartCoroutine("JumpOffMGround");
                        }
                        else
                        {
                            playerRB.AddForce(Vector2.up * jumpForce);
                        }
                        

                    }
                    else
                    {
                        StartCoroutine("JumpOffMGround");
                    }

                }
                else
                {
                    if (isBall == true)
                    {
                        if (isOnSlope == true)
                        {
                            StartCoroutine("JumpOffSGround");
                        }
                        else
                        {
                            playerRB.AddForce(Vector2.up * jumpForce);
                        }
                        
                    }
                    else
                    {
                        GetComponent<CapsuleCollider2D>().enabled = false;
                        playerRB.AddForce(Vector2.up * jumpForce);
                        GetComponent<CapsuleCollider2D>().enabled = true;
                    }
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
                track2.clip = turningBallSounds[randomSoundInt.Next(turningBallSounds.Count)];
                track2.Play();
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

        

        //Animation Stuff
        if ((translationX > 0.01f || translationX < -0.01f) && isBall == false && isGrounded == true)
        {
            myAnimations.SetBool("AmMoving", true);
        }
        else
        {
            //myAnimations.SetBool("AmMoving", false);

            if ((translationY > 0.01f || translationY < -0.01f) && isBall == false && isGrounded == false)
            {
                myAnimations.SetBool("AmMoving", true);
            }
            else
            {
                myAnimations.SetBool("AmMoving", false);
            }
        }

        

        if (isGrounded == false)
        {
            myAnimations.SetBool("AmInAir", true);
        }
        else
        {
            myAnimations.SetBool("AmInAir", false);
        }

        if (isClimbing == true)
        {
            myAnimations.SetBool("AmClimbing", true);
        }
        else
        {
            myAnimations.SetBool("AmClimbing", false);
        }

        if (isRoaring == true)
        {
            myAnimations.SetBool("AmRoaring", true);
        }
        else
        {
            myAnimations.SetBool("AmRoaring", false);
        }

        
       
    }

    IEnumerator JumpOffSGround()
    {
        //GetComponent<CircleCollider2D>().enabled = false;

        if (playerRB.linearVelocityY < -1f)
        {
            playerRB.linearVelocityY = 0;
        }
        

        yield return new WaitForSeconds(0.1f);

        if (Math.Abs(playerRB.linearVelocityX) > 3)
        {
            playerRB.AddForce(Vector2.up * (jumpForce / 2));
        }
        else
        {
            playerRB.AddForce(Vector2.up * (jumpForce / 1));
        }
        

        //GetComponent<CircleCollider2D>().enabled = true;
        isOnSlope = false;
    }

    IEnumerator JumpOffMGround()
    {
        GetComponent<Collider2D>().enabled = false;


        if (playerRB.linearVelocityY > 0.1f)
        {
            
            playerRB.linearVelocityY = playerRB.linearVelocityY + 0.5f;
        }
        else
        {
            playerRB.linearVelocityY = 0;
        }
        

        /*if (playerRB.linearVelocityY > 0)
        {
            
            playerRB.linearVelocityY = playerRB.linearVelocityY + 1;
        }
        else
        {
            playerRB.linearVelocityY = 0;
        }*/

        playerRB.linearVelocityY = playerRB.linearVelocityY + 0.5f;


        yield return new WaitForSeconds(0.1f);

        if (translationX == 0)
        {
            playerRB.AddForce(Vector2.up * (jumpForce));
        }
        else
        {
            if (isOnFallingP == true)
            {
                playerRB.AddForce(Vector2.up * (jumpForce));
            }
            else
            {
                playerRB.AddForce(Vector2.up * (jumpForce + 40));
            }
            
        }
        

        GetComponent<Collider2D>().enabled = true;
    }

    IEnumerator ClawSwipe(string whichWay)
    {
        
        //myClaw.GetComponent<Collider2D>().enabled = true;
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

        //myClaw.GetComponent<Collider2D>().enabled = false;

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
            endPos = gameObject.transform.right * (2f/6);
        }
        else
        {
            startPos = gameObject.transform.position;
            endPos = gameObject.transform.right * (-2f/6);
        }

        Debug.DrawRay(startPos, endPos);

        hit = Physics2D.Raycast(startPos, endPos, (2f /6));

        if (hit.transform != null && hit.transform.tag == "Ground")
        {
            //Debug.Log("Bruh");
            playerSpeed = 0;
            playerRB.linearVelocityX = 0;
            
            if (movingRight == true)
            {
                transform.position = new Vector3(transform.position.x - 0.001f, transform.position.y);
            }
            else if (movingRight == false)
            {
                transform.position = new Vector3(transform.position.x + 0.001f, transform.position.y);
            }
        }
        else
        {
            playerSpeed = playerSpeedI;
        }
    }

    public void PlayClawSound()
    {
        track1.clip = clawSounds[randomSoundInt.Next(clawSounds.Count)];
        track1.Play();
    }

    public void PlayCollectionSound()
    {
        track1.clip = collectionSounds[randomSoundInt.Next(collectionSounds.Count)];
        track1.Play();
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

        if (collision.transform.parent != null && collision.transform.parent.GetComponent<MovingGround>() != null)
        {
            whereToClone = collision.transform.parent.GetComponent<MovingGround>().whereTo;
        }

        if (collision.gameObject.tag == "Ground" && isBall == false)
        {
            track2.clip = landingSounds[randomSoundInt.Next(landingSounds.Count)];
            track2.Play();
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
            isOnFallingP = false;

            if (collision.transform.parent != null && collision.transform.parent.GetComponent<MovingGround>() != null)
            {
                isOnMGround = false;
                myMGround = null;
                whereToClone = 0;
            }

        }

    }

    public GameObject myMGround;
    bool isOnFallingP = false;
    int whereToClone;
    bool jeff1 = false;
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

                        if (collision.transform.parent != null && collision.transform.parent.GetComponent<MovingGround>() != null && isBall == false)
                        {
                            GetComponent<Rigidbody2D>().MovePosition(Vector2.MoveTowards(transform.position, new Vector2(collision.transform.position.x, collision.transform.position.y + collision.gameObject.GetComponent<Collider2D>().bounds.size.y / 2), collision.transform.parent.GetComponent<MovingGround>().moveSpeed * Time.deltaTime));
                            myMGround = collision.gameObject;
                            playerRB.AddRelativeForceY(-10 * collision.transform.parent.GetComponent<MovingGround>().moveSpeed);
                            isOnMGround = true;

                            if (whereToClone != collision.transform.parent.GetComponent<MovingGround>().whereTo)
                            {
                                playerRB.linearVelocityY = 0;
                                whereToClone = collision.transform.parent.GetComponent<MovingGround>().whereTo;
                            }
                        }

                        if (collision.transform.GetComponent<FallingPlatform>() != null && collision.transform.GetComponent<FallingPlatform>().readyToFall == true)
                        {
                            isOnFallingP = true;
                        }
                        else
                        {
                            isOnFallingP = false;
                        }
                    }
                }
                else if (collision.gameObject.tag == "Ground")
                {
                    jumpAmount = jumpAmountI;
                    isGrounded = true;
                    climbingTimer = climbingTimerI;

                    if (collision.transform.parent != null && collision.transform.parent.GetComponent<MovingGround>() != null && isBall == false)
                    {
                        GetComponent<Rigidbody2D>().MovePosition(Vector2.MoveTowards(transform.position, new Vector2(collision.transform.position.x, collision.transform.position.y + collision.gameObject.GetComponent<Collider2D>().bounds.size.y / 2), collision.transform.parent.GetComponent<MovingGround>().moveSpeed * Time.deltaTime));
                        myMGround = collision.gameObject;
                        playerRB.AddRelativeForceY(-10 * collision.transform.parent.GetComponent<MovingGround>().moveSpeed);
                        isOnMGround = true;

                        if (whereToClone != collision.transform.parent.GetComponent<MovingGround>().whereTo)
                        {
                            playerRB.Sleep();
                            playerRB.WakeUp();
                            whereToClone = collision.transform.parent.GetComponent<MovingGround>().whereTo;
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
            isOnSlope = true;
            if (jeff1 == false)
            {
                StartCoroutine("RollingSounds");
            }

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
        else
        {
            isOnSlope = false;
        }
    }

    IEnumerator RollingSounds()
    {
        jeff1 = true;
        if(playerRB.linearVelocityX != 0)
        {
            yield return new WaitForSeconds(0.2f);
            track1.clip = rollingSounds[randomSoundInt.Next(rollingSounds.Count)];
            track1.pitch = track1.pitch + 0.1f;
            track1.Play();
            jeff1 = false;
        }
    }

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Water" && splashSounds.Contains(track1.clip) == false && isRoaring == false && isBall == false)
        {
            track1.clip = splashSounds[randomSoundInt.Next(splashSounds.Count)];
            track1.Play();
        }
    }

    float waterSpeed = 0.75f;
    float playerSpeedIHold = 1f;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Water")
        {
            playerRB.gravityScale = waterGravity;
            isSwiming = true;
            myAnimations.SetBool("AmSwiming", true);
            climbingTimer = climbingTimerI;

            if (playerSpeedI != waterSpeed)
            {
                playerSpeedIHold = playerSpeedI;
            }
            
            if (isGrounded == false)
            {
                playerSpeedI = playerSpeedIHold;
            }
            else
            {
                playerSpeedI = waterSpeed;
            }
            
        }

        if (collision.gameObject.tag == "ClimbPass")
        {
            collision.GetComponent<Climbable>().GetOnMe();
        }
    }

    

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Water")
        {
            playerRB.gravityScale = ogGrav;
            isSwiming = false;
            myAnimations.SetBool("AmSwiming", false);
            playerSpeedI = playerSpeedIHold;

            if (playerRB.linearVelocityY > 0)
            {
                playerRB.AddForce(Vector2.up * outOfWaterUpForce);
            }
            /*if (collision.gameObject.transform.position.y + (collision.GetComponent<Collider2D>().bounds.size.y * 0.5f) < transform.position.y)
            {
                playerRB.AddForce(Vector2.up * outOfWaterUpForce);
            }*/
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