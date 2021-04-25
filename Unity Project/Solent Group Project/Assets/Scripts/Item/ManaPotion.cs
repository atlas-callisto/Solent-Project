using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaPotion : MonoBehaviour
{
    [SerializeField] int manaAmount = 5;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Player.currentWolfBar = Mathf.Clamp(Player.currentWolfBar + manaAmount, 0, Player.maxWolfBar);
            Destroy(this.gameObject);
        }
    }
}
