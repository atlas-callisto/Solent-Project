using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : MonoBehaviour
{
    [SerializeField] int healingAmount = 5;
    private Player playerRef;
    private void Start()
    {
        playerRef = FindObjectOfType<Player>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            playerRef.currentHealth = Mathf.Clamp(playerRef.currentHealth + healingAmount, 0, playerRef.maxHealth);
            Destroy(this.gameObject);
        }
    }
}
