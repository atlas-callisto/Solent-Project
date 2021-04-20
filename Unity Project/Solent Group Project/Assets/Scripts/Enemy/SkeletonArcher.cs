using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonArcher : EnemyAI
{

    [Header("Skeleton Stats")]
    [SerializeField] private float attackRange;
    [SerializeField] private float attackInterval;
    [SerializeField] private float runAwayRange; // If player is within this range, the skeleton tries to run away from the player

    [SerializeField] BoxCollider2D wallChecker; // If the box collider is touching the wall, it means skeleton is near the wall and cannot run beyond it

    [SerializeField] private GameObject arrowPrefab;

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
        SkeletonArcherAI();
    }
    private void SkeletonArcherAI()
    {
        attackTimer += Time.deltaTime;
        if (fearDebuff)
        {
            RunAwayFromPlayer();
        }
        else if (distanceToThePlayer > attackRange)
        {
            myEnemyPatrol.Patrol();
        }
        else if (distanceToThePlayer > runAwayRange) // Player is withing attack range but outside runAway range so enemy will attack
        {
            myRB.velocity = new Vector2(0, myRB.velocity.y); // The skeleton stops moving
            TurnTowardsPlayer();
            if (attackTimer >= attackInterval)
            {
                // Attack Animation Maybe
                // Attack SFX
                GameObject enemyArrow = Instantiate(arrowPrefab, transform.position, transform.localRotation);
                attackTimer = 0;
            }
            //var hits2D = Physics2D.RaycastAll(this.transform.position, Vector2.right, attackRange);
            //foreach (RaycastHit2D hit in hits2D)
            //{               
            //    if (hit.transform.gameObject.tag == "Player")
            //    {
            //        myRB.velocity = new Vector2(0, myRB.velocity.y); // The skeleton stops moving
            //        TurnTowardsPlayer();
            //        if (attackTimer >= attackInterval)
            //        {
            //            // Attack Animation Maybe
            //            GameObject enemyArrow = Instantiate(arrowPrefab, transform.position, transform.localRotation);
            //            attackTimer = 0;
            //        }
            //    }
            //}

        }
        else if (distanceToThePlayer <= runAwayRange && !wallChecker.IsTouchingLayers(LayerMask.GetMask("Ground")) && !wallChecker.IsTouchingLayers(LayerMask.GetMask("EnemyLedgeGuard"))) // Player is too close so runaway from the player. Unless near the wall
        {
            RunAwayFromPlayer();
        }
        else // this means no where left to run, so just attack the player
        {
            myRB.velocity = new Vector2(0, myRB.velocity.y);
            TurnTowardsPlayer();
            if (attackTimer >= attackInterval)
            {                
                // Attack Animation Maybe
                GameObject enemyArrow = Instantiate(arrowPrefab, transform.position, transform.localRotation);
                attackTimer = 0;
            }
        }        
    }
}
