using UnityEngine;

public class FinalBonusOpen : MonoBehaviour
{
    private void Awake()
    {
        if (FindAnyObjectByType<CanvasThing>().allGold == true)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
            transform.position = new Vector3(1000, 1000);
        }
    }

    void Start()
    {

    }

    void Update()
    {

    }
}
