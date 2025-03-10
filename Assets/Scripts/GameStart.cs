using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStart : MonoBehaviour
{
    private CanvasThing sceneCanvas;
    private CamFollow mainCamera;
    private void Awake()
    {
        sceneCanvas = FindAnyObjectByType<CanvasThing>();
        
        mainCamera = FindAnyObjectByType<CamFollow>();
        sceneCanvas.gameObject.SetActive(false);
        mainCamera.gameObject.SetActive(false);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartLevel()
    {
        
        SceneManager.LoadScene("TutorialLvlDesign");
        sceneCanvas.gameObject.SetActive(true);
        mainCamera.gameObject.SetActive(true);
    }
}
