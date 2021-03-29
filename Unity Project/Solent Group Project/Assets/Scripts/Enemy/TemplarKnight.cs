using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemplarKnight : EnemyAI
{


    [Header("Templar Knight Attack Stats")]
    [SerializeField] private float chargeRange;
    [SerializeField] private float shieldAttackRange;
    [SerializeField] private float shieldAttackInterval;
    [SerializeField] private float chargeAttackMoveSpeed;
    [Tooltip("The delay before the knight charges")]
    [SerializeField] private float preChargeDuration;
    [Tooltip("The duration till which the knight charges forward")]
    [SerializeField] private float chargeDuration;
    [Tooltip("The delay before the knight waits before turning towards the player")] 
    [SerializeField] private float afterChargeWaitDuration;

    [Header("Templar Knight Objects Ref")]
    [SerializeField] private GameObject spear;
    [SerializeField] private GameObject shield;

    [Header("Templar Knight SFX")]
    [SerializeField] private AudioClip chargeAttackSFX;
    [SerializeField] private AudioClip shieldAttackSFX;
    public bool chargingTowardsPlayer = false;    
    private float shieldAttackTimer;
    private IEnumerator attackEnemy;

    protected override void Start()
    {
        base.Start();
        shieldAttackTimer = shieldAttackInterval;
    }
    protected override void Update()
    {
        if (!base.isAlive) return;
        AdjustHealthBarOrientation();
        if (fearDebuff)
        {
            RunAwayFromPlayer();
            return;
        }
        TemplarKnightAI();
    }
    private void TemplarKnightAI()
    {

        MeasureDistanceToThePlayer();
        shieldAttackTimer += Time.deltaTime;
        
        if (distanceToThePlayer > chaseDistance && !chargingTowardsPlayer)
        {
            base.EnemyAIChaseOrPatrol();
        }
        else if (distanceToThePlayer <= chargeRange && distanceToThePlayer > shieldAttackRange && !chargingTowardsPlayer)
        {
            attackEnemy = ChargeTowardsPlayer();
            StartCoroutine(attackEnemy); 
        }
        else if (!chargingTowardsPlayer && distanceToThePlayer <= shieldAttackRange)
        {
            base.EnemyAIChaseOrPatrol();
            if ( shieldAttackTimer >= shieldAttackInterval) ShieldBash();
        }
    }
    private IEnumerator ChargeTowardsPlayer() 
    {
        PlaySFX(chargeAttackSFX);
        chargingTowardsPlayer = true;
        myRB.velocity = new Vector2(0, myRB.velocity.y);
        TurnTowardsPlayer();
        spear.SetActive(true);

        yield return new WaitForSeconds(preChargeDuration);
        if (playerIsOnRightSide) myRB.velocity = new Vector2(chargeAttackMoveSpeed, myRB.velocity.y);
        else if (!playerIsOnRightSide) myRB.velocity = new Vector2(-chargeAttackMoveSpeed, myRB.velocity.y);

        yield return new WaitForSeconds(chargeDuration);
        myRB.velocity = new Vector2(0, myRB.velocity.y);

        yield return new WaitForSeconds(afterChargeWaitDuration);
        chargingTowardsPlayer = false;
        spear.SetActive(false);
    }
    private void ShieldBash()
    {
        PlaySFX(shieldAttackSFX);
        shieldAttackTimer = 0;
        shield.SetActive(true);
    }
    public void StopChargingTowardsPlayer()
    {
        StopCoroutine(attackEnemy);
        chargingTowardsPlayer = false;
        myRB.velocity = new Vector2(0, myRB.velocity.y);        
    }
    protected override void OnCollisionStay2D(Collision2D collision)
    {
        IDamageable iDamageableObj;
        iDamageableObj = collision.gameObject.GetComponent<IDamageable>();
        if (iDamageableObj != null && collision.gameObject.tag != "Enemy")
        {
            iDamageableObj.TakeDamage(collisionDamage);
            if (collision.gameObject.GetComponent<Player>())
            {
                player.KnockBackEffect(GetKnockBackDirection(collision));
            }
        }
    }

    public override void TakeDamage(int damage)
    {
        Vector3 distanceVect = player.transform.position - transform.position;
        playerIsOnRightSide = distanceVect.x > 0 ? true : false;
        // Halves the damage that the enemy takes if the player 
        if ((playerIsOnRightSide && transform.rotation.y == 0) || (!playerIsOnRightSide && transform.rotation.y != 0))damage = damage / 2;
        base.TakeDamage(damage);        
    }
}
