using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemplarKnight : EnemyAI
{


    [Header("Templar Knight Attack Stats")]
    [SerializeField] private float chargeRange;
    [SerializeField] private float shieldAttackRange;
    [SerializeField] private float shieldAttackInterval;
    [SerializeField] private float chargeSpeed;
    [Tooltip("The delay before the knight charges")]
    [SerializeField] private float preChargeDuration;
    [SerializeField] private float chargeDuration;

    [Header("Templar Knight Objects Ref")]
    [SerializeField] private GameObject spear;
    [SerializeField] private GameObject shield;

    [Header("Templar Knight SFX")]
    [SerializeField] private AudioClip chargeAttackSFX;
    [SerializeField] private AudioClip shieldAttackSFX;
    private bool chargingTowardsPlayer = false;    
    private float shieldAttackTimer;

    protected override void Start()
    {
        base.Start();
        shieldAttackTimer = shieldAttackInterval;
    }
    protected override void Update()
    {
        if (!base.isAlive) return;
        LordProtectorAI();
    }
    private void LordProtectorAI()
    {
        MeasureDistanceToThePlayer();
        shieldAttackTimer += Time.deltaTime;

        if (distanceToThePlayer > chaseDistance)
        {
            base.EnemyAIChaseOrPatrol();
        }
        else if (distanceToThePlayer <= chargeRange && distanceToThePlayer > shieldAttackRange)
        {
            if(!chargingTowardsPlayer)
            {
                StartCoroutine(ChargeTowardsPlayer());
            }
        }
        else if (!chargingTowardsPlayer && distanceToThePlayer <= shieldAttackRange)
        {
            if ( shieldAttackTimer >= shieldAttackInterval) ShieldBash();
        }
    }
    private IEnumerator ChargeTowardsPlayer()
    {
        chargingTowardsPlayer = true;
        spear.SetActive(true);
        myRB.velocity = new Vector2(0, myRB.velocity.y);
        yield return new WaitForSeconds(preChargeDuration);
        TurnTowardsPlayer();
        myRB.velocity = new Vector2(chargeSpeed, myRB.velocity.y);
        yield return new WaitForSeconds(chargeDuration);
        chargingTowardsPlayer = false;
        spear.SetActive(false);

    }
    private void ShieldBash()
    {
        shieldAttackTimer = 0;
        //myAnimator.SetTrigger("Attack");
        shield.SetActive(true);
    }

    protected override void OnCollisionStay2D(Collision2D collision)
    {
        IDamageable iDamageableObj;
        iDamageableObj = collision.gameObject.GetComponent<IDamageable>();
        if (iDamageableObj != null && collision.gameObject.tag != "Enemy") // Excluding objects with enemy tag of course
        {
            iDamageableObj.TakeDamage(collisionDamage);
            if (collision.gameObject.GetComponent<Player>())
            {
                collision.gameObject.GetComponent<Player>().KnockBackEffect(collision.gameObject.transform.position - this.transform.position);
            }
        }
    }
}
