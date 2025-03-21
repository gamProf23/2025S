using System.Collections;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    

    public float timeTillFallI;
    float timeTillFall;

    public float fallDistance;
    Vector2 ogPos;

    public bool willRespawn;
    public float respawnTimer;

    public bool readyToFall = false;
    bool playerOnMe = false;

    

    private void Awake()
    {
        timeTillFall = timeTillFallI;
        ogPos = transform.position;
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (playerOnMe == false && timeTillFall < timeTillFallI && readyToFall == false)
        {
            timeTillFall = timeTillFall + Time.deltaTime;
        }

        if (readyToFall == true && transform.position.y <= ogPos.y - fallDistance)
        {
            if (willRespawn == true)
            {
                StartCoroutine(WaitToRespawn());
            }

            //GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<Rigidbody2D>().linearVelocity = Vector3.zero;
            GetComponent<Rigidbody2D>().gravityScale = 0;
            GetComponent<Rigidbody2D>().mass = 1000000;
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
            GetComponent<Rigidbody2D>().freezeRotation = true;

        }
    }

    IEnumerator WaitToRespawn()
    {
        yield return new WaitForSeconds(respawnTimer);

        //GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<BoxCollider2D>().enabled = true;
        transform.position = ogPos;
        timeTillFall = timeTillFallI;
        playerOnMe = false;
        readyToFall = false;

        
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        
        foreach (ContactPoint2D hitPos in collision.contacts)
        {
            if (hitPos.normal.y < 0 && collision.transform.tag == "Player" && readyToFall == false)
            {
               playerOnMe = true;
               timeTillFall = timeTillFall - Time.deltaTime;

                if (timeTillFall < 0)
                {
                    readyToFall = true;
                    GetComponent<Rigidbody2D>().gravityScale = 0.5f;
                    GetComponent<Rigidbody2D>().mass = 100;
                    GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX;
                    GetComponent<Rigidbody2D>().freezeRotation = true;
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            playerOnMe = false;
        }
    }
}