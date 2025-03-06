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
<<<<<<< HEAD
        berryText = transform.GetChild(0).GetComponent<TMP_Text>();
        fishText = transform.GetChild(1).GetComponent<TMP_Text>();
        honeyText = transform.GetChild(2).GetComponent<TMP_Text>();
        textBox = transform.GetChild(3).GetComponent<Image>();
        textBoxText = textBox.transform.GetChild(0).GetComponent<TMP_Text>();
        textBoxPortrait = textBox.transform.GetChild(1).GetComponent<Image>();

        pauseMenu = transform.GetChild(4).GetComponent<Image>();
        backButton = pauseMenu.transform.GetChild(1).GetComponent<Button>();
        mapButton = pauseMenu.transform.GetChild(2).GetComponent<Button>();
        exitButton = pauseMenu.transform.GetChild(3).GetComponent<Button>();

        backButton.onClick.AddListener(BackButton);
        mapButton.onClick.AddListener(MapButton);
        exitButton.onClick.AddListener(ExitButton);

        DontDestroyOnLoad(gameObject);
=======
       berryText = transform.GetChild(0).GetComponent<TMP_Text>();
       fishText = transform.GetChild(1).GetComponent<TMP_Text>();
       honeyText = transform.GetChild(2).GetComponent<TMP_Text>();
       DontDestroyOnLoad(gameObject);
>>>>>>> 674cb876febceae95efe1bb3f8e9248ff6df3e0a
    }
    void Start()
    {
        
    }
    void Update()
    {
<<<<<<< HEAD
        ReduceBFH();
        if (Input.anyKeyDown == true)
        {
            keyPressed = true;
        }
        else
        {
            keyPressed = false;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseMenu.transform.gameObject.activeSelf == false)
            {
                Time.timeScale = 0;
                pauseMenu.transform.gameObject.SetActive(true);
            }
            else if (pauseMenu.transform.gameObject.activeSelf == true)
            {
                Time.timeScale = 1;
                pauseMenu.transform.gameObject.SetActive(false);
            }
        }
    }



    void ReduceBFH()
    {


        if (FindAnyObjectByType<Bear>().GetComponent<Rigidbody2D>().linearVelocityX != 0 && pauseMenu.transform.gameObject.activeSelf == false)
        {
            berryText.color = Color.Lerp(berryText.color, new Color(berryText.color.r, berryText.color.g, berryText.color.b, 0.25f), changeSpeed);
            berryText.transform.GetChild(0).GetComponent<Image>().color = Color.Lerp(berryText.transform.GetChild(0).GetComponent<Image>().color, new Color(255, 255, 255, 0.25f), changeSpeed);

            fishText.color = Color.Lerp(fishText.color, new Color(fishText.color.r, fishText.color.g, fishText.color.b, 0.25f), changeSpeed);
            fishText.transform.GetChild(0).GetComponent<Image>().color = Color.Lerp(fishText.transform.GetChild(0).GetComponent<Image>().color, new Color(255, 255, 255, 0.25f), changeSpeed);

            honeyText.color = Color.Lerp(honeyText.color, new Color(honeyText.color.r, honeyText.color.g, honeyText.color.b, 0.25f), changeSpeed);
            honeyText.transform.GetChild(0).GetComponent<Image>().color = Color.Lerp(honeyText.transform.GetChild(0).GetComponent<Image>().color, new Color(255, 255, 255, 0.25f), changeSpeed);
        }
        else
        {

            berryText.color = Color.Lerp(berryText.color, new Color(berryText.color.r, berryText.color.g, berryText.color.b, 1), changeSpeed);
            berryText.transform.GetChild(0).GetComponent<Image>().color = Color.Lerp(berryText.transform.GetChild(0).GetComponent<Image>().color, new Color(255, 255, 255, 1), changeSpeed);

            fishText.color = Color.Lerp(fishText.color, new Color(fishText.color.r, fishText.color.g, fishText.color.b, 1), changeSpeed);
            fishText.transform.GetChild(0).GetComponent<Image>().color = Color.Lerp(fishText.transform.GetChild(0).GetComponent<Image>().color, new Color(255, 255, 255, 1), changeSpeed);

            honeyText.color = Color.Lerp(honeyText.color, new Color(honeyText.color.r, honeyText.color.g, honeyText.color.b, 1), changeSpeed);
            honeyText.transform.GetChild(0).GetComponent<Image>().color = Color.Lerp(honeyText.transform.GetChild(0).GetComponent<Image>().color, new Color(255, 255, 255, 1), changeSpeed);
        }
    }
    void BackButton()
    {
        pauseMenu.transform.gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    void MapButton()
    {
        //put map stuff here
    }

    void ExitButton()
    {
        Application.Quit();
=======
        
>>>>>>> 674cb876febceae95efe1bb3f8e9248ff6df3e0a
    }

    public void AddBerry()
    {
        berryCount++;
        berryText.text = /*"B: " + */berryCount.ToString();
    }

    public void AddFish()
    {
        fishCount++;
        fishText.text = /*"F: " + */fishCount.ToString();
    }

    public void AddHoney()
    {
        honeyCount++;
        honeyText.text = /* "H: " + */honeyCount.ToString();
    }
}
