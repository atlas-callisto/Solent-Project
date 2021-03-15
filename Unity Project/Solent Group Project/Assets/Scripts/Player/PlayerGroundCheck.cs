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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8 || collision.gameObject.layer == 10) // 8 = "Ground" layer, 10 = "Transparent Platform"
        {
            playerRef.isGrounded = true;
            playerRef.canDoubleJump = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8 || collision.gameObject.layer == 10)
        {
            playerRef.isGrounded = false;
        }
    }
}
