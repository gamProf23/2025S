using UnityEngine;

public class Roarable : MonoBehaviour
{
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

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "BearRoar")
        {
            FindAnyObjectByType<SceneInfo>().AddToCDList(gameObject.name);
            Destroy(gameObject);
        }

    }
}
