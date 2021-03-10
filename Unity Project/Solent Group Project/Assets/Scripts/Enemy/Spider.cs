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

    private float attackTimer = 0;
    private Vector3 restingPoint;

    [SerializeField] private GameObject webPrefab;
    [SerializeField] private float shootingInterval;

    protected override void Start()
    {
        base.Start();
        restingPoint = transform.position;
        attackTimer = shootingInterval; // Enemy can immediately shoot in the begining
    }
    protected override void Update()
    {
        if (!base.isAlive) return;
        SpiderAI();
        attackTimer += Time.deltaTime;
    }
    private void SpiderAI()
    {
        Vector3 distanceVect = base.player.transform.position - transform.position;
        distanceToThePlayer = Vector3.Distance(base.player.transform.position, transform.position);

        //Vector3.Normalize(distanceVect) Remove it later?
        playerIsOnRightSide = distanceVect.x > 0 ? true : false;
        if (distanceToThePlayer <= aggroDistance)
        {
            if (distanceVect.y > 0) verticalMoveSpeed = Mathf.Abs(verticalMoveSpeed);
            else if (distanceVect.y < 0) verticalMoveSpeed = Mathf.Abs(verticalMoveSpeed) * -1;
            if (playerIsOnRightSide)
            {
                transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
                myRB.velocity = new Vector2(0, verticalMoveSpeed);
            }
            else if (!playerIsOnRightSide)
            {
                transform.localRotation = Quaternion.Euler(new Vector3(0, 180, 0));
                myRB.velocity = new Vector2(0, verticalMoveSpeed);
            }
            
            if (transform.position.y <= player.transform.position.y + 1f || transform.position.y >= player.transform.position.y - 1f)
            {
                ShootWeb();
            }
        }
        else if (transform.position != restingPoint) // Return to original point when player out of range
        {
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
