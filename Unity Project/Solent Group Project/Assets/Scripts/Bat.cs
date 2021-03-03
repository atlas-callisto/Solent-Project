using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : EnemyAI
{
    [SerializeField] public float aggroDistance;
    [SerializeField] private float horizontalFlightSpeed;
    [SerializeField] private float verticalFlightSpeed;
    protected override void Update()
    {
        if (!base.isAlive) return;
        BatChasePlayer();
    }
    private void BatChasePlayer()
    {
        Vector3 distanceVect = base.player.transform.position - transform.position;
        distanceToThePlayer = Vector3.Distance(base.player.transform.position, transform.position);

        //Vector3.Normalize(distanceVect) Remove it later?
        playerIsOnRightSide = distanceVect.x > 0 ? true : false;
        if (distanceToThePlayer <= aggroDistance)
        {
            if (distanceVect.y > 0) verticalFlightSpeed = Mathf.Abs(verticalFlightSpeed);
            else if (distanceVect.y < 0) verticalFlightSpeed = Mathf.Abs(verticalFlightSpeed) * -1;
            if (playerIsOnRightSide)
            {
                transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
                myRB.velocity = new Vector2(horizontalFlightSpeed, verticalFlightSpeed);
            }
            if (!playerIsOnRightSide)
            {
                transform.localRotation = Quaternion.Euler(new Vector3(0, 180, 0));
                myRB.velocity = new Vector2(-horizontalFlightSpeed, verticalFlightSpeed);
            }
        }

    }

}
