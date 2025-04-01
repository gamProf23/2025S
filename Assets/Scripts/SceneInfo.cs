using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Xml;
using UnityEngine.UI;

public class SceneInfo : MonoBehaviour
{
    public Bear myPlayerPrefab;
    public CamFollow myCameraPrefab;
    public Canvas myCanvasPrefab;
    public Sprite myBackGround;

    public static bool hasLoadedPrefab = false;

    public static Bear myPlayer;

    public CamFollow cameraClone;
    public Canvas canvasClone;
    //public Bear playerClone;

    Bear player;

    public static string previousScene;
    public static bool loadingNew;

    LeftExit1 L1;
    LeftExit2 L2;
    LeftExit3 L3;

    RightExit1 R1;
    RightExit2 R2;
    RightExit3 R3;

    UpExit1 U1;
    UpExit2 U2;
    UpExit3 U3;

    DownExit1 D1;
    DownExit2 D2;
    DownExit3 D3;

    Door1 door1;
    Door2 door2;
    Door3 door3;

    WildExit1 W1;
    WildExit2 W2;
    WildExit3 W3;

    public string L1SceneName;
    public string L2SceneName;
    public string L3SceneName;

    public string R1SceneName;
    public string R2SceneName;
    public string R3SceneName;

    public string U1SceneName;
    public string U2SceneName;
    public string U3SceneName;

    public string D1SceneName;
    public string D2SceneName;
    public string D3SceneName;

    public string door1SceneName;
    public string door2SceneName;
    public string door3SceneName;

    public string W1SceneName;
    public string W2SceneName;
    public string W3SceneName;

    static bool hasEnteredDoor;

    public static Dictionary<string, List<string>> collectablesDestroyed = collectablesDestroyed = new Dictionary<string, List<string>>();
    public static Dictionary<string, List<string>> conditionsMet = conditionsMet = new Dictionary<string, List<string>>();
    public void AddToCDList(string objName)
    {
        if (collectablesDestroyed.ContainsKey(gameObject.scene.name) == false)
        {
            collectablesDestroyed.Add(gameObject.scene.name, new List<string>());
            //Debug.Log("bruh");
        }

        if (collectablesDestroyed[gameObject.scene.name].Contains(objName) == false)
        {
            collectablesDestroyed[gameObject.scene.name].Add(objName);
        }
        
    }

    public Dictionary<string, List<string>> GetCDList()
    {
        return collectablesDestroyed;
    }

    public void AddToCMList(string objName)
    {
        if (conditionsMet.ContainsKey(gameObject.scene.name) == false)
        {
            conditionsMet.Add(gameObject.scene.name, new List<string>());
            //Debug.Log("bruh");
        }

        conditionsMet[gameObject.scene.name].Add(objName);
    }

    public Dictionary<string, List<string>> GetCMList()
    {
        return conditionsMet;
    }

    //leaving more room for more directions
    public enum ExitDirections
    {
        none,
        R1, R2, R3,
        L1, L2, L3,
        U1, U2, U3,
        D1, D2, D3,
    }

    public static ExitDirections exitDirection;

    private void Awake()
    {
        FindExits();

        
        
        //Debug.Log(previousScene);

        if (hasLoadedPrefab == false)
        {
            myPlayer = myPlayerPrefab;
            hasLoadedPrefab = true;
            player = Instantiate(myPlayer);
            cameraClone = Instantiate(myCameraPrefab);
            canvasClone = Instantiate(myCanvasPrefab);

        }
        else
        {
            player = FindAnyObjectByType<Bear>();
        }

        if (myBackGround != null)
        {
            transform.GetChild(0).GetComponent<Image>().sprite = myBackGround;
            transform.GetChild(0).GetComponent<Image>().color = new Color(255, 255, 255, 255);
            transform.GetChild(0).GetComponent<Canvas>().worldCamera = FindAnyObjectByType<CamFollow>().GetComponent<Camera>();
        }
    }

    public void ToTitleScreen()
    {
        hasLoadedPrefab = false;
    }

