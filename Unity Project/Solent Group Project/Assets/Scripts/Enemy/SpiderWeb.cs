using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderWeb : MonoBehaviour
{
    [SerializeField] float moveSpeedSlowPercentage;
    [SerializeField] float slowDuration;
    [SerializeField] Player playerRef;

    private void Start()
    {
        playerRef = FindObjectOfType<Player>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            playerRef.SlowMoveSpeedDebuff(moveSpeedSlowPercentage, slowDuration);
        }
    }
}
