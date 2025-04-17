using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;

public class Wind : MonoBehaviour
{
    public float windTimer;
    public bool windOn = true;

    private void Awake()
    {
       

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        windTimer = windTimer + Time.deltaTime;
        if (windTimer >= 5f)
        {
            
            windOn = false;
            GetComponentInChildren<SpriteShapeRenderer>().enabled = false;
            GetComponent<PolygonCollider2D>().enabled = false;
            GetComponent<AreaEffector2D>().enabled = false;
            
        }
        if (windTimer >= 8f)
        {
            windOn = true;
            GetComponentInChildren<SpriteShapeRenderer>().enabled = true;
            GetComponent<PolygonCollider2D>().enabled = true;
            GetComponent<AreaEffector2D>().enabled = true;
            windTimer = 0f;
        }
    }
}
