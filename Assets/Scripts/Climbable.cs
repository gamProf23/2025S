using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Climbable : MonoBehaviour
{

    private void Awake()
    {

    }

    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "BearClaw" && FindAnyObjectByType<Bear>().isGrounded == false)
        {
            FindAnyObjectByType<Bear>().isClimbing = true;
            FindAnyObjectByType<Bear>().whatImClimbing = gameObject;
            FindAnyObjectByType<Bear>().PlayClawSound();
        }

        


    }

    public void GetOnMe()
    {
        if (FindAnyObjectByType<Bear>().isSwiping == true && FindAnyObjectByType<Bear>().isGrounded == false)
        {
            FindAnyObjectByType<Bear>().isClimbing = true;
            FindAnyObjectByType<Bear>().whatImClimbing = gameObject;
        }
    }
}
