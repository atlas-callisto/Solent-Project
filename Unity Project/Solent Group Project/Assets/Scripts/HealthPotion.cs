using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : MonoBehaviour
{
    [SerializeField] int healingAmount = 5;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            Player.currentHealth = Mathf.Clamp(Player.currentHealth + healingAmount, 0, Player.maxHealth);
            Destroy(this.gameObject);
        }
    }
}
