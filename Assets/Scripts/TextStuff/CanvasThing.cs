using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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

    public Sprite bearPortrait;
    bool keyPressed = false;

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
        if (Input.anyKeyDown == true)
        {
            keyPressed = true;
        }
        else
        {
            keyPressed = false;
        }
    }

    public void AddBerry()
    {
        berryCount++;
        berryText.text = " " + berryCount.ToString();
    }

    public void AddFish()
    {
        fishCount++;
        fishText.text = " " + fishCount.ToString();
    }

    public void AddHoney()
    {
        honeyCount++;
        honeyText.text = " " + honeyCount.ToString();
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
        keyPressed = false;

        foreach (string s in text)
        {
            if (s.Contains("SwitchP") == true)
            {
                if (textBoxPortrait.sprite == image)
                {
                    textBoxPortrait.sprite = bearPortrait;
                }
                else if(textBoxPortrait.sprite == bearPortrait)
                {
                    textBoxPortrait.sprite = image;
                }

                continue;
            }

            foreach (char c in s)
            {
                emptyText += c;
                textBoxText.text = emptyText;
                Input.ResetInputAxes();

                yield return new WaitForSeconds(0.05f);

                
                if (Input.anyKey == true)
                {
                    emptyText = s;
                    textBoxText.text = emptyText;
                    keyPressed = false;
                    break;
                }

            }

            while (keyPressed == false)
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
