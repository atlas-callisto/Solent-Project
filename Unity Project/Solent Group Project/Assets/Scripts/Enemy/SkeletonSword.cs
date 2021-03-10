using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonSword : MonoBehaviour
{
    [SerializeField] private int damage = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamageable iDamageable = collision.GetComponent<IDamageable>();
        if (iDamageable != null)
        {
            iDamageable.TakeDamage(damage);
        }
    }
    public void FinishedAttacking() //called by animation event
    {
        this.gameObject.SetActive(false);
    }
}
