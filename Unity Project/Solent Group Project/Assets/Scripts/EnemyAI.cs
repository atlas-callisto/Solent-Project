using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour, IDamageable
{
    [SerializeField] int health = 4;
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] int damage = 1;    
    [SerializeField] float damageTickRate = 3f;
    [SerializeField] float timer = 0;
        
    [SerializeField] bool isAlive = true;
    [SerializeField] float chaseDistance = 4f;
    [SerializeField] float distanceToThePlayer;
    [SerializeField] bool playerIsOnRightSide;

    [SerializeField] GameObject player;
    [SerializeField] IDamageable otherObj;

    Rigidbody2D myRB;
    SpriteRenderer mySpriteRenderer;


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

    private void ChasePlayer()
    {
        Vector3 distanceVect = player.transform.position - transform.position;
        distanceToThePlayer = Vector3.Distance(player.transform.position, transform.position);

        //Vector3.Normalize(distanceVect) Remove it later;
        playerIsOnRightSide = distanceVect.x > 0 ? true : false;
        moveSpeed = playerIsOnRightSide ? Mathf.Abs(moveSpeed) : moveSpeed = - Mathf.Abs(moveSpeed);
        if (distanceToThePlayer <= chaseDistance)
        {
            myRB.velocity = new Vector2(moveSpeed, myRB.velocity.y);
        }
        else
        {
            myRB.velocity = new Vector2(0, myRB.velocity.y);
        }
    }

    public void TakeDamage(int damage) //Take Damage
    {
        if(health > 0 && isAlive)
        {
            StartCoroutine(enemyTookDamageIndicator());
            health -= damage;
            if (health <= 0) isAlive = false;
            SoundManager.mySoundManager.PlaySFX("SlimeDeathSound", 1f);
            Destroy(this.gameObject);
            //Death anim           
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        IDamageable iDamageableObj;
        iDamageableObj = collision.gameObject.GetComponent<IDamageable>();
        if (iDamageableObj != null)
        {
            if (timer < damageTickRate)
            {
                timer += Time.deltaTime;
                if (timer >= damageTickRate)
                {
                    timer = 0;
                    iDamageableObj.TakeDamage(damage);
                }
            }
        }       
    }

    IEnumerator enemyTookDamageIndicator()
    {
        mySpriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        mySpriteRenderer.color = Color.white;
    }
}
