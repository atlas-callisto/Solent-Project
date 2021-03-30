using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelActivation : MonoBehaviour
{
    private Animator myAnimator;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            myAnimator.SetTrigger("Interact");
        }
        
    }
}
