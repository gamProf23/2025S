using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamLimitDown : MonoBehaviour
{
    Vector2 startPos;
    Vector2 endPos;

    // Start is called before the first frame update
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
