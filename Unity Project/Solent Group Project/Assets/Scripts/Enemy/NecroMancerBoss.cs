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

    [Header("Summoning Stats")]
    [SerializeField] int numOfZombiesToSpawn;
    [SerializeField] int numOfBatsToSpawn;
    [SerializeField] int numOfSkeletonsToSpawn;
    [SerializeField] int numOfWaves;
    [SerializeField] private float summoningDuration;   

    [Header("Object Refs")]
    [SerializeField] GameObject magicalBarrierGameObject;
    [SerializeField] GameObject zombieAIPrefab;
    [SerializeField] GameObject batAIPrefab;
    [SerializeField] GameObject skeletonAIPrefab;
    [SerializeField] GameObject fireBallProjectilePrefab;
    [SerializeField] GameObject staffWeaponPrefab;

    [Header("SFX Refs")]
    [SerializeField] private AudioClip fireBallSFX;
    [SerializeField] private AudioClip staffAttackSFX;

    [SerializeField] List<Transform> zombieSpawnLocations = new List<Transform>();
    [SerializeField] List<Transform> batSpawnLocations = new List<Transform>();
    [SerializeField] List<Transform> skeletonSpawnLocations = new List<Transform>();

    private bool magicalBarrierIsOn = false;
    private bool summoningMinions = false;    
    private float rangedAttackTimer;
    private float meleeAttackTimer;
    internal int minionCounter = 0; // internal becaused its accessed by minion counter
    private int currentWave = 1;
    Collider2D myCollider2D;

    protected override void Awake()
    {
        base.Awake();
        if (GameManager.myGameManager.necromancerBossDefeated)
        {
            Destroy(this.gameObject);
            return;
        }
    }
    protected override void Start()
    {
        myCollider2D = GetComponent<Collider2D>();
        magicalBarrierGameObject.SetActive(magicalBarrierIsOn);
    }
    protected override void Update()
    {
        if (!isAlive) return;
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
        else if (distanceToThePlayer <= rangeAttackRange && distanceToThePlayer > meleeAttackRange) // Player is within projectile attack range but outside melee range
        {
            myAnimator.SetBool("isWalking", false);
            TurnTowardsPlayer();
            myRB.velocity = new Vector2(0, myRB.velocity.y);

            if (rangedAttackTimer >= rangedAttackInterval)
            {
                //do ranged attack
                //SFX??
                // AudioSource.PlayClipAtPoint(fireBallSFX, Camera.main.transform.position);
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
                //AudioSource.PlayClipAtPoint(staffAttackSFX, Camera.main.transform.position);
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
        yield return new WaitForSeconds(summoningDuration);

        if (currentWave == 3) numOfZombiesToSpawn = 2;
        for (int i = 0; i < numOfZombiesToSpawn; i++)
        {
            GameObject minion = Instantiate(zombieAIPrefab, zombieSpawnLocations[i].position, Quaternion.identity);
            minion.GetComponent<EnemyAI>().chaseDistance = 20f;
            minion.AddComponent<MinionCounter>();
            minionCounter++;
        }
        if (currentWave >= 2) // Bats are summond in 2nd wave and after
        {
            for (int i = 0; i < numOfBatsToSpawn; i++)
            {                
                GameObject minion = Instantiate(batAIPrefab, batSpawnLocations[i].position, Quaternion.identity);
                minion.GetComponent<EnemyAI>().chaseDistance = 20f;
                minion.AddComponent<MinionCounter>();
                minionCounter++;
            }
        }
        if(currentWave >= 3) // Skeleton are summond in 3rd wave
        {
            for (int i = 0; i < numOfSkeletonsToSpawn; i++)
            {
                GameObject minion = Instantiate(skeletonAIPrefab, skeletonSpawnLocations[i].position, Quaternion.identity);
                minion.GetComponent<EnemyAI>().chaseDistance = 20f;
                minion.AddComponent<MinionCounter>();
                minionCounter++;
            }       
        }
        currentWave++;
        summoningMinions = false;
        if(currentWave != 4) ToggleMagicalShield(true);
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
    public override void TakeDamage(int damage) //Take Damage
    {
        if (currentHealth > 0 && isAlive)
        {
            currentHealth -= damage;
            StartCoroutine(enemyTookDamageIndicator());

            if (healthDisplayer != null) StopCoroutine(healthDisplayer); // stops previous co-routine to stop health bar from flickering
            healthDisplayer = UpdateHealthBar();
            StartCoroutine(healthDisplayer);
            if (currentHealth <= 0)
            {
                isAlive = false;
                PlaySFX(enemyDeathSFX);
                SpawnHealingPotions();
                GameManager.myGameManager.necromancerBossDefeated = true;
                Destroy(gameObject);
            }        
        }
    }
}
