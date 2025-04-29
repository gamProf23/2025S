using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class CanvasThing : MonoBehaviour
{
    public int berryCount;
    public int fishCount;
    public int honeyCount;

    TMP_Text berryText;
    TMP_Text fishText;
    TMP_Text honeyText;

    Image textBox;
    TMP_Text textBoxText;
    Image textBoxPortrait;

    Image pauseMenu;

    Button backButton;
    Button mapButton;
    Button exitButton;
    Button optionsButton;

    Image tbInteraction;
    TMP_Text yText;
    TMP_Text nText;
    Image ySel;
    Image nSel;

    Image map;
    Button mapBackButton;

    public Image seasonTimer;
    public Image seasonTimerMarker;
    float markerStart = -96;
    float markerEnd = 96;
    float timeLimit = 900; //900
    float currentTime;

    Image optionsMenu;
    Image soundOptions;
    public Image controlOptions;

    public Slider soundSlider;

    public TMP_Dropdown jumpDrop;
    public TMP_Dropdown rollDrop;
    public TMP_Dropdown roarDrop;
    public TMP_Dropdown clawDrop;

    Button soundButton;
    Button controlButton;
    Button optionBackButton;

    int sel = 0;
    NPCThing npcThing;

    float changeSpeed = 0.05f;

    public Sprite bearPortrait;
    bool keyPressed = false;

    public int cmAmount = 0;

    public AudioClip animalTalk;
    public AudioClip bearTalk;

    bool inBonus = false;

    private void Awake()
    {
        berryText = transform.GetChild(0).GetComponent<TMP_Text>();
        fishText = transform.GetChild(1).GetComponent<TMP_Text>();
        honeyText = transform.GetChild(2).GetComponent<TMP_Text>();
        textBox = transform.GetChild(3).GetComponent<Image>();
        textBoxText = textBox.transform.GetChild(0).GetComponent<TMP_Text>();
        textBoxPortrait = textBox.transform.GetChild(1).GetComponent<Image>();

        pauseMenu = transform.GetChild(5).GetComponent<Image>();
        backButton = pauseMenu.transform.GetChild(1).GetComponent<Button>();
        mapButton = pauseMenu.transform.GetChild(2).GetComponent<Button>();
        exitButton = pauseMenu.transform.GetChild(3).GetComponent<Button>();
        optionsButton = pauseMenu.transform.GetChild(4).GetComponent<Button>();

        backButton.onClick.AddListener(BackButton);
        mapButton.onClick.AddListener(MapButton);
        exitButton.onClick.AddListener(ExitButton);
        optionsButton.onClick.AddListener(OptionsButton);

        tbInteraction = transform.GetChild(4).GetComponent<Image>();
        yText = tbInteraction.transform.GetChild(0).GetComponent<TMP_Text>();
        nText = tbInteraction.transform.GetChild(1).GetComponent<TMP_Text>();
        ySel = tbInteraction.transform.GetChild(2).GetComponent<Image>();
        nSel = tbInteraction.transform.GetChild(3).GetComponent<Image>();

        map = transform.GetChild(6).GetComponent<Image>();
        mapBackButton = map.transform.GetChild(0).GetComponent<Button>();
        mapBackButton.onClick.AddListener(MapBackButton);

        seasonTimer = transform.GetChild(7).GetComponent<Image>();
        seasonTimerMarker = seasonTimer.transform.GetChild(0).GetComponent<Image>();

        optionsMenu = transform.GetChild(8).GetComponent<Image>();

        soundOptions = optionsMenu.transform.GetChild(0).GetComponent<Image>();
        soundSlider = soundOptions.transform.GetChild(0).transform.GetChild(0).GetComponent<Slider>();
        soundSlider.onValueChanged.AddListener(delegate { SoundSliderChanged(); });

        controlOptions = optionsMenu.transform.GetChild(1).GetComponent<Image>();
        jumpDrop = controlOptions.transform.GetChild(0).GetComponent<TMP_Dropdown>();
        rollDrop = controlOptions.transform.GetChild(1).GetComponent<TMP_Dropdown>();
        roarDrop = controlOptions.transform.GetChild(2).GetComponent<TMP_Dropdown>();
        clawDrop = controlOptions.transform.GetChild(3).GetComponent<TMP_Dropdown>();

        soundButton = optionsMenu.transform.GetChild(2).GetComponent<Button>();
        controlButton = optionsMenu.transform.GetChild(3).GetComponent<Button>();
        optionBackButton = optionsMenu.transform.GetChild(4).GetComponent<Button>();

        soundButton.onClick.AddListener(SoundButton);
        controlButton.onClick.AddListener(ControlButton);
        optionBackButton.onClick.AddListener(OptionBackButton);

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        
    }

    void Update()
    {
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

        if (sel != 0)
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) || (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)))
            {
                if (sel == 1)
                {
                    ySel.gameObject.SetActive(false);
                    nSel.gameObject.SetActive(true);
                    sel = 2;
                }
                else if (sel == 2)
                {
                    ySel.gameObject.SetActive(true);
                    nSel.gameObject.SetActive(false);
                    sel = 1;
                }
            }

            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.J))
            {
                tbInteraction.gameObject.SetActive(false);

                if (sel == 1)
                {
                    if (npcThing.berryCondition == 0 && npcThing.fishCondition == 0 && npcThing.honeyCondition == 0)
                    {
                        StartCoroutine(NPCTalkCR(npcThing.myInteractionYText, npcThing.myPortrait, null));
                    }
                    else if (berryCount >= npcThing.berryCondition && fishCount >= npcThing.fishCondition && honeyCount >= npcThing.honeyCondition)
                    {
                        berryCount = berryCount - npcThing.berryCondition;
                        fishCount = fishCount - npcThing.fishCondition;
                        honeyCount = honeyCount - npcThing.honeyCondition;

                        berryText.text = " " + berryCount.ToString();
                        fishText.text = " " + fishCount.ToString();
                        honeyText.text = " " + honeyCount.ToString();

                        npcThing.conditionIsMet = true;

                        FindAnyObjectByType<SceneInfo>().AddToCMList(npcThing.name);

                        StartCoroutine(NPCTalkCR(npcThing.myInteractionYText, npcThing.myPortrait, null));
                    }
                    else
                    {
                        StartCoroutine(NPCTalkCR(npcThing.myInteractionFailText, npcThing.myPortrait, null));
                    }
                    
                }
                else if (sel == 2)
                {
                    StartCoroutine(NPCTalkCR(npcThing.myInteractionNText, npcThing.myPortrait, null));
                }

                sel = 0;
                npcThing = null;
            }
        }

        float distanceBetween = Math.Abs(markerStart) + Math.Abs(markerEnd);

        if (FindAnyObjectByType<Bear>().isTalking == true)
        {
            seasonTimer.enabled = false;
            seasonTimerMarker.enabled = false;
        }
        else if (seasonTimer.enabled == false)
        {
            seasonTimer.enabled = true;
            seasonTimerMarker.enabled = true;
        }

        if (pauseMenu.transform.gameObject.activeSelf == false && FindAnyObjectByType<Bear>().isTalking == false && inBonus == false)
        {
            currentTime = currentTime + Time.deltaTime;

            seasonTimerMarker.transform.localPosition = new Vector3(markerStart + ((currentTime/timeLimit) * distanceBetween), 0, 0);

            if(Math.Abs(seasonTimerMarker.transform.localPosition.x) > markerEnd)
            {
                cmAmount = FindAnyObjectByType<SceneInfo>().GetCMAmount();
                FindAnyObjectByType<SceneInfo>().ToTitleScreen();
                SceneManager.LoadScene("PlayTestEnd");
            }
        }
        

        if(Input.GetKeyDown(KeyCode.M))
        {
            if (pauseMenu.transform.gameObject.activeSelf == false && map.gameObject.activeSelf == false)
            {
                Time.timeScale = 0;
                pauseMenu.transform.gameObject.SetActive(true);
                MapButton();
            }
            else if (map.gameObject.activeSelf == true)
            {
                MapBackButton();
                Time.timeScale = 1;
                pauseMenu.transform.gameObject.SetActive(false);
            }
        }
 
        if (FindAnyObjectByType<SceneInfo>().gameObject.scene.name.Contains("bonus") == true)
        {
            seasonTimer.gameObject.SetActive(false);
            inBonus = true;
        }
        else
        {
            seasonTimer.gameObject.SetActive(true);
            inBonus = false;

        }
    }



    void ReduceBFH()
    {


        if ((FindAnyObjectByType<Bear>().GetComponent<Rigidbody2D>().linearVelocityX != 0  || FindAnyObjectByType<Bear>().isClimbing == true) && pauseMenu.transform.gameObject.activeSelf == false)
        {
            berryText.color = Color.Lerp(berryText.color, new Color(berryText.color.r, berryText.color.g, berryText.color.b, 0.25f), changeSpeed);
            berryText.transform.GetChild(0).GetComponent<Image>().color = Color.Lerp(berryText.transform.GetChild(0).GetComponent<Image>().color, new Color(255, 255, 255, 0.25f), changeSpeed);

            fishText.color = Color.Lerp(fishText.color, new Color(fishText.color.r, fishText.color.g, fishText.color.b, 0.25f), changeSpeed);
            fishText.transform.GetChild(0).GetComponent<Image>().color = Color.Lerp(fishText.transform.GetChild(0).GetComponent<Image>().color, new Color(255, 255, 255, 0.25f), changeSpeed);

            honeyText.color = Color.Lerp(honeyText.color, new Color(honeyText.color.r, honeyText.color.g, honeyText.color.b, 0.25f), changeSpeed);
            honeyText.transform.GetChild(0).GetComponent<Image>().color = Color.Lerp(honeyText.transform.GetChild(0).GetComponent<Image>().color, new Color(255, 255, 255, 0.25f), changeSpeed);

            seasonTimer.color = Color.Lerp(seasonTimer.color, new Color(seasonTimer.color.r, seasonTimer.color.g, seasonTimer.color.b, 0.25f), changeSpeed);
            seasonTimerMarker.color = Color.Lerp(seasonTimerMarker.color, new Color(seasonTimerMarker.color.r, seasonTimerMarker.color.g, seasonTimerMarker.color.b, 0.25f), changeSpeed);
        }
        else
        {

            berryText.color = Color.Lerp(berryText.color, new Color(berryText.color.r, berryText.color.g, berryText.color.b, 1), changeSpeed);
            berryText.transform.GetChild(0).GetComponent<Image>().color = Color.Lerp(berryText.transform.GetChild(0).GetComponent<Image>().color, new Color(255, 255, 255, 1), changeSpeed);

            fishText.color = Color.Lerp(fishText.color, new Color(fishText.color.r, fishText.color.g, fishText.color.b, 1), changeSpeed);
            fishText.transform.GetChild(0).GetComponent<Image>().color = Color.Lerp(fishText.transform.GetChild(0).GetComponent<Image>().color, new Color(255, 255, 255, 1), changeSpeed);

            honeyText.color = Color.Lerp(honeyText.color, new Color(honeyText.color.r, honeyText.color.g, honeyText.color.b, 1), changeSpeed);
            honeyText.transform.GetChild(0).GetComponent<Image>().color = Color.Lerp(honeyText.transform.GetChild(0).GetComponent<Image>().color, new Color(255, 255, 255, 1), changeSpeed);

            seasonTimer.color = Color.Lerp(seasonTimer.color, new Color(seasonTimer.color.r, seasonTimer.color.g, seasonTimer.color.b, 1f), changeSpeed);
            seasonTimerMarker.color = Color.Lerp(seasonTimerMarker.color, new Color(seasonTimerMarker.color.r, seasonTimerMarker.color.g, seasonTimerMarker.color.b, 1f), changeSpeed);
        }
    }
    void BackButton()
    {
        pauseMenu.transform.gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    void MapButton()
    {
        map.transform.gameObject.SetActive(true);
        seasonTimer.gameObject.SetActive(false);

    }

    void ExitButton()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("TitleScreen");

        FindAnyObjectByType<SceneInfo>().ToTitleScreen();
        Destroy(FindAnyObjectByType<Bear>().gameObject);
        Destroy(FindAnyObjectByType<CamFollow>().gameObject);
        Destroy(gameObject);
    }

    void OptionsButton()
    {
        optionsMenu.gameObject.SetActive(true);
        controlOptions.gameObject.SetActive(true);
        soundOptions.gameObject.SetActive(false);
    }

    void SoundButton()
    {
        soundOptions.gameObject.SetActive(true);
        controlOptions.gameObject.SetActive(false);
    }

    void SoundSliderChanged()
    {
        Bear bear = FindAnyObjectByType<Bear>();

        bear.track1.volume = 0.25f * soundSlider.value;
        bear.track2.volume = 0.25f * soundSlider.value;
        bear.track3.volume = 0.25f * soundSlider.value;


        CamFollow cam = FindAnyObjectByType<CamFollow>();
        cam.GetComponent<AudioSource>().volume = 0.1f * soundSlider.value;


    }

    void ControlButton()
    {
        soundOptions.gameObject.SetActive(false);
        controlOptions.gameObject.SetActive(true);
    }

    void OptionBackButton()
    {
        optionsMenu.gameObject.SetActive(false);
    }

    void MapBackButton()
    {
        map.transform.gameObject.SetActive(false);
        seasonTimer.gameObject.SetActive(true);
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

    public void NPCTalk(List<string> text, Sprite image, NPCThing interaction)
    {
        StartCoroutine(NPCTalkCR(text, image, interaction));
    }

    public bool crRunning = false;
    
    public IEnumerator NPCTalkCR(List<string> text, Sprite image, NPCThing interaction)
    {
        bool interactionBool = false;
        crRunning = true;
        FindAnyObjectByType<Bear>().isTalking = true;
        string emptyText = "";
        textBoxText.text = "";
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

                if (GetComponent<AudioSource>().clip == animalTalk)
                {
                    GetComponent<AudioSource>().clip = bearTalk;
                }
                else
                {
                    GetComponent<AudioSource>().clip = animalTalk;
                }

                continue;
            }

            if (s.Contains("Interaction") == true)
            {
                interactionBool = true;
                break;
            }

            foreach (char c in s)
            {
                emptyText += c;
                textBoxText.text = emptyText;
                Input.ResetInputAxes();

                if (s.IndexOf(c) % 2 == 0)
                {
                    GetComponent<AudioSource>().Stop();
                    GetComponent<AudioSource>().volume = 0.25f * soundSlider.value;
                    GetComponent<AudioSource>().Play();
                }

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
        
        if (interactionBool == false)
        {
            textBox.transform.gameObject.SetActive(false);
            FindAnyObjectByType<Bear>().isTalking = false;
            crRunning = false;
        }
        else
        {
            StartCoroutine(NPCInteractionCR(interaction));
        }
        

    }

    IEnumerator NPCInteractionCR(NPCThing intr)
    {
        string emptyText = "";
        textBoxText.text = "";
        keyPressed = false;

        foreach (char c in intr.myInteractionText)
        {
            emptyText += c;
            textBoxText.text = emptyText;
            Input.ResetInputAxes();

            yield return new WaitForSeconds(0.05f);


            if (Input.anyKey == true)
            {
                emptyText = intr.myInteractionText;
                textBoxText.text = emptyText;
                keyPressed = false;
                break;
            }

        }

        tbInteraction.gameObject.SetActive(true);

        sel = 1;
        npcThing = intr;
        yText.text = intr.myInteractionYSel;
        nText.text = intr.myInteractionNSel;
        ySel.gameObject.SetActive(true);
        nSel.gameObject.SetActive(false);
        
    }

}
