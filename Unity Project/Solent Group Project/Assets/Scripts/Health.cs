using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] public int health;
    public void TakeDamage(int damage)
    {
        health -= damage;
    }
}
