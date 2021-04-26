using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyAI : MonoBehaviour, IDamageable
{
    [Header("Enemy Stats")]
    [SerializeField] protected int maxHealth = 4;
    [SerializeField] protected int currentHealth = 4;
    [SerializeField] public float moveSpeed = 1f;
    [SerializeField] [Tooltip("The damage the enemy does to the player when touching the player")]
    protected int collisionDamage = 1;
    [SerializeField] [Tooltip("For this duration the enemy will remain in the scene after dying inorder to play the death animation")] 
    protected float deathAnimDuration;

    [SerializeField] public float chaseDistance = 4f;
    [SerializeField] protected GameObject healthPotion;
    [SerializeField] protected GameObject manaPotion;

    [Header("Chance is in Percentage from 0 to 100")]
    [SerializeField] float chanceToSpawnPotions = 20f; // percentage from 1 to 100

    [Header("SFX Lists")]
    [SerializeField] protected AudioClip enemyDeathSFX;


    [SerializeField] private GameObject heatlhBarGameObject;
    [SerializeField] protected Slider healthBar; 
    

    protected bool isAlive = true;
    protected float distanceToThePlayer;
    protected bool playerIsOnRightSide;
    internal bool fearDebuff = false; // When player uses scream, enemy gets fear debuff so enemy runs away from player.
    
    protected EnemyPatrol myEnemyPatrol;

    protected Player player;
    protected Rigidbody2D myRB;
    protected SpriteRenderer mySpriteRenderer;
    protected Animator myAnimator;
    protected Vector3 distanceVect;
    protected IEnumerator healthDisplayer;

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
        AdjustHealthBarOrientation();
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
    virtual protected void OnCollisionStay2D(Collision2D collision)
    {
        IDamageable iDamageableObj;
        iDamageableObj = collision.gameObject.GetComponent<IDamageable>();
        if (iDamageableObj != null && collision.gameObject.tag != "Enemy")
        {
            iDamageableObj.TakeDamage(collisionDamage);
            if (collision.gameObject.tag == "Player")
            {
                player.KnockBackEffect(GetKnockBackDirection(collision));
            }
        }
    }

    virtual protected Vector2 GetKnockBackDirection(Collision2D collision)
    {
        if (collision.gameObject.transform.position.x > this.transform.position.x) return Vector2.right;
        else return Vector2.left;
    }
    virtual public void TakeDamage(int damage) //Take Damage
    {
        if (currentHealth > 0 && isAlive)
        {
            currentHealth -= damage;
            StartCoroutine(enemyTookDamageIndicator());
            
            if(healthDisplayer != null) StopCoroutine(healthDisplayer); // stops previous co-routine to stop health bar from flickering
            healthDisplayer = UpdateHealthBar();
            StartCoroutine(healthDisplayer);
            if (currentHealth <= 0)
            {
                EnemyDied();
                //isAlive = false;
                //PlaySFX(enemyDeathSFX);
                //SpawnHealingOrManaPotions();
                //Destroy(this.gameObject);
            }
            //Death anim           
        }
    }

    protected IEnumerator enemyTookDamageIndicator()
    {
        mySpriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        mySpriteRenderer.color = Color.white;
    }
    virtual protected void AdjustHealthBarOrientation()
    {
        heatlhBarGameObject.transform.rotation = Quaternion.Euler(new Vector2(0, 0));
    }
    protected IEnumerator UpdateHealthBar()
    {        
        heatlhBarGameObject.SetActive(true);
        healthBar.value = (float)currentHealth / (float)maxHealth;

        yield return new WaitForSeconds(3f); // Magic number, It's Magic

        heatlhBarGameObject.SetActive(false);
    }
    virtual protected void SpawnHealingOrManaPotions()
    {
        float i = Random.Range(0, 100);
        if(i < chanceToSpawnPotions)
        {
            float j = Random.Range(0, 100);
            if(j >=50) Instantiate(healthPotion, transform.position, Quaternion.identity);
            else Instantiate(manaPotion, transform.position, Quaternion.identity);
        }        
    }

    public IEnumerator ApplyFearDebuff(float Fearduration) // Player skill apply fear debuff that makes enemy runaway
    {
        fearDebuff = true;
        yield return new WaitForSeconds(Fearduration);
        fearDebuff = false;
    }
    #endregion
    protected void PlaySFX(AudioClip clipName)
    {
        var sfx = new GameObject();
        sfx.AddComponent<AudioSource>();
        sfx.GetComponent<AudioSource>().PlayOneShot(clipName, GameManager.myGameManager.GetSFXVolume());
        Destroy(sfx, clipName.length);
    }

    virtual protected void EnemyDied()
    {
        isAlive = false;
        myAnimator.SetTrigger("Dead");
        PlaySFX(enemyDeathSFX);
        SpawnHealingOrManaPotions();
        gameObject.layer = 15; // The player won't collide with the enemy after the enemy dies by switching layer
        myRB.velocity = new Vector2(0, myRB.velocity.y); // Stops enemy from sliding after dying
        Destroy(gameObject, deathAnimDuration);
    }
}
