using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamLimitL : MonoBehaviour
{
    Vector2 startPos;
    Vector2 endPos;
    // Start is called before the first frame update
    void Start()
    {
        startPos = new Vector2 (transform.position.x, transform.position.y + 50);
        endPos = new Vector2(transform.position.x, transform.position.y - 50);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawLine(startPos, endPos);
    }
}
