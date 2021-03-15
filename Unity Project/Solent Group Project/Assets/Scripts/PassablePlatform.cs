using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassablePlatform : MonoBehaviour
{
    private PlatformEffector2D myPlatformEffector2D;
    private Rigidbody2D playerRB;

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
        if(collision.gameObject.tag == "Player")
        {
            if(Input.GetAxis("Vertical") <= -0.2 && playerRB.velocity.y <= 0)
            {
                myPlatformEffector2D.rotationalOffset = 180;
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

}
