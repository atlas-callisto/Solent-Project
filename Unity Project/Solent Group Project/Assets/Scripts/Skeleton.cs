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
        SkeletonAI();
    }
    private void SkeletonAI()
    {
        attackTimer += Time.deltaTime;
        if (distanceToThePlayer > attackRange)
        {
            base.ChasePlayer();
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
    private void TurnTowardsPlayer()
    {
        Vector3 distanceVect = player.transform.position - transform.position;
        playerIsOnRightSide = distanceVect.x > 0 ? true : false;
        if (playerIsOnRightSide)
        {
            transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }
        if (!playerIsOnRightSide)
        {
            transform.localRotation = Quaternion.Euler(new Vector3(0, 180, 0));
        }
    }
}
