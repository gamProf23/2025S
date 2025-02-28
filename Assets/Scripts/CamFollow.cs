using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CamFollow : MonoBehaviour
{

    static public GameObject POI; // The static projectile of interest for the camera.

    [Header("Set in Inspector")]
    public float easing = 0.05f;
    public Vector2 minXY = new Vector2 (0, -5);

    public Vector2 camLimitL;
    public Vector2 camLimitR;
    public Vector2 camLimitUp;
    public Vector2 camLimitDown;
    

    [HeaderAttribute("Set Dynamically")]

    public float camZ; // The variable that wll hold the camera's position in the Z axis.

    public float offsetX = 17.75f;

    public float offsetY = 10;

    float textBoxOffset = 5f;

    public bool setMeUp = false;

    float adjustedTextBoxY;
    bool adjustedForTextBox = false;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        camZ = this.transform.position.z;  // Assigns the camera's Z position to the camZ variable.
    }

    private void Start()
    {
        SetUpCam();
    }

    // Update is called once per frame
    void FixedUpdate() // Changes the update to wait until all the physics calculations are complete.
    {
        if (setMeUp == true)
        {
            //Debug.Log("Bruh");
            FindAnyObjectByType<SceneInfo>().SetUpPlayer();
            SetUpCam();
            setMeUp = false;
        }

        


        if (POI == null) return;

        // Assigns the POI position to the destination variable.
        Vector3 destination = POI.transform.position;


        if (destination.x < camLimitL.x + offsetX)
        {
            destination.x = camLimitL.x + offsetX;
        }

        if (destination.x > camLimitR.x - offsetX)
        {
            destination.x = camLimitR.x - offsetX;
        }

        if (FindAnyObjectByType<CamLimitUp>() != null && destination.y > camLimitUp.y - offsetY)
        {
            destination.y = camLimitUp.y - offsetY;
        }

        if (destination.y < camLimitDown.y + offsetY)
        {
            destination.y = camLimitDown.y + offsetY;
        }

        if (FindAnyObjectByType<Bear>().isTalking == true && adjustedForTextBox == false)
        {
            adjustedTextBoxY = transform.position.y - textBoxOffset;
            destination.y = adjustedTextBoxY;
            adjustedForTextBox = true;
        }
        else if (FindAnyObjectByType<Bear>().isTalking == true && adjustedForTextBox == true)
        {
            destination.y = adjustedTextBoxY;
        }


        // Interpolate from the current Camera position toward destination
        destination = Vector3.Lerp(transform.position, destination, easing);  // Smoothes the transtion between the initial camera position and the projectile).

        destination.z = camZ;  // Changes the destination Z position to prevent the camera from being to close. The "camZ" field holds the initial position of the camera. The camera is moved to the position of POI, except for the z coordinate, which is set to "camZ" every frame. This prevents the camera from being so close to the POI that the POI fills the frame or becomes invisible.

        transform.position = destination; // Aligns the camera position with the POI X and Y position values.



        Camera.main.orthographicSize = 10;  // Zooms out the camera to keep the ground in view when the projectile elevates too high.
    }

    public void SetUpCam()
    {
        camLimitL = FindAnyObjectByType<CamLimitL>().gameObject.transform.position;
        
        
        camLimitR = FindAnyObjectByType<CamLimitR>().gameObject.transform.position;
        
        if (FindAnyObjectByType<CamLimitUp>() != null)
        {
            camLimitUp = FindAnyObjectByType<CamLimitUp>().gameObject.transform.position;
        }
        
        camLimitDown = FindAnyObjectByType<CamLimitDown>().gameObject.transform.position;

        POI = FindAnyObjectByType<Bear>().gameObject;
        transform.position = POI.transform.position;

        if (POI.transform.position.x < camLimitL.x + offsetX)
        {
            transform.position = new Vector2(camLimitL.x + offsetX, transform.position.y);
        }

        if (POI.transform.position.x > camLimitR.x - offsetX)
        {
            transform.position = new Vector2(camLimitR.x - offsetX, transform.position.y);
        }

        if (FindAnyObjectByType<CamLimitUp>() != null && POI.transform.position.y > camLimitUp.y - offsetY)
        {
            transform.position = new Vector2(transform.position.x, camLimitUp.y - offsetY);
        }

        if (POI.transform.position.y < camLimitDown.y + offsetY)
        {
            transform.position = new Vector2(transform.position.x, camLimitDown.y + offsetY);
        }
    }
}
