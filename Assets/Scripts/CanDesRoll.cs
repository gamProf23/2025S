using UnityEngine;

public class CanDesRoll : MonoBehaviour
{
    public float velocityToKill;

    public bool destroyForever;

    private void Awake()
    {
        if (FindAnyObjectByType<SceneInfo>().GetCDList().ContainsKey(gameObject.scene.name) == true && FindAnyObjectByType<SceneInfo>().GetCDList()[gameObject.scene.name].Contains(gameObject.name) == true && destroyForever == true)
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && collision.relativeVelocity.x > velocityToKill)
        {
            collision.gameObject.GetComponent<Rigidbody2D>().linearVelocity = collision.relativeVelocity;
            FindAnyObjectByType<SceneInfo>().AddToCDList(gameObject.name);
            Destroy(gameObject);
        }
    }
}
