using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class CanvasThing : MonoBehaviour
{
    public  int berryCount;
    public int fishCount;
    public int honeyCount;

    TMP_Text berryText;
    TMP_Text fishText;
    TMP_Text honeyText;

    private void Awake()
    {
       berryText = transform.GetChild(0).GetComponent<TMP_Text>();
       fishText = transform.GetChild(1).GetComponent<TMP_Text>();
       honeyText = transform.GetChild(2).GetComponent<TMP_Text>();
       DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        
    }
    void Update()
    {
        
    }

    public void AddBerry()
    {
        berryCount++;
        berryText.text = "B: " + berryCount.ToString();
    }

    public void AddFish()
    {
        fishCount++;
        fishText.text = "F: " + fishCount.ToString();
    }

    public void AddHoney()
    {
        honeyCount++;
        honeyText.text = "H: " + honeyCount.ToString();
    }
}
