using UnityEngine;

public class MovingFish : MonoBehaviour
{
    public int fishSpeed;
    bool movingRight = false;
    float scaleX;
    float scaleXNeg;

    private void Awake()
    {
        scaleX = transform.localScale.x;
        scaleXNeg = transform.localScale.x * -1;
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ChangeDirection();

        if (movingRight == false)
        {
            transform.Translate(Vector2.left * fishSpeed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector2.right * fishSpeed * Time.deltaTime);
        }

    }

    void ChangeDirection()
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
            endPos = gameObject.transform.right * -2f;
        }

        Debug.DrawRay(startPos, endPos);

        hit = Physics2D.Raycast(startPos, endPos, 2f);

        if (hit.transform != null && (hit.transform.tag == "Ground" || hit.transform.tag == "Slope"))
        {
            if (movingRight == false)
            {
                movingRight = true;
                transform.localScale = new Vector3(scaleXNeg, transform.localScale.y, 1);
            }
            else
            {
                movingRight = false;
                transform.localScale = new Vector3(scaleX, transform.localScale.y, 1);
            }
        }
    }

    public void ChangeDirectionOOW()
    {
        if (movingRight == false)
        {
            movingRight = true;
            transform.localScale = new Vector3(scaleXNeg, transform.localScale.y, 1);
        }
        else
        {
            movingRight = false;
            transform.localScale = new Vector3(scaleX, transform.localScale.y, 1);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
       
    }
}
