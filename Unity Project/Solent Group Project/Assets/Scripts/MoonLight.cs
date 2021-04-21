using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonLight : MonoBehaviour
{    
    [SerializeField] float moonLightRegenRate = 2f;
    private bool playerIsOnMoonlight = false;

    Player player;
    void Start()
    {
        player = FindObjectOfType<Player>();
    }

    private void Update()
    {
        if(playerIsOnMoonlight) Mathf.Clamp(Player.currentWolfBar += Time.deltaTime * moonLightRegenRate, 0, Player.maxWolfBar);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {        
        if (collision.tag == "Player")
        {
            playerIsOnMoonlight = true;
            if(!Player.canTransformIntoWolf)
            {
                Player.canTransformIntoWolf = true;
                player.wolf = true;
            }
        }         
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerIsOnMoonlight = false;
        }

    }
}
