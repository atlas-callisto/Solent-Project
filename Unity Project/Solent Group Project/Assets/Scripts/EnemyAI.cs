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

    private bool isAlive = true;
    private float distanceToThePlayer;
    private bool playerIsOnRightSide;

    [Header("Patrol")]
    public GameObject leftPatrolPoint;
    public GameObject rightPatrolPoint;

    private Vector3 pointA = new Vector3(0, 0, 0);
    private Vector3 pointB = new Vector3(0, 0, 0);
    private bool moveTowardspointB = true;    

    private Player player;
    private Rigidbody2D myRB;
    private SpriteRenderer mySpriteRenderer;
        
    // Start is called before the first frame update
    void Awake()
    {
        myRB = GetComponent<Rigidbody2D>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive) return;
        ChasePlayer();
    }

    private void Start()
    {
        player = FindObjectOfType<Player>();
        pointA = leftPatrolPoint.transform.position;
        pointB = rightPatrolPoint.transform.position;
    }

    #region Enemy A.I.
    private void ChasePlayer()
    {
        Vector3 distanceVect = player.transform.position - transform.position;
        distanceToThePlayer = Vector3.Distance(player.transform.position, transform.position);

        //Vector3.Normalize(distanceVect) Remove it later?
        playerIsOnRightSide = distanceVect.x > 0 ? true : false;
        if (distanceToThePlayer <= chaseDistance)
        {
            if (playerIsOnRightSide) myRB.velocity = new Vector2(moveSpeed, myRB.velocity.y);
            if (!playerIsOnRightSide) myRB.velocity = new Vector2(-moveSpeed, myRB.velocity.y);
        }
        else
        {
            EnemyPatrol();
        }
    }

    private void EnemyPatrol()
    {
        if (pointA == null || pointB == null) return;
        if (transform.position.x <= pointA.x) moveTowardspointB = true;
        if (transform.position.x >= pointB.x) moveTowardspointB = false;
        if (moveTowardspointB)
        {
            myRB.velocity = new Vector2(moveSpeed, myRB.velocity.y);
        }
        else
        {
            myRB.velocity = new Vector2(-moveSpeed, myRB.velocity.y);
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
