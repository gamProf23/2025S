using UnityEngine;
using UnityEngine.UI;

public class ClimbingMeter : MonoBehaviour
{
    Image redMeter;
    Image greenMeter;

    Bear player;

    private void Awake()
    {
        player = FindAnyObjectByType<Bear>();
        redMeter = transform.GetChild(0).GetComponent<Image>();
        greenMeter = transform.GetChild(1).GetComponent<Image>();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (player.isClimbing == true)
        {
            redMeter.gameObject.SetActive(true);
            greenMeter.gameObject.SetActive(true);

            greenMeter.transform.localScale = new Vector3(1, player.climbingTimer / player.climbingTimerI, 1);
        }
        else
        {
            redMeter.gameObject.SetActive(false);
            greenMeter.gameObject.SetActive(false);
        }
    }
}
