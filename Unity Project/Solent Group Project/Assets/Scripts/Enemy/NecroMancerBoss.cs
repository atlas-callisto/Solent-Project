using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NecroMancerBoss : EnemyAI
{
    [Header("NecroMancer Stats")]
    [SerializeField] private float meleeAttackRange;
    [SerializeField] private float rangeAttackRange;

    [SerializeField] float meleeAttackInterval;
    [SerializeField] float rangedAttackInterval;

    [SerializeField] int numOfZombiesToSpawn;
    [SerializeField] int numOfBatsToSpawn;
    [SerializeField] int numOfSkeletonsToSpawn;
    [SerializeField] int numOfWaves;

    [SerializeField] bool magicalBarrierIsOn = false; // Remove Expose later on
    [SerializeField] bool summoningMinions = false;   // Remove Expose later on

    [SerializeField] GameObject magicalBarrierGameObject;
    [SerializeField] GameObject zombieAIPrefab;
    [SerializeField] GameObject batAIPrefab;
    [SerializeField] GameObject skeletonAIPrefab;
    [SerializeField] GameObject fireBallProjectilePrefab;
    [SerializeField] GameObject staffWeaponPrefab;


    [SerializeField] List<Transform> zombieSpawnLocations = new List<Transform>();
    [SerializeField] List<Transform> batSpawnLocations = new List<Transform>();
    [SerializeField] List<Transform> skeletonSpawnLocations = new List<Transform>();

    Collider2D myCollider2D;
    private float rangedAttackTimer;
    private float meleeAttackTimer;
    [SerializeField] public int minionCounter = 0;
    [SerializeField] private int currentWave = 1;

    protected override void Start()
    {
        myCollider2D = GetComponent<Collider2D>();
        magicalBarrierGameObject.SetActive(magicalBarrierIsOn);
    }
    protected override void Update()
    {
        if(summoningMinions)
        {
            myRB.velocity = Vector3.zero;
        }
        if(magicalBarrierIsOn && minionCounter == 0)
        {
            myRB.gravityScale = 1;
            myCollider2D.enabled = true;            
            myRB.velocity = Vector3.zero;
            ToggleMagicalShield(false);
        }
        if(magicalBarrierIsOn)
        {
            myRB.gravityScale = 0;
            myCollider2D.enabled = false;
            myRB.velocity = Vector3.zero;            
        }
        else if (!magicalBarrierIsOn && currentWave <= numOfWaves && !summoningMinions)
        {
            myRB.gravityScale = 1;
            myCollider2D.enabled = true;
            myRB.velocity = Vector3.zero;
            SummonEnemies();
        }
        else if(!magicalBarrierIsOn && !summoningMinions)
        {
            myRB.gravityScale = 1;
            myCollider2D.enabled = true;
            NecroMancerFightAI();
        }            
    }

    private void NecroMancerFightAI()
    {
        MeasureDistanceToThePlayer();
        meleeAttackTimer += Time.deltaTime;
        rangedAttackTimer += Time.deltaTime;
        if (distanceToThePlayer > rangeAttackRange)
        {
            base.EnemyAIChaseOrPatrol();
            myAnimator.SetBool("isWalking", true);
        }
        else if (distanceToThePlayer <= rangeAttackRange && distanceToThePlayer > meleeAttackRange) // Player is within boulder attack range but outside melee range
        {
            myAnimator.SetBool("isWalking", false);
            TurnTowardsPlayer();
            myRB.velocity = new Vector2(0, myRB.velocity.y);

            if (rangedAttackTimer >= rangedAttackInterval)
            {
                //do ranged attack
                GameObject enemyFireBall = Instantiate(fireBallProjectilePrefab, transform.position, transform.localRotation);
                rangedAttackTimer = 0;                
            }
        }
        else if (distanceToThePlayer <= meleeAttackRange)
        {
            myAnimator.SetBool("isWalking", true);
            base.EnemyAIChaseOrPatrol();
            if (meleeAttackTimer >= meleeAttackInterval)
            {
                //Attack with staff
                staffWeaponPrefab.SetActive(true);
                meleeAttackTimer = 0;
            }
        }
    }

    private IEnumerator StartSummoning()
    {
        summoningMinions = true;
        ToggleMagicalShield(false);
        myAnimator.SetBool("summoning", true);
        for (int i = 0; i < numOfZombiesToSpawn; i++)
        {
            GameObject minion = Instantiate(zombieAIPrefab, zombieSpawnLocations[0].position, Quaternion.identity);
            minion.AddComponent<MinionCounter>();
            minionCounter++;
            yield return new WaitForSeconds(1.5f);
        }
        if (currentWave >= 2) // Bats are summond in 2nd wave and after
        {
            for (int i = 0; i < numOfBatsToSpawn; i++)
            {
                GameObject minion = Instantiate(batAIPrefab, batSpawnLocations[0].position, Quaternion.identity);
                minion.AddComponent<MinionCounter>();
                minionCounter++;
                yield return new WaitForSeconds(1.5f);
            }
        }
        if(currentWave >= 3) // Skeleton are summond in 3rd wave
        {
            for (int i = 0; i < numOfSkeletonsToSpawn; i++)
            {
                GameObject minion = Instantiate(skeletonAIPrefab, skeletonSpawnLocations[0].position, Quaternion.identity);
                minion.AddComponent<MinionCounter>();
                minionCounter++;
                yield return new WaitForSeconds(1.5f);
            }       
        }
        currentWave++;
        summoningMinions = false;
        ToggleMagicalShield(true);
        myAnimator.SetBool("summoning", false);        
    }
    private void SummonEnemies()
    {
        StartCoroutine(StartSummoning());
    }
    
    private void ToggleMagicalShield(bool toggle)
    {
        magicalBarrierIsOn = toggle;
        magicalBarrierGameObject.SetActive(toggle);
    }
}
