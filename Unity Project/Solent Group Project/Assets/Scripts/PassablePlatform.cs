using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassablePlatform : MonoBehaviour
{
    private PlatformEffector2D myPlatformEffector2D;
    private Rigidbody2D playerRB;
    private bool platformCanFlip = true;

    private void Awake()
    {
        playerRB = FindObjectOfType<Player>().gameObject.GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        myPlatformEffector2D = GetComponent<PlatformEffector2D>();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && platformCanFlip)
        {
            if (Input.GetAxis("Vertical") <= -0.2 && playerRB.velocity.y <= 0)
            {
                platformCanFlip = false;
                myPlatformEffector2D.rotationalOffset = 180;
                StartCoroutine(ResetPlatformOrientation());
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            myPlatformEffector2D.rotationalOffset = 0;
        }
    }
    IEnumerator ResetPlatformOrientation()
    {
        Debug.Log("Co-Called");
        yield return new WaitForSeconds(0.1f);
        platformCanFlip = true;

    }
}
