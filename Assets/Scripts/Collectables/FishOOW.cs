using UnityEngine;

public class FishOOW : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.transform.tag == "Water")
        {
            transform.parent.GetComponent<MovingFish>().ChangeDirectionOOW();
        }
    }
}
