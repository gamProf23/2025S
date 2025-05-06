using UnityEngine;

public class FinalBonusOpen : MonoBehaviour
{
    private void Awake()
    {
        if (FindAnyObjectByType<CanvasThing>().allGold == true)
        {
            gameObject.SetActive(true);
            transform.position = new Vector3(-70.1217f, -15.07157f);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    void Start()
    {

    }

    void Update()
    {
      
    }
}
