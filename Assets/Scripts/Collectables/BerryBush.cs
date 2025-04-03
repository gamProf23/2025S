using UnityEngine;

public class BerryBush : MonoBehaviour
{
    public Sprite bushNoBerry;
    public Collectable myBerry;
    Collectable berryClone;

    bool hasBeenPicked = false;

    private void Awake()
    {
        if (FindAnyObjectByType<SceneInfo>().GetCDList().ContainsKey(gameObject.scene.name) == true && FindAnyObjectByType<SceneInfo>().GetCDList()[gameObject.scene.name].Contains(gameObject.name) == true)
        {
            GetComponent<SpriteRenderer>().sprite = bushNoBerry;
            hasBeenPicked = true;
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Claw" && hasBeenPicked == false && FindAnyObjectByType<Bear>().isSwiping == true)
        {
            GetComponent<SpriteRenderer>().sprite = bushNoBerry;
            berryClone = Instantiate(myBerry, transform.position, transform.rotation);
            FindAnyObjectByType<SceneInfo>().AddToCDList(gameObject.name);
            hasBeenPicked = true;
        }
    }
}
