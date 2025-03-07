using UnityEngine;

public class MovingGround : MonoBehaviour
{
    Vector3 mp1;
    Vector3 mp2;
    GameObject movingThing;
    public float moveSpeed;

    int whereTo = 1;

    private void Awake()
    {
        movingThing = transform.GetChild(0).gameObject;
        mp1 = transform.GetChild(1).transform.position;
        mp2 = transform.GetChild(2).transform.position;

        Destroy(transform.GetChild(1).gameObject);
        Destroy(transform.GetChild(2).gameObject);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Debug.DrawLine(mp1, mp2);

        if (whereTo == 1)
        {
            movingThing.GetComponent<Rigidbody2D>().MovePosition(Vector2.MoveTowards(movingThing.transform.position, mp1, moveSpeed * Time.deltaTime));

            if (Vector2.Distance(movingThing.transform.position, mp1) < 0.01f)
            {
                whereTo = 2;

                /*if (FindAnyObjectByType<Bear>().myMGround == movingThing)
                {
                    Debug.Log("bruh");
                    //FindAnyObjectByType<Bear>().GetComponent<Rigidbody2D>().AddRelativeForceY(-1000);
                }  
                */
            }
            
            

        }
        else if (whereTo == 2)
        {
            movingThing.GetComponent<Rigidbody2D>().MovePosition(Vector2.MoveTowards(movingThing.transform.position, mp2, moveSpeed * Time.deltaTime));

            if (Vector2.Distance(movingThing.transform.position, mp2) < 0.01f)
            {
                whereTo = 1;

                /*if (FindAnyObjectByType<Bear>().myMGround == movingThing)
                {
                    Debug.Log("bruh");
                    //FindAnyObjectByType<Bear>().GetComponent<Rigidbody2D>().AddRelativeForceY(-1000);
                }
                */
            }

            
        }

        

        
    }

    public Vector3 GetMoveTowards()
    {
        if (whereTo == 1)
        {
            return Vector2.MoveTowards(movingThing.transform.position, mp1, moveSpeed * Time.deltaTime);

        }
        else if (whereTo == 2)
        {
            return Vector2.MoveTowards(movingThing.transform.position, mp2, moveSpeed * Time.deltaTime);
        }

        return Vector2.zero;
    }
}
