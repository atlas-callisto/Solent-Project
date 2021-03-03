using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour, IDamageable
{
    [Header("Stats")]
    [SerializeField] int health = 4;
    [SerializeField] public float moveSpeed = 1f;
    [SerializeField] int damage = 1;    
    [SerializeField] float chaseDistance = 4f;

    protected bool isAlive = true;
    protected float distanceToThePlayer;
    protected bool playerIsOnRightSide;

    private EnemyPatrol myEnemyPatrol;

    protected Player player;
    protected Rigidbody2D myRB;
    protected SpriteRenderer mySpriteRenderer;
    protected Animator myAnimator;

    // Start is called before the first frame update
    protected virtual void Awake()
    {
        myRB = GetComponent<Rigidbody2D>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        myEnemyPatrol = GetComponent<EnemyPatrol>();
        myAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (!isAlive) return;
        ChasePlayer();
    }

    protected virtual void Start()
    {
        player = FindObjectOfType<Player>();
    }

    #region Enemy A.I.
    protected virtual void ChasePlayer()
    {
        Vector3 distanceVect = player.transform.position - transform.position;
        distanceToThePlayer = Vector3.Distance(player.transform.position, transform.position);

        //Vector3.Normalize(distanceVect) Remove or use it later?
        playerIsOnRightSide = distanceVect.x > 0 ? true : false;
        if (distanceToThePlayer <= chaseDistance)
        {
            if (playerIsOnRightSide)
            {
                transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
                myRB.velocity = new Vector2(moveSpeed, myRB.velocity.y);
            }
            if (!playerIsOnRightSide)
            {
                transform.localRotation = Quaternion.Euler(new Vector3(0, 180, 0));
                myRB.velocity = new Vector2(-moveSpeed, myRB.velocity.y);
            }
        }
        else if (myEnemyPatrol != null && myEnemyPatrol.shouldPatrol)
        {
            myEnemyPatrol.Patrol();
        }
    }

    #endregion

    #region Enemy Damage
    private void OnCollisionStay2D(Collision2D collision)
    {
        IDamageable iDamageableObj;
        iDamageableObj = collision.gameObject.GetComponent<IDamageable>();
        if (iDamageableObj != null && collision.gameObject.tag != "Enemy")
        {
            iDamageableObj.TakeDamage(damage);
        }
    }
    public void TakeDamage(int damage) //Take Damage
    {
        if (health > 0 && isAlive)
        {
            StartCoroutine(enemyTookDamageIndicator());
            health -= damage;
            if (health <= 0) isAlive = false;
            SoundManager.mySoundManager.PlaySFX("SlimeDeathSound", 1f);
            Destroy(this.gameObject);
            //Death anim           
        }
    }
    IEnumerator enemyTookDamageIndicator()
    {
        mySpriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        mySpriteRenderer.color = Color.white;
    } 
    #endregion

}
