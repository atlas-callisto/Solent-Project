using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassablePlatform : MonoBehaviour
{
    private PlatformEffector2D myPlatformEffector2D;
    void Start()
    {
        myPlatformEffector2D = GetComponent<PlatformEffector2D>();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if(Input.GetKey(KeyCode.S))
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
