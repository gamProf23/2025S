using System.Collections;
using UnityEngine;

public class Salmon : MonoBehaviour
{
    bool firstFrame = true;
    public float upDistance;
    public float downDistance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(AfterFirstFrame());
    }

    

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Water" && firstFrame == false)
        {
            GetComponent<Rigidbody2D>().linearVelocityY = downDistance * -1;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Water" && firstFrame == false)
        {
            GetComponent<Rigidbody2D>().linearVelocityY = upDistance;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Water")
        {
            GetComponent<Rigidbody2D>().AddForceY(10);
        }

    }

    IEnumerator AfterFirstFrame()
    {
        yield return null;
        firstFrame = false;
    }
}
