using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : EnemyAI
{
    [Header("Bat Stats")]
    [SerializeField] public float aggroDistance;
    [SerializeField] private float horizontalFlightSpeed;
    [SerializeField] private float verticalFlightSpeed;
    [SerializeField] private float runAwaySpeed;
    private Vector3 randomPoint;
    private Vector3 restingPoint;
    private Vector3 randomOffset;
    bool chasePlayer = true;

    protected override void Start()
    {
        base.Start();
        restingPoint = transform.position;
    }
    protected override void Update()
    {
        if (!base.isAlive) return;
        BatAI();
        IsMovingAnimation();
    }
    private void BatAI()
    {
        MeasureDistanceToThePlayer();
        if (fearDebuff)
        {
            myRB.gravityScale = 1;
            myRB.velocity = new Vector2(0, myRB.velocity.y);
        }
        else if (distanceToThePlayer <= aggroDistance && chasePlayer)
        {
            myRB.gravityScale = 0;
            TurnTowardsPlayer();
            if (distanceVect.y > 0) verticalFlightSpeed = Mathf.Abs(verticalFlightSpeed);
            else if (distanceVect.y < 0) verticalFlightSpeed = Mathf.Abs(verticalFlightSpeed) * -1;
            if (playerIsOnRightSide)
            {
                myRB.velocity = new Vector2(horizontalFlightSpeed, verticalFlightSpeed);
            }
            if (!playerIsOnRightSide)
            {
                myRB.velocity = new Vector2(-horizontalFlightSpeed, verticalFlightSpeed);
            }
        }
        else if (distanceToThePlayer <= aggroDistance && !chasePlayer) // Return to original point when player out of range
        {
            myRB.gravityScale = 0;
            myRB.velocity = Vector3.zero;
            transform.position = Vector3.MoveTowards(transform.position, randomPoint, Mathf.Abs(runAwaySpeed) * Time.deltaTime);
            if (transform.position == randomPoint)
            {
                chasePlayer = true;
            }
        }
        else if (transform.position != restingPoint)
        {
            myRB.gravityScale = 0;
            myRB.velocity = Vector3.zero;
            chasePlayer = true;
            transform.position = Vector3.MoveTowards(transform.position, restingPoint, Mathf.Abs(runAwaySpeed) * Time.deltaTime);
        }
    }

    protected override void OnCollisionStay2D(Collision2D collision)
    {
        base.OnCollisionStay2D(collision);
        if (chasePlayer == true)
        { 
        randomOffset = new Vector3 (UnityEngine.Random.Range(-2f,2f), UnityEngine.Random.Range(1f, 2f), 0);
        randomPoint = player.transform.position + randomOffset;
        chasePlayer = false;
        }
    }

    private void IsMovingAnimation()
    {
        //if(myRB.velocity.x != 0 || myRB.velocity.y != 0)
        if (transform.position != restingPoint)
        {
            myAnimator.SetBool("isMoving", true);
        }
        else
        {
            myAnimator.SetBool("isMoving", false);
        }
    }

}
