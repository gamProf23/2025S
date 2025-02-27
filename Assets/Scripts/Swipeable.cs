using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swipeable : MonoBehaviour
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

    
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Claw")
        {
            FindAnyObjectByType<SceneInfo>().AddToCDList(gameObject.name);
            Destroy(gameObject);
        }
    }
}
