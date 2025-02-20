using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using Unity.VisualScripting;

public class SceneInfo : MonoBehaviour
{
    public Bear myPlayerPrefab;
    public CamFollow myCameraPrefab;

    public static bool hasLoadedPrefab = false;

    public static Bear myPlayer;

    public CamFollow cameraClone;
    //public Bear playerClone;

    public string leftExitScene;
    public string rightExitScene;

    public string leftAltExitScene1;
    public string rightAltExitScene1;

    public string leftAltExitScene2;
    public string rightAltExitScene2;

    Bear player;
    
    LeftExit leftExit;
    RightExit rightExit;

    LeftAltExit1 leftAltExit1;
    RightAltExit1 rightAltExit1;

    LeftAltExit2 leftAltExit2;
    RightAltExit2 rightAltExit2;

    public static string previousScene;
    public static bool loadingNew;
    

    //leaving more room for more directions
    public enum ExitDirections
    {
        none,
        left,
        right,
        leftAlt1,
        rightAlt1,
        leftAlt2,
        rightAlt2,
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
            
        }
        else
        {
            player = FindAnyObjectByType<Bear>();
        }

        
    }

    void Start()
    {
        
        loadingNew = false;
        FindAnyObjectByType<CamFollow>().setMeUp = true;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (leftExit != null && leftExitScene != "" && loadingNew == false)
        {
            if (player.transform.position.x <= leftExit.transform.position.x)
            {
                player.transform.position = Vector2.zero;
                myPlayer = player;
                loadingNew = true;
                exitDirection = ExitDirections.left;
                SceneHelper.LoadScene(leftExitScene);
            }
        }

        if (rightExit != null && rightExitScene != "" && loadingNew == false)
        {
            if (player.transform.position.x >= rightExit.transform.position.x)
            {
                player.transform.position = Vector2.zero;
                myPlayer = player;
                loadingNew = true;
                exitDirection = ExitDirections.right;
                SceneHelper.LoadScene(rightExitScene);
            }
        }

        if (leftAltExit1 != null && leftAltExitScene1 != "" && loadingNew == false)
        {
            if (player.transform.position.x <= leftAltExit1.transform.position.x && (player.transform.position.y < leftAltExit1.transform.position.y + (leftAltExit1.GetComponent<SpriteRenderer>().bounds.size.y / 2) && player.transform.position.y > leftAltExit1.transform.position.y - (leftAltExit1.GetComponent<SpriteRenderer>().bounds.size.y / 2)))
            {
                player.transform.position = Vector2.zero;
                myPlayer = player;
                loadingNew = true;
                exitDirection = ExitDirections.leftAlt1;
                SceneHelper.LoadScene(leftAltExitScene1);
            }
        }

        if (rightAltExit1 != null && rightAltExitScene1 != "" && loadingNew == false)
        {
            if (player.transform.position.x >= rightAltExit1.transform.position.x && (player.transform.position.y < rightAltExit1.transform.position.y + (rightAltExit1.GetComponent<SpriteRenderer>().bounds.size.y / 2) && player.transform.position.y > rightAltExit1.transform.position.y - (rightAltExit1.GetComponent<SpriteRenderer>().bounds.size.y / 2)))
            {
                player.transform.position = Vector2.zero;
                myPlayer = player;
                loadingNew = true;
                exitDirection = ExitDirections.rightAlt1;
                SceneHelper.LoadScene(rightAltExitScene1);
            }
        }

        if (leftAltExit2 != null && leftAltExitScene2 != "" && loadingNew == false)
        {
            if (player.transform.position.x <= leftAltExit2.transform.position.x && (player.transform.position.y < leftAltExit2.transform.position.y + (leftAltExit2.GetComponent<SpriteRenderer>().bounds.size.y / 2) && player.transform.position.y > leftAltExit2.transform.position.y - (leftAltExit2.GetComponent<SpriteRenderer>().bounds.size.y / 2)))
            {
                player.transform.position = Vector2.zero;
                myPlayer = player;
                loadingNew = true;
                exitDirection = ExitDirections.leftAlt2;
                SceneHelper.LoadScene(leftAltExitScene2);
            }
        }

        if (rightAltExit2 != null && rightAltExitScene2 != "" && loadingNew == false)
        {
            if (player.transform.position.x >= rightAltExit2.transform.position.x && (player.transform.position.y < rightAltExit2.transform.position.y + (rightAltExit2.GetComponent<SpriteRenderer>().bounds.size.y / 2) && player.transform.position.y > rightAltExit2.transform.position.y - (rightAltExit2.GetComponent<SpriteRenderer>().bounds.size.y / 2)))
            {
                player.transform.position = Vector2.zero;
                myPlayer = player;
                loadingNew = true;
                exitDirection = ExitDirections.rightAlt2;
                SceneHelper.LoadScene(rightAltExitScene2);
            }
        }
    }

    private void OnDestroy()
    {
        previousScene = gameObject.scene.name;
    }

    public void SetUpPlayer()
    {
        FindExits();
        //transform.position = Vector3.zero;
        if (previousScene == leftExitScene && exitDirection == ExitDirections.right)
        {
            player.transform.position = new Vector2(leftExit.transform.position.x + 1f, leftExit.transform.position.y);
        }

        if (previousScene == rightExitScene && exitDirection == ExitDirections.left)
        {
            player.transform.position = new Vector2(rightExit.transform.position.x - 1f, rightExit.transform.position.y);
        }

        if (previousScene == leftAltExitScene1 && exitDirection == ExitDirections.rightAlt1)
        {
            player.transform.position = new Vector2(leftAltExit1.transform.position.x + leftAltExit1.GetComponent<SpriteRenderer>().bounds.size.x / 2, leftAltExit1.transform.position.y - ((leftAltExit1.GetComponent<SpriteRenderer>().bounds.size.y / 2) /*+ (player.GetComponent<SpriteRenderer>().bounds.size.y / 2)*/));
        }

        if (previousScene == rightAltExitScene1 && exitDirection == ExitDirections.leftAlt1)
        {
            player.transform.position = new Vector2(rightAltExit1.transform.position.x - rightAltExit1.GetComponent<SpriteRenderer>().bounds.size.x / 2 , rightAltExit1.transform.position.y - ((rightAltExit1.GetComponent<SpriteRenderer>().bounds.size.y / 2) /*+ (player.GetComponent<SpriteRenderer>().bounds.size.y / 2)*/));
        } 

        if (previousScene == leftAltExitScene2 && exitDirection == ExitDirections.rightAlt2)
        {
            player.transform.position = new Vector2(leftAltExit2.transform.position.x + leftAltExit2.GetComponent<SpriteRenderer>().bounds.size.x / 2, leftAltExit2.transform.position.y - ((leftAltExit2.GetComponent<SpriteRenderer>().bounds.size.y / 2) /*+ (player.GetComponent<SpriteRenderer>().bounds.size.y / 2)*/));
        }

        if (previousScene == rightAltExitScene2 && exitDirection == ExitDirections.leftAlt2)
        {
            player.transform.position = new Vector2(rightAltExit2.transform.position.x - rightAltExit2.GetComponent<SpriteRenderer>().bounds.size.x / 2, rightAltExit2.transform.position.y - ((rightAltExit2.GetComponent<SpriteRenderer>().bounds.size.y / 2) /*+ (player.GetComponent<SpriteRenderer>().bounds.size.y / 2)*/));
        }
    }

    void FindExits()
    {
        if (FindAnyObjectByType<LeftExit>() != null)
        {
            leftExit = FindAnyObjectByType<LeftExit>();
        }

        if (FindAnyObjectByType<RightExit>() != null)
        {
            rightExit = FindAnyObjectByType<RightExit>();
        }

        if (FindAnyObjectByType<LeftAltExit1>() != null)
        {
            leftAltExit1 = FindAnyObjectByType<LeftAltExit1>();
        }

        if (FindAnyObjectByType<RightAltExit1>() != null)
        {
            rightAltExit1 = FindAnyObjectByType<RightAltExit1>();
        }

        if (FindAnyObjectByType<LeftAltExit2>() != null)
        {
            leftAltExit2 = FindAnyObjectByType<LeftAltExit2>();
        }

        if (FindAnyObjectByType<RightAltExit2>() != null)
        {
            rightAltExit2 = FindAnyObjectByType<RightAltExit2>();
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
