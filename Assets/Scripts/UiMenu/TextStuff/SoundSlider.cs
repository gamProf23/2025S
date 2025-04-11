using UnityEngine;
using UnityEngine.UI;
public class SoundSlider : MonoBehaviour
{
    Slider mySlider;

    private void Awake()
    {
        mySlider = transform.GetChild(0).GetComponent<Slider>();
        GetComponent<Image>().type = Image.Type.Filled;
        GetComponent<Image>().fillMethod = Image.FillMethod.Horizontal;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Image>().fillAmount = mySlider.value;
        //Debug.Log(GetComponent<Image>().fillAmount);
    }
}
