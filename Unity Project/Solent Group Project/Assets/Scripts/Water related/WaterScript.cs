using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterScript : MonoBehaviour
{
    
    [SerializeField] float moveSpeedSlowPercentage;
    private Player playerRef;
    private bool playerIsOnWater;

    private void Start()
    {
        playerRef = FindObjectOfType<Player>();
        gameObject.SetActive(!WheelActivation.WheelActivated);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerIsOnWater = true;
            playerRef.SlowDebuffWater(moveSpeedSlowPercentage,playerIsOnWater);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerIsOnWater = false;
            playerRef.SlowDebuffWater(moveSpeedSlowPercentage, playerIsOnWater);
        }
    }
}
