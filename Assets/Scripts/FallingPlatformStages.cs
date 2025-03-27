using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;

public class FallingPlatformStages : MonoBehaviour
{
    public Sprite[] stageList;

    FallingPlatform dad;

    float stageMarker;

    Sprite currentStage;
    int currentStageInt = 0;

    private void Awake()
    {
        dad = transform.parent.GetComponent<FallingPlatform>();
        currentStage =  stageList[0];
        stageMarker = dad.timeTillFallI / stageList.Length;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        

        if (dad.timeTillFall < dad.timeTillFallI - (stageMarker*(currentStageInt+1)) && dad.timeTillFall > 0)
        {
            
            currentStageInt++;
            currentStage = stageList[currentStageInt];
            GetComponent<SpriteRenderer>().sprite = currentStage;
        }
        
        if (GetComponent<SpriteRenderer>().enabled == false)
        {
            currentStageInt = 0;
            currentStage = stageList[currentStageInt];
            GetComponent<SpriteRenderer>().sprite = currentStage;
        }
    }
}
