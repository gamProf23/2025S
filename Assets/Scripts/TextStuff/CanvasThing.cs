using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Dependencies.NCalc;
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

    Image textBox;
    TMP_Text textBoxText;
    Image textBoxPortrait;

    private void Awake()
    {
       berryText = transform.GetChild(0).GetComponent<TMP_Text>();
       fishText = transform.GetChild(1).GetComponent<TMP_Text>();
       honeyText = transform.GetChild(2).GetComponent<TMP_Text>();
       textBox = transform.GetChild(3).GetComponent<Image>();
       textBoxText = textBox.transform.GetChild(0).GetComponent<TMP_Text>();
       textBoxPortrait = textBox.transform.GetChild(1).GetComponent<Image>();
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

    public void NPCTalk(List<string> text, Sprite image)
    {
        StartCoroutine(NPCTalkCR(text, image));
    }

    public bool crRunning = false;
    public IEnumerator NPCTalkCR(List<string> text, Sprite image)
    {
        crRunning = true;
        FindAnyObjectByType<Bear>().isTalking = true;
        string emptyText = "";
        textBox.transform.gameObject.SetActive(true);
        textBoxPortrait.sprite = image;

        foreach (string s in text)
        {
            foreach (char c in s)
            {
                emptyText += c;
                textBoxText.text = emptyText;
                yield return new WaitForSeconds(0.05f);
            }

            while (Input.anyKey == false)
            {
                yield return new WaitForEndOfFrame();
            }

            emptyText = "";
        }
        

        textBox.transform.gameObject.SetActive(false);
        FindAnyObjectByType<Bear>().isTalking = false;
        crRunning = false;

    }

}
