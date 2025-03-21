using Unity.VisualScripting;
using UnityEngine;

public class Collectable : MonoBehaviour
{

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (gameObject.tag == "Berry")
            {
                FindAnyObjectByType<CanvasThing>().AddBerry();
            }
            else if (gameObject.tag == "Fish")
            {
                FindAnyObjectByType<CanvasThing>().AddFish();
            }
            else if (gameObject.tag == "Honey")
            {
                FindAnyObjectByType<CanvasThing>().AddHoney();
            }

            Destroy(gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Water")
        {
            GetComponent<Rigidbody2D>().gravityScale = 0;
        }
    }
}
