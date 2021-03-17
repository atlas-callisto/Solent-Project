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
    [SerializeField] int numOfWaves;

    [SerializeField] bool magicalBarrierIsOn = false; // Remove Expose later on
    [SerializeField] bool summoningMinions = false;   // Remove Expose later on

    [SerializeField] GameObject magicalBarrierGameObject;
    [SerializeField] GameObject zombieAIPrefab;
    [SerializeField] GameObject BatAIPrefab;

    [SerializeField] List<Transform> zombieSpawnLocations = new List<Transform>();
    [SerializeField] List<Transform> batSpawnLocations = new List<Transform>();


    


    Collider2D myCollider2D;
    private float rangedAttackTimer;
    private float meleeAttackTimer;
    public int minionCounter = 0;
    private int currentWave = 1;



    // Start is called before the first frame update
    protected override void Start()
    {
        myCollider2D = GetComponent<Collider2D>();
        magicalBarrierGameObject.SetActive(magicalBarrierIsOn);
    }

    // Update is called once per frame
    protected override void Update()
    {
        if(magicalBarrierIsOn && minionCounter == 0)
        {
            myRB.gravityScale = 1;
            myCollider2D.enabled = true;
            ToggleMagicalShield(false);
        }
        if(magicalBarrierIsOn)
        {
            myCollider2D.enabled = false;
            myRB.velocity = Vector3.zero;
            myRB.gravityScale = 0;
        }
        else if (!magicalBarrierIsOn && currentWave <= numOfWaves && !summoningMinions)
        {
            myRB.gravityScale = 1;
            myCollider2D.enabled = true;
            SummonEnemies();
        }
        else if(!magicalBarrierIsOn)
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
        }
        else if (distanceToThePlayer <= rangeAttackRange && distanceToThePlayer > meleeAttackRange) // Player is within boulder attack range but outside melee range
        {
            TurnTowardsPlayer();
            myRB.velocity = new Vector2(0, myRB.velocity.y);

            if (rangedAttackTimer >= rangedAttackInterval)
            {
                //do ranged attack
                rangedAttackTimer = 0;                
            }
        }
        else if (distanceToThePlayer <= meleeAttackRange)
        {
            TurnTowardsPlayer();
            myRB.velocity = new Vector2(moveSpeed, myRB.velocity.y);
            if (meleeAttackTimer >= meleeAttackInterval)
            {
                //Attack with staff
                meleeAttackTimer = 0;
            }
        }
    }

    private IEnumerator StartSummoning(float summomDuration)
    {
        summoningMinions = true;
        ToggleMagicalShield(false);
        myAnimator.SetBool("Summoning", true);

        yield return new WaitForSeconds(summomDuration);

        summoningMinions = false;
        ToggleMagicalShield(true);
        myAnimator.SetBool("Summoning", false);
    }
    private void SummonEnemies()
    {
        StartCoroutine(StartSummoning(5f));

        for (int i = 0; i < numOfZombiesToSpawn; i++)
        {
            GameObject minion = Instantiate(zombieAIPrefab, zombieSpawnLocations[0].position, Quaternion.identity);
            minion.AddComponent<MinionCounter>();
            minionCounter++;
        }
        if (currentWave >= 2) // Bats are summond in 3rd wave and after
        {
            for (int i = 0; i < numOfBatsToSpawn; i++)
            {
                GameObject minion = Instantiate(BatAIPrefab, batSpawnLocations[0].position, Quaternion.identity);
                minion.AddComponent<MinionCounter>();
                minionCounter++;
            }
        }
        currentWave++;
    }
    
    private void ToggleMagicalShield(bool toggle)
    {
        magicalBarrierIsOn = toggle;
        magicalBarrierGameObject.SetActive(toggle);
    }
}
