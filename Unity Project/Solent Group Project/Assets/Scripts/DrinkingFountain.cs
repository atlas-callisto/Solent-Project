using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrinkingFountain : MonoBehaviour , Interactable
{    
    [Tooltip("The amount of health the player regenerates per tick")]
    public int healthRegenAmount = 1;
    [Tooltip("The amount of ticks per second")]
    public int healthRegenRate = 1;

    private bool healPlayer = false;
    private float healthRegenTimer = 0;
    private void Update()
    {
        healthRegenTimer += Time.deltaTime;
        if (healPlayer && healthRegenTimer > (1f/ (float)healthRegenRate))
        {
            healthRegenTimer = 0;
            Player.currentHealth += healthRegenAmount;
        }
    }
    public void Interact()
    {
        healPlayer = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player") healPlayer = false;
    }
}
