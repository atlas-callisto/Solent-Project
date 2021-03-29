using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonLight : MonoBehaviour
{    
    [SerializeField] float moonLightRegenRate = 2f;

    Player player;
    void Awake()
    {
        player = FindObjectOfType<Player>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {        
        if (collision.tag == "Player")
        {
            player.wolf = true;
        }         
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Mathf.Clamp(Player.currentWolfBar += Time.deltaTime * moonLightRegenRate, 0, Player.maxWolfBar);
        }

    }
}
