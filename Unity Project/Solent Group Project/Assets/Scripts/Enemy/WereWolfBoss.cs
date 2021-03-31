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
    private bool isDoingSpecialAttack = false;

    //WereWolfBoss Created. The Boss Patrols between two points, 
    //if the player moves within its boulder attack range, 
    //it screams (Dont have the sound or animations) and summons a 
    //number of boulders above the player
    protected override void Awake()
    {
        base.Awake();
        if (GameManager.myGameManager.werewolfBossDefeated)
        {
            Destroy(this.gameObject);
            return;
        }
    }
    protected override void Start()
    {
        base.Start();
        myCam = Camera.main.gameObject.GetComponent<CameraScript>();
        clawAttackTimer = clawAttackInterval;
        boulderAttackTimer = boulderAttackInterval;
    }
    protected override void Update()
    {
        if (!isAlive) return;
        AdjustHealthBarOrientation();
        WereWolfBossAI();
    }
    private void WereWolfBossAI()
    {
        MeasureDistanceToThePlayer();
        clawAttackTimer += Time.deltaTime;
        boulderAttackTimer += Time.deltaTime;
        if(isDoingSpecialAttack)
        {
            myRB.velocity = new Vector2(0, myRB.velocity.y);
        }
        else if (distanceToThePlayer > boulderAttackRange)
        {
            base.EnemyAIChaseOrPatrol();
            myAnimator.SetBool("IsWalking" , true);
        }
        else if (distanceToThePlayer <= boulderAttackRange && distanceToThePlayer > clawAttackRange && boulderAttackTimer >= boulderAttackInterval) // Player is within boulder attack range but outside melee range
        {
            TurnTowardsPlayer();
            myRB.velocity = new Vector2(0, myRB.velocity.y);
            myAnimator.SetBool("IsWalking", false);
            if (boulderAttackTimer >= boulderAttackInterval)
            {
                isDoingSpecialAttack = true;
                boulderAttackTimer = 0;
                myAnimator.SetTrigger("SpecialAttack");
            }
        }
        else if (distanceToThePlayer > clawAttackRange && boulderAttackTimer < boulderAttackInterval)
        {
            base.EnemyAIChaseOrPatrol();
            myAnimator.SetBool("IsWalking", true);
        }
        else if (distanceToThePlayer <= clawAttackRange)
        {
            TurnTowardsPlayer();
            myRB.velocity = new Vector2(0, myRB.velocity.y);
            myAnimator.SetBool("IsWalking", false);
            if (clawAttackTimer >= clawAttackInterval)
            {
                myAnimator.SetTrigger("BasicAttack");
                clawAttackTimer = 0;
                weapon.SetActive(true);
            }
        }
    }

    private void BoulderAttack() // called through animation events
    {
        isDoingSpecialAttack = false;
        myCam.StartCoroutine(myCam.CameraShake(1f, 0.5f));
        for (int i = 0; i < numberOfRocksToSpawn; i++)
        {
            rockYOffset = UnityEngine.Random.Range(8, 16);
            rockXOffset = UnityEngine.Random.Range(-5, 5); // Spawn boulder with this Offset on top of players x axis.
            Instantiate(fallingRockPrefab, player.transform.position + new Vector3(rockXOffset, rockYOffset), Quaternion.identity);
        }
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
                myAnimator.SetTrigger("Dead");
                PlaySFX(enemyDeathSFX);
                SpawnHealingPotions();
                GameManager.myGameManager.werewolfBossDefeated = true;
                Destroy(gameObject);
            }
        }
    }
}
