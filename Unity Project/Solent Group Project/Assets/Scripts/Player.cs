using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    //Params
    [SerializeField] float moveSpeed = 4f;
    [SerializeField] float jumpForce = 5f;
    [SerializeField] float rayCastHeightOffset = 0.1f;
    [SerializeField] int health = 10;

    [SerializeField] public bool wolf = false; //Transform to wolf
    [SerializeField] bool playerAlive = true;

    bool isGrounded = false;
    bool canDoubleJump;
    bool doubleJumpSkill;

    //temp
    float timer = 0.2f; //timer to enable hitbox duration to match attack animation

    //Comp Ref
    Rigidbody2D myRB;
    Animator myAnimator;
    SpriteRenderer mySpriteRenderer;
    BoxCollider2D myBoxCollider2D;
    // PlayerAnim myPlayerAnim;

    //Ref Objs
    public GameObject attackTrigger;    //Attack hitbox child
    public GameObject projectilePrefab; // bullet to spwan during attack 2

    // Start is called before the first frame update
    private void Awake()
    {
        myRB = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBoxCollider2D = GetComponent<BoxCollider2D>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();        
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerAlive) return;
        PlayerMovement();
        PlayerJump();
        PlayerAttack();
        CheckForGrounded();
        Transform();
    }


    private void PlayerMovement()
    {
        float horizontalMov = Input.GetAxisRaw("Horizontal");
        float verticalMov = Input.GetAxisRaw("Vertical"); // Unused game mechanics at the moment.

        bool isWalking = horizontalMov != 0 ? true : false;
        myAnimator.SetBool("IsWalking", isWalking);

        if (horizontalMov > 0)
        {
            transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }
        if (horizontalMov < 0)
        {
            transform.localRotation = Quaternion.Euler(new Vector3(0, 180, 0));
        }
        myRB.velocity = new Vector2(horizontalMov * moveSpeed, myRB.velocity.y);        
    }

    private void PlayerJump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            myRB.velocity = new Vector2(myRB.velocity.x, jumpForce);
        }
        else if (Input.GetButtonDown("Jump") && canDoubleJump)
        {
            myRB.velocity = new Vector2(myRB.velocity.x, jumpForce);
            canDoubleJump = false;
        }
    }

    private void PlayerAttack()
    {
        if (Input.GetButtonDown("Basic Attack"))
        {
            myAnimator.SetTrigger("Attack");
            AttackTrigger(timer);
            //Basic Attack????
        }
        if (Input.GetButtonDown("Heavy Attack"))
        {
            // myAnimator.SetTrigger("Attack");
            // AttackTrigger(timer);
            ShootProjectile();
            //Attack????
        }
        if (Input.GetButtonDown("Special Attack"))
        {
            myAnimator.SetTrigger("Attack");
            AttackTrigger(timer);
            //Attack???
        }
    }

    private void CheckForGrounded()
    {
        RaycastHit2D hitInfo;
        RaycastHit2D hitInfo2;
        Vector3 offset = new Vector3(0.1f, 0, 0); //offset not used atm, but might be used in boxcast to remove hitinfo when colliding on wall's sides

        hitInfo = Physics2D.BoxCast(myBoxCollider2D.bounds.center, myBoxCollider2D.bounds.size, 0f, Vector2.down, rayCastHeightOffset,
            LayerMask.GetMask("Ground"));
        hitInfo2 = Physics2D.BoxCast(myBoxCollider2D.bounds.center, myBoxCollider2D.bounds.size, 0f, Vector2.down,
            rayCastHeightOffset, LayerMask.GetMask("Transparent Platform"));
        // ground layer must be applied to ground tilemap
        //Debug.DrawRay(myBoxCollider2D.bounds.center, Vector2.down);

        if (hitInfo || hitInfo2)
        {
            isGrounded = true;
            canDoubleJump = true;
        }
        else
        {
            isGrounded = false;
        }

    }
    private void Transform()
    {        
        if(Input.GetButtonDown("Transform"))
        {
            wolf = !wolf;
        }
        myAnimator.SetBool("Transform", wolf);
    }

    private void Interact(Collider2D collision)
    {
        Interactable interactableObj = collision.GetComponent<Interactable>();       
        if (interactableObj != null && Input.GetButton("Interact"))
        {
            collision.GetComponent<Interactable>().Interact();
        }
    }

    private void AttackTrigger(float time)
    {
        SoundManager.mySoundManager.PlaySFX("SwordSound" , 0.2f);
        attackTrigger.SetActive(true);
        StartCoroutine(AttackTriggerTimer(time));
    }

    IEnumerator AttackTriggerTimer(float time)
    {
        yield return new WaitForSeconds(time);
        attackTrigger.SetActive(false);

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Interactable")
        {            
            Interact(collision);
        }        
    }

    public void TakeDamage(int damage) //Interface take damage
    {
        if (health > 0 && playerAlive)
        {
            health -= damage;
            StartCoroutine(playerTookDamageIndicator());
            if(health <=0)
            {
                SoundManager.mySoundManager.PlaySFX("PlayerDeathSound", 1f);
                playerAlive = false;
                myAnimator.SetTrigger("Dead");
                FindObjectOfType<LevelLoader>().RestartLevelAfterAPause();
            }
        }
    }
    public void ShootProjectile()
    {
        SoundManager.mySoundManager.PlaySFX("BulletSound", 0.2f);
        GameObject projectile = Instantiate(projectilePrefab, transform.position, transform.localRotation);
    }

    IEnumerator playerTookDamageIndicator()
    {
        mySpriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        mySpriteRenderer.color = Color.white;
    }


}
