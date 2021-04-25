using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LordProtector : EnemyAI
{
    [Header("Lord Protector Attack Stats")]
    [SerializeField] private float swordAttackRange;
    [SerializeField] private float swordAttackInterval;
    [SerializeField] private float crossbowAttackRange;
    [SerializeField] private float crossbowAttackInterval = 4;
    [SerializeField] private float groundSlamAttackRange;
    [SerializeField] private float groundSlamAttackInterval;

    [Header("Lord Protector Objects Ref")]
    [SerializeField] private GameObject sword;
    [SerializeField] private GameObject crossbow;
    [SerializeField] private Transform shockWaveSpawnLocation;
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private GameObject shockWavePrefab;

    [Header("Lord Protector SFX")]
    [SerializeField] private AudioClip shootingCrossbowSFX;
    [SerializeField] private AudioClip groundSlamSFX;

    [Header("Lord Protector Summoning")]
    [Tooltip("Bats are summonned when lord protector uses groundSlam Attack")]
    [SerializeField] private GameObject batAIPrefab;
    [SerializeField] List<Transform> batSpawnLocations = new List<Transform>();

    private CameraScript myCam;
    private float swordAttackTimer;
    private float crossbowAttackTimer;
    private float groundSlamAttackTimer;
    private int numOfBatsToSpawn = 3;

    protected override void Awake()
    {
        base.Awake();
        if(GameManager.myGameManager.lordProtectorBossDefeated)
        {
            Destroy(this.gameObject);
            return;
        }
    }
    protected override void Start()
    {
        base.Start();
        myCam = Camera.main.gameObject.GetComponent<CameraScript>();
        swordAttackTimer = swordAttackInterval;
        crossbowAttackTimer = crossbowAttackInterval;
        groundSlamAttackTimer = groundSlamAttackInterval;
    }
    protected override void Update()
    {
        if (!isAlive) return;
        AdjustHealthBarOrientation();
        LordProtectorAI();
    }
    private void LordProtectorAI()
    {
        MeasureDistanceToThePlayer();
        swordAttackTimer += Time.deltaTime;
        crossbowAttackTimer += Time.deltaTime;
        groundSlamAttackTimer += Time.deltaTime;
        if (distanceToThePlayer > swordAttackRange && crossbowAttackTimer >= crossbowAttackInterval)
        {
            base.EnemyAIChaseOrPatrol();
        }
        else if (distanceToThePlayer <= crossbowAttackRange && distanceToThePlayer > swordAttackRange 
            && crossbowAttackTimer >= crossbowAttackInterval && (float)currentHealth < (float)maxHealth/2 )
        {
            TurnTowardsPlayer();
            myRB.velocity = new Vector2(0, myRB.velocity.y);
            if (crossbowAttackTimer >= crossbowAttackInterval)
            {
                ShootCrossbow();
            }
        }
        else if (distanceToThePlayer <= swordAttackRange)
        {
            TurnTowardsPlayer();
            myRB.velocity = new Vector2(0, myRB.velocity.y);
            if (swordAttackTimer >= swordAttackInterval) SwordAttack();           
        }
        if(distanceToThePlayer <= groundSlamAttackRange && groundSlamAttackTimer > groundSlamAttackInterval
            && (float)currentHealth < (float)maxHealth / 2)
        {
            ShockWaveAttack();
            groundSlamAttackTimer = 0;
        }
    }
    private void SwordAttack()
    {
        swordAttackTimer = 0;
        myAnimator.SetTrigger("Attack");
        sword.SetActive(true);
    }
    private void ShootCrossbow()
    {
        crossbowAttackTimer = 0;
        PlaySFX(shootingCrossbowSFX);
        crossbow.SetActive(true);
        GameObject projectile = Instantiate(arrowPrefab, transform.position, transform.localRotation);
        crossbowAttackInterval = UnityEngine.Random.Range(4, 9);
    }
    private void ShockWaveAttack()
    {
        myCam.StartCoroutine(myCam.CameraShake(1f, 0.5f));
        PlaySFX(groundSlamSFX);
        GameObject shockWave1 = Instantiate(shockWavePrefab, shockWaveSpawnLocation.position, Quaternion.Euler(new Vector3(0, 0, 0)));
        GameObject shockWave2 = Instantiate(shockWavePrefab, shockWaveSpawnLocation.position, Quaternion.Euler(new Vector3(0, 180, 0)));

        for (int i = 0; i < numOfBatsToSpawn; i++)
        {
            GameObject minion = Instantiate(batAIPrefab, batSpawnLocations[i].position, Quaternion.identity);
            minion.GetComponent<EnemyAI>().chaseDistance = 20f;
        }
    }
    public override void TakeDamage(int damage)
    {
        if (currentHealth > 0 && isAlive)
        {
            StartCoroutine(tookDamageIndicator());
            if (healthDisplayer != null) StopCoroutine(healthDisplayer); // stops previous co-routine to stop health bar from flickering
            healthDisplayer = UpdateHealthBar();
            StartCoroutine(healthDisplayer);
            currentHealth -= damage;
            if (currentHealth <= 0)
            {
                isAlive = false;
                PlaySFX(enemyDeathSFX);
                SpawnHealingOrManaPotions();
                GameManager.myGameManager.lordProtectorBossDefeated = true;
                Destroy(this.gameObject);
            }
            //Death anim               
        }
    }
    private IEnumerator tookDamageIndicator()
    {
        mySpriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        mySpriteRenderer.color = new Color(1f, 0f, 0.9f);
    }
}
