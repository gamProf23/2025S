using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayTestEnd : MonoBehaviour
{
    TMP_Text berryText;
    TMP_Text fishText;
    TMP_Text honeyText;

    Button exitButton;

    TMP_Text animalText;

    
    private void Awake()
    {
        berryText = transform.GetChild(1).GetComponent<TMP_Text>();
        fishText = transform.GetChild(2).GetComponent<TMP_Text>();
        honeyText = transform.GetChild(3).GetComponent<TMP_Text>();
        exitButton = transform.GetChild(4).GetComponent<Button>();
        animalText = transform.GetChild(5).GetComponent<TMP_Text>();

        exitButton.onClick.AddListener(ExitButton);
        berryText.text = FindFirstObjectByType<CanvasThing>().berryCount.ToString();
        fishText.text = FindFirstObjectByType<CanvasThing>().fishCount.ToString();
        honeyText.text = FindFirstObjectByType<CanvasThing>().honeyCount.ToString();
        animalText.text = "Animals Helped: " + FindAnyObjectByType<CanvasThing>().cmAmount.ToString() + "/17";

        Destroy(FindAnyObjectByType<Bear>().gameObject);
        Destroy(FindAnyObjectByType<CamFollow>().gameObject);
        Destroy(FindAnyObjectByType<CanvasThing>().gameObject);
    }

    void Start()
    {
        
    }

    void ExitButton()
    {
       
        SceneManager.LoadScene("Credits");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
