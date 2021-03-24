using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WereWolfBoss : EnemyAI
{
    [Header("WereWolfBoss Stats")]
    [SerializeField] private float clawAttackRange;
    [SerializeField] private float clawAttackInterval;

    [SerializeField] private float boulderAttackRange;
    [SerializeField] private float boulderAttackInterval;
    [SerializeField] private int numberOfRocksToSpawn;

    [SerializeField] private GameObject weapon;
    [SerializeField] private GameObject fallingRockPrefab;

    private CameraScript myCam;
    private float clawAttackTimer;
    private float boulderAttackTimer;
    private float rockYOffset = 10;
    private float rockXOffset = 1;

    //WereWolfBoss Created. The Boss Patrols between two points, 
    //if the player moves within its boulder attack range, 
    //it screams (Dont have the sound or animations) and summons a 
    //number of boulders above the player
    protected override void Start()
    {
        base.Start();
        myCam = FindObjectOfType<CameraScript>();
        clawAttackTimer = clawAttackInterval;
        boulderAttackTimer = boulderAttackInterval;
    }
    protected override void Update()
    {
        if (!base.isAlive) return;
        MeasureDistanceToThePlayer();
        WereWolfBossAI();
    }
    private void WereWolfBossAI()
    {
        clawAttackTimer += Time.deltaTime;
        boulderAttackTimer += Time.deltaTime;
        if (distanceToThePlayer > boulderAttackRange)
        {
            base.EnemyAIChaseOrPatrol();
        }
        else if (distanceToThePlayer <= boulderAttackRange && distanceToThePlayer > clawAttackRange && boulderAttackTimer >= boulderAttackInterval) // Player is within boulder attack range but outside melee range
        {
            TurnTowardsPlayer();
            myRB.velocity = new Vector2(0, myRB.velocity.y);
            if(boulderAttackTimer >= boulderAttackInterval)
            {
                StartCoroutine(myCam.CameraShake(1f, 0.5f));
                boulderAttackTimer = 0;
                for(int i = 0; i < numberOfRocksToSpawn; i++)
                {
                    rockYOffset = UnityEngine.Random.Range(8, 16);
                    rockXOffset = UnityEngine.Random.Range(-5, 5); // Spawn boulder with this Offset on top of players x axis.
                    Instantiate(fallingRockPrefab, player.transform.position + new Vector3(rockXOffset, rockYOffset), Quaternion.identity);
                }
            }

        }
        else if (distanceToThePlayer > clawAttackRange && boulderAttackTimer < boulderAttackInterval)
        {
            base.EnemyAIChaseOrPatrol();
        }
        else if (distanceToThePlayer <= clawAttackRange)
        {
            TurnTowardsPlayer();
            myRB.velocity = new Vector2(0, myRB.velocity.y);
            if(clawAttackTimer >= clawAttackInterval)
            {
                clawAttackTimer = 0;
                weapon.SetActive(true);
            }
        }
    }
}