    void Start()
    {
        FindAnyObjectByType<CamFollow>().setMeUp = true;
        loadingNew = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (L1SceneName != "")
        {
            SwitchSceneLeftOrRight(L1SceneName, L1, ExitDirections.L1);
        }

        if (L2SceneName != "")
        {
            SwitchSceneLeftOrRight(L2SceneName, L2, ExitDirections.L2);
        }

        if (L3SceneName != "")
        {
            SwitchSceneLeftOrRight(L3SceneName, L3, ExitDirections.L3);
        }

        if (R1SceneName != "")
        {
            SwitchSceneLeftOrRight(R1SceneName, R1, ExitDirections.R1);
        }

        if (R2SceneName != "")
        {
            SwitchSceneLeftOrRight(R2SceneName, R2, ExitDirections.R2);
        }

        if (R3SceneName != "")
        {
            SwitchSceneLeftOrRight(R3SceneName, R3, ExitDirections.R3);
        }

        if (U1 != null && U1SceneName != "")
        {
            SwitchSceneUpOrDown(U1SceneName, U1.gameObject, ExitDirections.U1);
        }

        if (U2 != null && U2SceneName != "")
        {
            SwitchSceneUpOrDown(U2SceneName, U2.gameObject, ExitDirections.U2);
        }

        if (U3 != null && U3SceneName != "")
        {
            SwitchSceneUpOrDown(U3SceneName, U3.gameObject, ExitDirections.U3);
        }

        if (D1 != null && D1SceneName != "")
        {
            SwitchSceneUpOrDown(D1SceneName, D1.gameObject, ExitDirections.D1);
        }

        if (D2 != null && D2SceneName != "")
        {
            SwitchSceneUpOrDown(D2SceneName, D2.gameObject, ExitDirections.D2);
        }

        if (D3 != null && D3SceneName != "")
        {
            SwitchSceneUpOrDown(D3SceneName, D3.gameObject, ExitDirections.D3);
        }

        if (door1 != null && door1SceneName != "")
        {
            SwitchSceneDoor(door1SceneName, door1);
        }

        if (door2 != null && door2SceneName != "")
        {
            SwitchSceneDoor(door2SceneName, door2);
        }

        if (door3 != null && door3SceneName != "")
        {
            SwitchSceneDoor(door3SceneName, door3);
        }

        if (W1 != null && W1SceneName != "")
        {
            SwitchSceneWild(W1SceneName, W1);
        }

        if (W2 != null && W2SceneName != "")
        {
            SwitchSceneWild(W2SceneName, W2);
        }

        if (W3 != null && W3SceneName != "")
        {
            SwitchSceneWild(W3SceneName, W3);
        }

        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow))
        {
            hasEnteredDoor = false;
        }
    }

    private void OnDestroy()
    {
        previousScene = gameObject.scene.name;
    }

    void SwitchSceneLeftOrRight(string sceneName, Exit exit, ExitDirections ed)
    {
        if (exit != null && loadingNew == false)
        {

            
            if ((player.transform.position.x >= exit.transform.position.x) && (exit.name.Contains("R") == true) || (player.transform.position.x <= exit.transform.position.x) && (exit.name.Contains("L")))
            {
                if (exit.exitWhenAbove == true && exit.exitWhenBelow == true)
                {
                    player.transform.position = Vector2.zero;
                    myPlayer = player;
                    loadingNew = true;
                    exitDirection = ed;
                    SceneHelper.LoadScene(sceneName);
                }
                else if (exit.exitWhenAbove == true && exit.exitWhenBelow == false)
                {
                    if (player.transform.position.y >= exit.transform.position.y || (player.transform.position.y < exit.transform.position.y + (exit.GetComponent<SpriteRenderer>().bounds.size.y / 2) && player.transform.position.y > exit.transform.position.y - (exit.GetComponent<SpriteRenderer>().bounds.size.y / 2)))
                    {
                        player.transform.position = Vector2.zero;
                        myPlayer = player;
                        loadingNew = true;
                        exitDirection = ed;
                        SceneHelper.LoadScene(sceneName);
                    }
                }
                else if (exit.exitWhenAbove == false && exit.exitWhenBelow == true)
                {
                    if (player.transform.position.y <= exit.transform.position.y || (player.transform.position.y < exit.transform.position.y + (exit.GetComponent<SpriteRenderer>().bounds.size.y / 2) && player.transform.position.y > exit.transform.position.y - (exit.GetComponent<SpriteRenderer>().bounds.size.y / 2)))
                    {
                        player.transform.position = Vector2.zero;
                        myPlayer = player;
                        loadingNew = true;
                        exitDirection = ed;
                        SceneHelper.LoadScene(sceneName);
                    }
                }
                else if (player.transform.position.y < exit.transform.position.y + (exit.GetComponent<SpriteRenderer>().bounds.size.y / 2) && player.transform.position.y > exit.transform.position.y - (exit.GetComponent<SpriteRenderer>().bounds.size.y / 2))
                {
                    player.transform.position = Vector2.zero;
                    myPlayer = player;
                    loadingNew = true;
                    exitDirection = ed;
                    SceneHelper.LoadScene(sceneName);
                }

                if (exit.resetVelocityOnEnter == true)
                {
                    player.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
                }
            }
        }
        
    }

    void SwitchSceneUpOrDown(string sceneName, GameObject exit, ExitDirections ed)
    {
        if (exit != null && loadingNew == false)
        {
            if ((player.transform.position.y >= exit.transform.position.y) && (exit.name.Contains("U") == true) || (player.transform.position.y <= exit.transform.position.y) && (exit.name.Contains("D")))
            {
                if (player.transform.position.x < exit.transform.position.x + (exit.GetComponent<SpriteRenderer>().bounds.size.x / 2) && player.transform.position.x > exit.transform.position.x - (exit.GetComponent<SpriteRenderer>().bounds.size.x / 2))
                {
                    player.transform.position = Vector2.zero;
                    myPlayer = player;
                    loadingNew = true;
                    exitDirection = ed;
                    SceneHelper.LoadScene(sceneName);

                    if (exit.name.Contains("U") == true)
                    {
                        player.GetComponent<Rigidbody2D>().AddForceY(exit.GetComponent<UpExit>().upForceOnEnter);
                    }
                }
            }
        }

    }

    static string doorName;

    void SwitchSceneDoor(string sceneName, Door door)
    {
        if (door != null && loadingNew == false)
        {
            if ((player.transform.position.y < door.transform.position.y + (door.GetComponent<SpriteRenderer>().bounds.size.y / 2) && player.transform.position.y > door.transform.position.y - (door.GetComponent<SpriteRenderer>().bounds.size.y / 2)) && (player.transform.position.x < door.transform.position.x + (door.GetComponent<SpriteRenderer>().bounds.size.x / 2) && player.transform.position.x > door.transform.position.x - (door.GetComponent<SpriteRenderer>().bounds.size.x / 2)))
            {
                if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) && player.isGrounded == true && hasEnteredDoor == false)
                {
                    player.transform.position = Vector2.zero;
                    myPlayer = player;
                    loadingNew = true;
                    doorName = door.name;
                    SceneHelper.LoadScene(sceneName);
                    hasEnteredDoor = true;
                    player.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
                }
            }
        }
    }

    void SwitchSceneWild(string sceneName, WildExit exit)
    {
        if (exit != null && loadingNew == false)
        {
            if (exit.wildDirection == WildExit.WildDirection.left)
            {
                if (player.transform.position.x <= exit.transform.position.x)
                {
                    if (exit.exitWhenAbove == true && exit.exitWhenBelow == true)
                    {
                        player.transform.position = Vector2.zero;
                        myPlayer = player;
                        loadingNew = true;
                        doorName = exit.name;
                        SceneHelper.LoadScene(sceneName);
                    }
                    else if (exit.exitWhenAbove == true && exit.exitWhenBelow == false)
                    {
                        if (player.transform.position.y >= exit.transform.position.y || (player.transform.position.y < exit.transform.position.y + (exit.GetComponent<SpriteRenderer>().bounds.size.y / 2) && player.transform.position.y > exit.transform.position.y - (exit.GetComponent<SpriteRenderer>().bounds.size.y / 2)))
                        {
                            player.transform.position = Vector2.zero;
                            myPlayer = player;
                            loadingNew = true;
                            doorName = exit.name;
                            SceneHelper.LoadScene(sceneName);
                        }
                    }
                    else if (exit.exitWhenAbove == false && exit.exitWhenBelow == true)
                    {
                        if (player.transform.position.y <= exit.transform.position.y || (player.transform.position.y < exit.transform.position.y + (exit.GetComponent<SpriteRenderer>().bounds.size.y / 2) && player.transform.position.y > exit.transform.position.y - (exit.GetComponent<SpriteRenderer>().bounds.size.y / 2)))
                        {
                            player.transform.position = Vector2.zero;
                            myPlayer = player;
                            loadingNew = true;
                            doorName = exit.name;
                            SceneHelper.LoadScene(sceneName);
                        }
                    }
                    else if (player.transform.position.y < exit.transform.position.y + (exit.GetComponent<SpriteRenderer>().bounds.size.y / 2) && player.transform.position.y > exit.transform.position.y - (exit.GetComponent<SpriteRenderer>().bounds.size.y / 2))
                    {
                        player.transform.position = Vector2.zero;
                        myPlayer = player;
                        loadingNew = true;
                        doorName = exit.name;
                        SceneHelper.LoadScene(sceneName);
                    }

                    if (exit.resetVelocityOnEnter == true)
                    {
                        player.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
                    }
                }
            }
            else if (exit.wildDirection == WildExit.WildDirection.right)
            {
                if (player.transform.position.x >= exit.transform.position.x)
                {
                    if (exit.exitWhenAbove == true && exit.exitWhenBelow == true)
                    {
                        player.transform.position = Vector2.zero;
                        myPlayer = player;
                        loadingNew = true;
                        doorName = exit.name;
                        SceneHelper.LoadScene(sceneName);
                    }
                    else if (exit.exitWhenAbove == true && exit.exitWhenBelow == false)
                    {
                        if (player.transform.position.y >= exit.transform.position.y || (player.transform.position.y < exit.transform.position.y + (exit.GetComponent<SpriteRenderer>().bounds.size.y / 2) && player.transform.position.y > exit.transform.position.y - (exit.GetComponent<SpriteRenderer>().bounds.size.y / 2)))
                        {
                            player.transform.position = Vector2.zero;
                            myPlayer = player;
                            loadingNew = true;
                            doorName = exit.name;
                            SceneHelper.LoadScene(sceneName);
                        }
                    }
                    else if (exit.exitWhenAbove == false && exit.exitWhenBelow == true)
                    {
                        if (player.transform.position.y <= exit.transform.position.y || (player.transform.position.y < exit.transform.position.y + (exit.GetComponent<SpriteRenderer>().bounds.size.y / 2) && player.transform.position.y > exit.transform.position.y - (exit.GetComponent<SpriteRenderer>().bounds.size.y / 2)))
                        {
                            player.transform.position = Vector2.zero;
                            myPlayer = player;
                            loadingNew = true;
                            doorName = exit.name;
                            SceneHelper.LoadScene(sceneName);
                        }
                    }
                    else if (player.transform.position.y < exit.transform.position.y + (exit.GetComponent<SpriteRenderer>().bounds.size.y / 2) && player.transform.position.y > exit.transform.position.y - (exit.GetComponent<SpriteRenderer>().bounds.size.y / 2))
                    {
                        player.transform.position = Vector2.zero;
                        myPlayer = player;
                        loadingNew = true;
                        doorName = exit.name;
                        SceneHelper.LoadScene(sceneName);
                    }

                    if (exit.resetVelocityOnEnter == true)
                    {
                        player.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
                    }
                }

                
            }
        }
    }

    public void SetUpPlayer()
    {
        FindExits();

        if (doorName != null)
        {
            DoorSetUp();
        }
       
        if (previousScene == L1SceneName && exitDirection == ExitDirections.R1)
        {
            player.transform.position = new Vector2(L1.transform.position.x + L1.GetComponent<SpriteRenderer>().bounds.size.x / 2, L1.transform.position.y - ((L1.GetComponent<SpriteRenderer>().bounds.size.y / 2) /*+ (player.GetComponent<SpriteRenderer>().bounds.size.y / 2)*/));
        }

        if (previousScene == L2SceneName && exitDirection == ExitDirections.R2)
        {
            player.transform.position = new Vector2(L2.transform.position.x + L2.GetComponent<SpriteRenderer>().bounds.size.x / 2, L2.transform.position.y - ((L2.GetComponent<SpriteRenderer>().bounds.size.y / 2) /*+ (player.GetComponent<SpriteRenderer>().bounds.size.y / 2)*/));
        }

        if (previousScene == L3SceneName && exitDirection == ExitDirections.R3)
        {
            player.transform.position = new Vector2(L3.transform.position.x + L3.GetComponent<SpriteRenderer>().bounds.size.x / 2, L3.transform.position.y - ((L3.GetComponent<SpriteRenderer>().bounds.size.y / 2) /*+ (player.GetComponent<SpriteRenderer>().bounds.size.y / 2)*/));
        }

        if (previousScene == R1SceneName && exitDirection == ExitDirections.L1)
        {
            player.transform.position = new Vector2(R1.transform.position.x - R1.GetComponent<SpriteRenderer>().bounds.size.x / 2, R1.transform.position.y - ((R1.GetComponent<SpriteRenderer>().bounds.size.y / 2) /*+ (player.GetComponent<SpriteRenderer>().bounds.size.y / 2)*/));
        }

        if (previousScene == R2SceneName && exitDirection == ExitDirections.L2)
        {
            player.transform.position = new Vector2(R2.transform.position.x - R2.GetComponent<SpriteRenderer>().bounds.size.x / 2, R2.transform.position.y - ((R2.GetComponent<SpriteRenderer>().bounds.size.y / 2) /*+ (player.GetComponent<SpriteRenderer>().bounds.size.y / 2)*/));
        }

        if (previousScene == R3SceneName && exitDirection == ExitDirections.L3)
        {
            player.transform.position = new Vector2(R3.transform.position.x - R3.GetComponent<SpriteRenderer>().bounds.size.x / 2, R3.transform.position.y - ((R3.GetComponent<SpriteRenderer>().bounds.size.y / 2) /*+ (player.GetComponent<SpriteRenderer>().bounds.size.y / 2)*/));
        }

        if (previousScene == U1SceneName && exitDirection == ExitDirections.D1)
        {
            player.transform.position = new Vector2(U1.transform.position.x, U1.transform.position.y - ((U1.GetComponent<SpriteRenderer>().bounds.size.y / 2) /*+ (player.GetComponent<SpriteRenderer>().bounds.size.y / 2)*/));
        }

        if (previousScene == U2SceneName && exitDirection == ExitDirections.D2)
        {
            player.transform.position = new Vector2(U2.transform.position.x, U2.transform.position.y - ((U2.GetComponent<SpriteRenderer>().bounds.size.y / 2) /*+ (player.GetComponent<SpriteRenderer>().bounds.size.y / 2)*/));
        }

        if (previousScene == U3SceneName && exitDirection == ExitDirections.D3)
        {
            player.transform.position = new Vector2(U3.transform.position.x, U3.transform.position.y - ((U3.GetComponent<SpriteRenderer>().bounds.size.y / 2) /*+ (player.GetComponent<SpriteRenderer>().bounds.size.y / 2)*/));
        }

        if (previousScene == D1SceneName && exitDirection == ExitDirections.U1)
        {
            player.transform.position = new Vector2(D1.transform.position.x, D1.transform.position.y + ((D1.GetComponent<SpriteRenderer>().bounds.size.y / 2) /*+ (player.GetComponent<SpriteRenderer>().bounds.size.y / 2)*/));
        }

        if (previousScene == D2SceneName && exitDirection == ExitDirections.U2)
        {
            player.transform.position = new Vector2(D2.transform.position.x, D2.transform.position.y + ((D2.GetComponent<SpriteRenderer>().bounds.size.y / 2) /*+ (player.GetComponent<SpriteRenderer>().bounds.size.y / 2)*/));
        }

        if (previousScene == D3SceneName && exitDirection == ExitDirections.U3)
        {
            player.transform.position = new Vector2(D3.transform.position.x, D3.transform.position.y + ((D3.GetComponent<SpriteRenderer>().bounds.size.y / 2) /*+ (player.GetComponent<SpriteRenderer>().bounds.size.y / 2)*/));
        }
    }

    

    void DoorSetUp()
    {
        if (doorName.Contains("1"))
        {
            if (door1 != null)
            {
                player.transform.position = new Vector2(door1.transform.position.x, door1.transform.position.y - ((door1.GetComponent<SpriteRenderer>().bounds.size.y / 2)));
            }
            else if (W1 != null)
            {
                WildSetUp(W1);
            }
            
        }
        else if (doorName.Contains("2"))
        {
            if (door2 != null)
            {
                player.transform.position = new Vector2(door2.transform.position.x, door2.transform.position.y - ((door2.GetComponent<SpriteRenderer>().bounds.size.y / 2)));
            }
            else if (W2 != null)
            {
                WildSetUp(W2);
            }
            
        }
        else if (doorName.Contains("3"))
        {
            if (door3 != null)
            {
                player.transform.position = new Vector2(door3.transform.position.x, door3.transform.position.y - ((door3.GetComponent<SpriteRenderer>().bounds.size.y / 2)));
            }
            else if (W3 != null)
            {
                WildSetUp(W3);
            }
            
        }

        doorName = null;
    }

    void WildSetUp(WildExit exit)
    {
        if (exit.wildDirection == WildExit.WildDirection.left) 
        {
            player.transform.position = new Vector2(exit.transform.position.x + exit.GetComponent<SpriteRenderer>().bounds.size.x / 2, exit.transform.position.y - ((exit.GetComponent<SpriteRenderer>().bounds.size.y / 2)));
        }
        else if (exit.wildDirection == WildExit.WildDirection.right)
        {
            player.transform.position = new Vector2(exit.transform.position.x - exit.GetComponent<SpriteRenderer>().bounds.size.x / 2, exit.transform.position.y - ((exit.GetComponent<SpriteRenderer>().bounds.size.y / 2)));
        }
    }

    void FindExits()
    {

        if (FindAnyObjectByType<LeftExit1>() != null)
        {
            L1 = FindAnyObjectByType<LeftExit1>();
        }

        if (FindAnyObjectByType<LeftExit2>() != null)
        {
           L2 = FindAnyObjectByType<LeftExit2>();
        }

        if (FindAnyObjectByType<LeftExit3>() != null)
        {
           L3 = FindAnyObjectByType<LeftExit3>();
        }

        if (FindAnyObjectByType<RightExit1>() != null)
        {
            R1 = FindAnyObjectByType<RightExit1>();
        }

        if (FindAnyObjectByType<RightExit2>() != null)
        {
            R2 = FindAnyObjectByType<RightExit2>();
        }

        if (FindAnyObjectByType<RightExit3>() != null)
        {
            R3 = FindAnyObjectByType<RightExit3>();
        }

        if (FindAnyObjectByType<UpExit1>() != null)
        {
            U1 = FindAnyObjectByType<UpExit1>();
        }

        if (FindAnyObjectByType<UpExit2>() != null)
        {
            U2 = FindAnyObjectByType<UpExit2>();
        }

        if (FindAnyObjectByType<UpExit3>() != null)
        {
            U3 = FindAnyObjectByType<UpExit3>();
        }

        if (FindAnyObjectByType<DownExit1>() != null)
        {
            D1 = FindAnyObjectByType<DownExit1>();
        }

        if (FindAnyObjectByType<DownExit2>() != null)
        {
            D2 = FindAnyObjectByType<DownExit2>();
        }

        if (FindAnyObjectByType<DownExit3>() != null)
        {
            D3 = FindAnyObjectByType<DownExit3>();
        }

        if (FindAnyObjectByType<Door1>() != null)
        {
            door1 = FindAnyObjectByType<Door1>();
        }

        if (FindAnyObjectByType<Door2>() != null)
        {
            door2 = FindAnyObjectByType<Door2>();
        }

        if (FindAnyObjectByType<Door3>() != null)
        {
            door3 = FindAnyObjectByType<Door3>();
        }

        if (FindAnyObjectByType<WildExit1>() != null)
        {
            W1 = FindAnyObjectByType<WildExit1>();
        }

        if (FindAnyObjectByType<WildExit2>() != null)
        {
            W2 = FindAnyObjectByType<WildExit2>();
        }

        if (FindAnyObjectByType<WildExit3>() != null)
        {
            W3 = FindAnyObjectByType<WildExit3>();
        }

    }
    

    public static partial class SceneHelper
    {
        public static void LoadScene(string SceneNameToLoad)
        {
            PendingPreviousScene = SceneManager.GetActiveScene().name;
            SceneManager.sceneLoaded += ActivatorAndUnloader;
            SceneManager.LoadScene(SceneNameToLoad, LoadSceneMode.Additive);
        }

        static string PendingPreviousScene;
        static void ActivatorAndUnloader(Scene scene, LoadSceneMode mode)
        {
            SceneManager.sceneLoaded -= ActivatorAndUnloader;
            SceneManager.SetActiveScene(scene);
            SceneManager.UnloadSceneAsync(PendingPreviousScene);
        }
    }
}
