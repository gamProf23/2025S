using UnityEngine;

public class CanDesRoll : MonoBehaviour
{
    public float velocityToKill;
    void Start()
    {
        
    }

    
    void Update()
    {
       
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && collision.relativeVelocity.x > velocityToKill)
        {
            collision.gameObject.GetComponent<Rigidbody2D>().linearVelocity = collision.relativeVelocity;
            Destroy(gameObject);
        }
    }
}
