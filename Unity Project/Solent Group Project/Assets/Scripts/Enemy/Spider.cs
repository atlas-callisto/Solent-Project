using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : EnemyAI
{
    [Header("Spider Stats")]
    [SerializeField] private float aggroDistance;
    [SerializeField] private float verticalMoveSpeed;
    [SerializeField] private float returnSpeed;
    [SerializeField] private float maxVerticalMoveDistance;

    private float originYPos;
    private float attackTimer = 0;
    private float currentVerticalMoveSpeed;
    private Vector3 restingPoint;

    [Header("Spider Shooting")]
    [SerializeField] private GameObject webPrefab;
    [SerializeField] private float shootingInterval;

    protected override void Start()
    {
        base.Start();
        originYPos =   transform.position.y;
        currentVerticalMoveSpeed = verticalMoveSpeed;
        restingPoint = transform.position;
        attackTimer = shootingInterval; // Enemy can immediately shoot in the begining
    }
    protected override void Update()
    {
        if (!base.isAlive) return;
        MeasureDistanceToThePlayer();
        AdjustHealthBarOrientation();
        SpiderAI();
        TurnTowardsPlayer();
        attackTimer += Time.deltaTime;
    }
    private void SpiderAI()
    {
        MeasureDistanceToThePlayer();
        float distanceToOrigin = Mathf.Abs(originYPos - transform.position.y);
        if (fearDebuff)
        {
            myRB.gravityScale = 1;
        }
        else if (distanceToThePlayer <= aggroDistance)
        {
            myRB.gravityScale = 0;
            
            if ((Mathf.Abs(distanceVect.y) + 0.2f) > 0 && (Mathf.Abs(distanceVect.y) - 0.2f) < 0) // Don't bother with the Spider AI logic, My head hurts after coding this
            {
                currentVerticalMoveSpeed = 0;
            }
            else if (distanceVect.y > 0)
            {
                if (distanceToOrigin > maxVerticalMoveDistance) currentVerticalMoveSpeed = -verticalMoveSpeed;
                else currentVerticalMoveSpeed = verticalMoveSpeed;
            }
            else if (distanceVect.y < 0)
            {
                if (distanceToOrigin > maxVerticalMoveDistance) currentVerticalMoveSpeed = verticalMoveSpeed;
                else currentVerticalMoveSpeed = verticalMoveSpeed * -1;
            }

            myRB.velocity = new Vector2(myRB.velocity.x, currentVerticalMoveSpeed);

            if (transform.position.y <= player.transform.position.y + 1f || transform.position.y >= player.transform.position.y - 1f)
            {
                ShootWeb();
            }
        }               
        else if (transform.position != restingPoint) // Return to original point when player out of range
        {
            myRB.gravityScale = 0;
            myRB.velocity = Vector3.zero;            
            transform.position = Vector3.MoveTowards(transform.position, restingPoint, Mathf.Abs(returnSpeed) * Time.deltaTime);
        }
    }

    private void ShootWeb()
    {        
        if(attackTimer >= shootingInterval)
        {
            Instantiate(webPrefab, transform.position, transform.localRotation);
            attackTimer = 0;
        }
    }
}
