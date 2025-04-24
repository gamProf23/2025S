using UnityEngine;

public class RoarSwitch : MonoBehaviour
{
    public int whenImOn;
    void Start()
    {
         
    }

    void Update()
    {
        if (FindAnyObjectByType<Bear>().roarSwitch == whenImOn)
        {
            GetComponent<Collider2D>().enabled = true;
            GetComponent<SpriteRenderer>().color = new Color(GetComponent<SpriteRenderer>().color.r, GetComponent<SpriteRenderer>().color.g, GetComponent<SpriteRenderer>().color.b, 1);
        }
        else
        {
            GetComponent<Collider2D>().enabled = false;
            GetComponent<SpriteRenderer>().color = new Color(GetComponent<SpriteRenderer>().color.r, GetComponent<SpriteRenderer>().color.g, GetComponent<SpriteRenderer>().color.b, 0.5f);
        }
    }
}
