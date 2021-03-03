using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : EnemyAI
{
    [SerializeField] public float aggroDistance;
    [SerializeField] private float verticalMoveSpeed;

    [SerializeField] private GameObject webPrefab;
    [SerializeField] private float shootingInterval;
    protected override void Update()
    {
        if (!base.isAlive) return;
        SpiderAI();
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
            if (!playerIsOnRightSide)
            {
                transform.localRotation = Quaternion.Euler(new Vector3(0, 180, 0));
                myRB.velocity = new Vector2(0, verticalMoveSpeed);
            }
            ShootWeb();
        }
    }

    private void ShootWeb()
    {
        Instantiate(webPrefab, transform.position, transform.localRotation);
    }
}
