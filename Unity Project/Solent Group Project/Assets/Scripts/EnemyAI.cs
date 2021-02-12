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


    [SerializeField] IDamageable otherObj;
    SpriteRenderer mySpriteRenderer;

    [SerializeField] bool isAlive = true;

    [SerializeField] float chaseDistance = 4f;
    [SerializeField] float distanceToThePlayer;
    [SerializeField] bool playerIsOnRightSide;

    [SerializeField] GameObject player;
    Rigidbody2D myRB;


    // Start is called before the first frame update
    void Awake()
    {
        myRB = GetComponent<Rigidbody2D>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive) Destroy(this.gameObject);
        ChasePlayer();
    }

    private void ChasePlayer()
    {
        Vector3 distanceVect = player.transform.position - transform.position;
        distanceToThePlayer = Mathf.Abs(distanceVect.x);

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

    public void Damage(int damage)
    {
        if(health > 0 && isAlive)
        {
            StartCoroutine(enemyTookDamageIndicator());
            health -= damage;
            if (health <= 0) isAlive = false;
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
                Debug.Log(timer);
                if (timer >= damageTickRate)
                {
                    Debug.Log("Acid Damage called");
                    timer = 0;
                    iDamageableObj.Damage(damage);
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
