using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Fish : MonoBehaviour
{
    public Collectable myFish;
    Collectable fishClone;
    
    private void Awake()
    {
        if (FindAnyObjectByType<SceneInfo>().GetCDList().ContainsKey(gameObject.scene.name) == true && FindAnyObjectByType<SceneInfo>().GetCDList()[gameObject.scene.name].Contains(gameObject.name) == true)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    bool hasBeenHit = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Claw" && FindAnyObjectByType<Bear>().isSwiping == true)
        {
            if (myFish != null)
            {
                fishClone = Instantiate(myFish, transform.position, Quaternion.Euler(Vector3.zero));
            }

            myFish = null;
            FindAnyObjectByType<SceneInfo>().AddToCDList(gameObject.name);
            Destroy(gameObject);
        }

        if (collision.gameObject.tag == "Water")
        {
            GetComponent<Rigidbody2D>().gravityScale = 0;
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Water")
        {
            GetComponent<Rigidbody2D>().gravityScale = 1;
        }
    }
}
