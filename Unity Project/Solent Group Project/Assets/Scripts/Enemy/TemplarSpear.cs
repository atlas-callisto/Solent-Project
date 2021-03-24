using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemplarSpear : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    public bool playerWasHit = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamageable iDamageable = collision.GetComponent<IDamageable>();
        if (iDamageable != null)
        {
            iDamageable.TakeDamage(damage);
            if (collision.gameObject.GetComponent<Player>())
            {
                playerWasHit = true;
                collision.gameObject.GetComponent<Player>().KnockBackEffect(collision.gameObject.transform.position - this.transform.position);
            }
        }
    }
    public void FinishedAttacking() //called by animation event
    {
        this.gameObject.SetActive(false);
    }
}
