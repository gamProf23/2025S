using UnityEngine;

public class CamLimitUp : MonoBehaviour
{
    Vector2 startPos;
    Vector2 endPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPos = new Vector2(transform.position.x + 50, transform.position.y);
        endPos = new Vector2(transform.position.x - 50, transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawLine(startPos, endPos);
    }
}
