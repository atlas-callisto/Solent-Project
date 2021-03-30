using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundCheck : MonoBehaviour
{
    Player playerRef;
    void Start()
    {
        playerRef = FindObjectOfType<Player>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8) // 8 = "Ground" layer
        {
            playerRef.isGrounded = true;
            playerRef.canDoubleJump = true;
            playerRef.GetComponent<Animator>().SetBool("OnAir", false);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            playerRef.isGrounded = false;
            playerRef.GetComponent<Animator>().SetBool("OnAir", true);
        }
    }
}
