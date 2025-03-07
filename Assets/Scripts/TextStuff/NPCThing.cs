using NUnit.Framework;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class NPCThing : MonoBehaviour
{
    //public string myText;

    public List<string> myText;
    public Sprite myPortrait;
    Bear player;

    public string myInteractionYSel;
    public string myInteractionNSel;

    public string myInteractionText;

    public int berryCondition;
    public int fishCondition;
    public int honeyCondition;

    public List<string> myInteractionYText;
    public List<string> myInteractionNText;
    public List<string> myInteractionFailText;

    public List<string> conditionMetText;

    public bool conditionIsMet;

    private void Awake()
    {
        if (FindAnyObjectByType<SceneInfo>().GetCMList().ContainsKey(gameObject.scene.name) == true && FindAnyObjectByType<SceneInfo>().GetCMList()[gameObject.scene.name].Contains(gameObject.name) == true)
        {
            conditionIsMet = true;
        }
        else
        {
            conditionIsMet = false;
        }
    }
    void Start()
    {
        player = FindAnyObjectByType<Bear>();
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) && player.isBall == false && player.isGrounded == true && player.isTalking == false && player.isBall == false))
        {
            if ((player.transform.position.y < transform.position.y + (GetComponent<SpriteRenderer>().bounds.size.y / 2) && player.transform.position.y > transform.transform.position.y - (GetComponent<SpriteRenderer>().bounds.size.y / 2)) && (player.transform.position.x < transform.position.x + (GetComponent<SpriteRenderer>().bounds.size.x / 2) && player.transform.position.x > transform.position.x - (GetComponent<SpriteRenderer>().bounds.size.x / 2)))
            {
                if (FindAnyObjectByType<CanvasThing>().crRunning == false)
                {
                    if (conditionIsMet == false)
                    {
                        FindAnyObjectByType<CanvasThing>().NPCTalk(myText, myPortrait, GetComponent<NPCThing>());
                    }
                    else
                    {
                        FindAnyObjectByType<CanvasThing>().NPCTalk(conditionMetText, myPortrait, GetComponent<NPCThing>());
                    }
                    
                }
            }
        }
    }
}
