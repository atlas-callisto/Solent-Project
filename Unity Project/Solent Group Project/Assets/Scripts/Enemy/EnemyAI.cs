using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour, IDamageable
{
    [Header("Enemy Stats")]
    [SerializeField] int health = 4;
    [SerializeField] public float moveSpeed = 1f;
    [SerializeField] int damage = 1;    
    [SerializeField] protected float chaseDistance = 4f;
    [SerializeField] protected GameObject healthPotions;

    [Header("Chance is in Percentage from 0 to 100")]
    [SerializeField] float chanceToSpawnHealthPotions = 20f; // percentage from 1 to 100

    protected bool isAlive = true;
    protected float distanceToThePlayer;
    protected bool playerIsOnRightSide;
    [SerializeField] protected bool fearDebuff = false; // When player uses scream, enemy gets fear debuff so enemy runs away from player.


    protected EnemyPatrol myEnemyPatrol;

    protected Player player;
    protected Rigidbody2D myRB;
    protected SpriteRenderer mySpriteRenderer;
    protected Animator myAnimator;
    protected Vector3 distanceVect;

    // Start is called before the first frame update
    protected virtual void Awake()
    {
        myRB = GetComponent<Rigidbody2D>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        myEnemyPatrol = GetComponent<EnemyPatrol>();
        myAnimator = GetComponent<Animator>();
        player = FindObjectOfType<Player>();
    }
    protected virtual void Start()
    {
        distanceToThePlayer = Vector3.Distance(player.transform.position, transform.position);        
    }
    protected virtual void Update()
    {
        if (!isAlive) return;
        MeasureDistanceToThePlayer();
        if (fearDebuff) RunAwayFromPlayer();       
        else EnemyAIChaseOrPatrol();        
    }
    
    #region Enemy A.I.
    protected virtual void EnemyAIChaseOrPatrol()
    {        
        if (distanceToThePlayer <= chaseDistance)
        {
            TurnTowardsPlayer();
            if (playerIsOnRightSide)
            {
                myRB.velocity = new Vector2(moveSpeed, myRB.velocity.y);
            }
            if (!playerIsOnRightSide)
            {
                myRB.velocity = new Vector2(-moveSpeed, myRB.velocity.y);
            }
        }
        else if (myEnemyPatrol != null)
        {
            myEnemyPatrol.Patrol();
        }
    }

    protected virtual void MeasureDistanceToThePlayer() // Needs to be called in the update or else the AI won't work, Needs to be called in the inherited objects as well
    {
        distanceToThePlayer = Vector3.Distance(player.transform.position, transform.position);
    }

    protected virtual void TurnTowardsPlayer()
    {
        distanceVect = player.transform.position - transform.position;
        playerIsOnRightSide = distanceVect.x > 0 ? true : false;
        if (playerIsOnRightSide)
        {
            transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }
        if (!playerIsOnRightSide)
        {
            transform.localRotation = Quaternion.Euler(new Vector3(0, 180, 0));
        }
    }

    protected virtual void RunAwayFromPlayer()
    {        
        Vector3 distanceVect = player.transform.position - transform.position;
        playerIsOnRightSide = distanceVect.x > 0 ? true : false;
        if (playerIsOnRightSide)
        {
            transform.localRotation = Quaternion.Euler(new Vector3(0, 180, 0));
            myRB.velocity = new Vector2(-moveSpeed, myRB.velocity.y);
        }
        if (!playerIsOnRightSide)
        {
            transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
            myRB.velocity = new Vector2(moveSpeed, myRB.velocity.y);
        }
    }

    #endregion

    #region Enemy Damage
    virtual protected void OnCollisionStay2D(Collision2D collision) // Enemy damages anything that have hp when it collides
    {
        IDamageable iDamageableObj;
        iDamageableObj = collision.gameObject.GetComponent<IDamageable>();
        if (iDamageableObj != null && collision.gameObject.tag != "Enemy") // Excluding objects with enemy tag of course
        {
            iDamageableObj.TakeDamage(damage);
        }
    }
    virtual public void TakeDamage(int damage) //Take Damage
    {
        if (health > 0 && isAlive)
        {
            StartCoroutine(enemyTookDamageIndicator());
            health -= damage;
            if (health <= 0)
            {
                isAlive = false;
                SoundManager.mySoundManager.PlaySFX("SlimeDeathSound", 1f);
                SpawnHealingPotions();
                Destroy(this.gameObject);
            }
            //Death anim           
        }
    }
    IEnumerator enemyTookDamageIndicator()
    {
        mySpriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        mySpriteRenderer.color = Color.white;
    } 

    private void SpawnHealingPotions()
    {
        float i = Random.Range(0, 100);
        if(i < chanceToSpawnHealthPotions)
        {
            Instantiate(healthPotions, transform.position, Quaternion.identity);
        }        
    }

    public IEnumerator ApplyFearDebuff(float Fearduration) // Player skill apply fear debuff that makes enemy runaway
    {
        fearDebuff = true;
        yield return new WaitForSeconds(Fearduration);
        fearDebuff = false;
    }
    #endregion

}
