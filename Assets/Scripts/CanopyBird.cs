using UnityEngine;

public class CanopyBird : MonoBehaviour
{
    public bool leftHeading;
    public int birdSpeed;
    public SpriteRenderer birdSprite;
    private void awake()
    {

        
    }

    private void Start(){
        if (leftHeading != true)
        {
            flipBird();
        }
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
                flipBird();
            }
            else
            {
                leftHeading = true;
                flipBird();
            }
        }
    }

    private void flipBird()
    {
        Vector2 birdOrientation = birdSprite.gameObject.transform.localScale;
        birdOrientation.x *= -1;
        birdSprite.gameObject.transform.localScale = birdOrientation;
    }

}
