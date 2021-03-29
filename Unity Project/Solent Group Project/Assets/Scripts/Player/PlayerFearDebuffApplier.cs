using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFearDebuffApplier : MonoBehaviour
{
    [SerializeField] float fearDebuffDuration = 4f;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy"))
        {
            EnemyAI enemyAI = collision.GetComponent<EnemyAI>();
            if (enemyAI)
            {
                enemyAI.StartCoroutine(enemyAI.ApplyFearDebuff(fearDebuffDuration));
            }
        }
    }

}
