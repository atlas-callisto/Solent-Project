using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    internal int basicAttackDamage = 1;
    internal int heavyAttackDamage = 2;
    internal int specialAttackDamage = 4;
    internal int damage = 1;
    Animator myAnimator;

    void Awake()
    {
        myAnimator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamageable iDamageable = collision.GetComponent<IDamageable>();
        if (iDamageable != null)
        {
            iDamageable.TakeDamage(damage);
        }
    }
    public void Attack(int attackType)
    {
        myAnimator.SetInteger("Attack Type", attackType);
    }
    public void SetDamage(int attackType)
    {
        if(attackType == 1) // basic attack
        {
            damage = basicAttackDamage;
        }
        else if (attackType == 2) // Heavy attack
        {
            damage = heavyAttackDamage;
        }
        else if (attackType == 3) // basic attack
        {
            damage = specialAttackDamage; // special attack
        }
    }

    public void FinishedAttacking() //called by animation event
    {
        this.gameObject.SetActive(false);
    }
}
