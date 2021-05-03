using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : EnemyAI
{
    [Header("Skeleton Stats")]
    [SerializeField] private float attackRange;
    [SerializeField] private float attackInterval;
    [SerializeField] private GameObject weapon;

    private float attackTimer;

    protected override void Start()
    {
        base.Start();
        attackTimer = attackInterval;
    }
    protected override void Update()
    {
        if (!base.isAlive) return;
        MeasureDistanceToThePlayer();
        AdjustHealthBarOrientation();
        SkeletonAI();
    }
    private void SkeletonAI()
    {
        attackTimer += Time.deltaTime;
        if(fearDebuff)
        {
            print("fear");
            RunAwayFromPlayer();
        }
        else if (distanceToThePlayer > attackRange)
        {
            myAnimator.SetBool("IsWalking", true);
            base.EnemyAIChaseOrPatrol();
        }
        else if (distanceToThePlayer <= attackRange)
        {
            TurnTowardsPlayer();
            myRB.velocity = new Vector2(0, myRB.velocity.y);
            if (attackTimer >= attackInterval)
            {
                weapon.SetActive(true); // Activates the weapon child and it does damage to the player
                attackTimer = 0;
            }
        }
    }
}
