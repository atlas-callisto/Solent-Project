using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Patrol")]
    public GameObject leftPatrolPoint;
    public GameObject rightPatrolPoint;
    public bool shouldPatrol = true;
    public float patrolmoveSpeed = 2;

    private Vector3 pointA = new Vector3(0, 0, 0);
    private Vector3 pointB = new Vector3(0, 0, 0);
    private bool moveTowardspointB = true;

    private Rigidbody2D myRB;
    private SpriteRenderer mySpriteRenderer;

    void Awake()
    {
        myRB = GetComponent<Rigidbody2D>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        pointA = leftPatrolPoint.transform.position;
        pointB = rightPatrolPoint.transform.position;
    }
    public void Patrol()
    {
        if (pointA == null || pointB == null) return;
        if (transform.position.x <= pointA.x) moveTowardspointB = true;
        if (transform.position.x >= pointB.x) moveTowardspointB = false;
        if (moveTowardspointB)
        {
            transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
            myRB.velocity = new Vector2(patrolmoveSpeed, myRB.velocity.y);
        }
        else
        {
            myRB.velocity = new Vector2(-patrolmoveSpeed, myRB.velocity.y);
            transform.localRotation = Quaternion.Euler(new Vector3(0, 180, 0));
        }
    }
}
