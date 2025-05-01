using UnityEngine;

public class CanopyBird : MonoBehaviour
{
    public bool leftHeading;
    public int birdSpeed;
    private void awake()
    {

    }

    private void Update()
    {
        if (leftHeading == true)
        {
            transform.Translate(Vector2.left*birdSpeed*Time.deltaTime);
        }
        else{
            transform.Translate(Vector2.right*birdSpeed*Time.deltaTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {
            if(leftHeading == true)
            {
                leftHeading = false;
            }
            else
            {
                leftHeading = true;
            }
        }
    }

}
