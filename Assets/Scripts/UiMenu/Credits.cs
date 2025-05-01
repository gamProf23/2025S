using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{

    Button exitButton;
    void Awake()
    {
        exitButton = transform.GetChild(1).GetComponent<Button>();
        exitButton.onClick.AddListener(ExitButton);
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void ExitButton()
    {
        SceneManager.LoadScene("TitleScreen");
    }
}
