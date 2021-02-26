using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonLight : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] float moonLightRegenRate = 2f;
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
            Mathf.Clamp(player.currentWolfBar += Time.deltaTime * moonLightRegenRate, 0, player.maxWolfBar);
        }

    }
}
