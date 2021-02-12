using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonLight : MonoBehaviour
{
    [SerializeField] Player player;
    void Awake()
    {
        player = FindObjectOfType<Player>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {        
        if (collision.tag == "Player")
        {
            Debug.Log("player");
        }
            player.wolf = true;

    }
}
