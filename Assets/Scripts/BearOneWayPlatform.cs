using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class BearOneWayPlatform : MonoBehaviour
{
    private GameObject currentOneWayPlatform;

    private Collider2D playerCollider;

    private void Awake()
    {
        playerCollider = GetComponent<CapsuleCollider2D>();
    }

    private void Update()
    {
        if (FindAnyObjectByType<Bear>().isBall == true)
        {
            playerCollider = transform.GetChild(1).GetComponent<CircleCollider2D>();
        }
        else
        {
            playerCollider = GetComponent<CapsuleCollider2D>();
        }

        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (currentOneWayPlatform != null)
            {
                StartCoroutine(DisableCollision());
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("OneWayPlatform"))
        {
            currentOneWayPlatform = collision.gameObject;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("OneWayPlatform"))
        {
            currentOneWayPlatform = null;
        }
    }

    private IEnumerator DisableCollision()
    {
        BoxCollider2D platformCollider = currentOneWayPlatform.GetComponent<BoxCollider2D>();

        Physics2D.IgnoreCollision(playerCollider, platformCollider);
        yield return new WaitForSeconds(0.75f);
        Physics2D.IgnoreCollision(playerCollider, platformCollider, false);
    }
}