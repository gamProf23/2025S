using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swipeable : MonoBehaviour
{
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Claw")
        {
            Destroy(gameObject);
        }
    }
}
