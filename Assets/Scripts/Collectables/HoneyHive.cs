using UnityEngine;

public class HoneyHive : MonoBehaviour
{
    public Collectable myHoney;
    Collectable honeyClone;

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Claw")
        {
            honeyClone = Instantiate(myHoney, transform.position, transform.rotation);
            FindAnyObjectByType<SceneInfo>().AddToCDList(gameObject.name);
            Destroy(gameObject);
        }
    }
}
