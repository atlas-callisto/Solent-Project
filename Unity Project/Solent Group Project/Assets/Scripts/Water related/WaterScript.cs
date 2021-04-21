using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterScript : MonoBehaviour
{
    public GameManager GM;
    [SerializeField] Player playerRef;
    [SerializeField] float moveSpeedSlowPercentage;
    [SerializeField] float slowDuration;

    private void Start()
    {
        playerRef = FindObjectOfType<Player>();

        if (GM.HasWheelTurned == true)
        {
            gameObject.SetActive(!WheelActivation.WheelActivated);
        }
      
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("WATERCOLLISION");
            playerRef.SlowMoveSpeedDebuff(moveSpeedSlowPercentage, slowDuration);
        }
    }
}
