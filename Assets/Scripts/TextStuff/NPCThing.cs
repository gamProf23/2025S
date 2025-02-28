using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCThing : MonoBehaviour
{
    //public string myText;

    public List<string> myText;
    public Sprite myPortrait;
    Bear player;
    private void Awake()
    {

    }
    void Start()
    {
        player = FindAnyObjectByType<Bear>();
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) && player.amBall == false && player.isGrounded == true && player.isTalking == false))
        {
            if ((player.transform.position.y < transform.position.y + (GetComponent<SpriteRenderer>().bounds.size.y / 2) && player.transform.position.y > transform.transform.position.y - (GetComponent<SpriteRenderer>().bounds.size.y / 2)) && (player.transform.position.x < transform.position.x + (GetComponent<SpriteRenderer>().bounds.size.x / 2) && player.transform.position.x > transform.position.x - (GetComponent<SpriteRenderer>().bounds.size.x / 2)))
            {
                if (FindAnyObjectByType<CanvasThing>().crRunning == false)
                {
                    FindAnyObjectByType<CanvasThing>().NPCTalk(myText, myPortrait);
                }
            }
        }
    }
}
