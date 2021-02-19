using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    //Params
    [Header("Stats")]
    [SerializeField] float moveSpeed = 4f;
    [SerializeField] float jumpForce = 5f;
    [SerializeField] int health = 10;

    //Ref Objs
    [Header("Objects Ref")]
    public GameObject attackTrigger;    //Attack hitbox child
    public GameObject projectilePrefab; // bullet to spwan during attack 2

    internal bool wolf = false; //Transform to wolf, also called by moonlight script
    private bool playerAlive = true;

    internal bool isGrounded = false; // is modified by playerGroundCheck
    internal bool canDoubleJump = false;// is modified by playerGroundCheck

    private bool doubleJumpSkill; // enable double jump after skill is unlocked??? for later use
    private bool playerCanTakeDmg = true;
    private float timer = 0.2f; //timer to enable hitbox duration to match attack animation
    private float playerInvunerabletimer = 0.5f; // timer to stop player playing from getting damage after taking a hit

    //Comp Ref
    private Rigidbody2D myRB;
    private Animator myAnimator;
    private SpriteRenderer mySpriteRenderer;


    private void Awake()
    {
        myRB = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();        
    }
    void Update()
    {
        if (!playerAlive) return;
        PlayerMovement();
        PlayerJump();
        PlayerAttack();
        Transform();
    }

    #region Player Movement
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
    #endregion
    private void Transform() // Wolf Transformation
    {
        if (Input.GetButtonDown("Transform"))
        {
            wolf = !wolf;
        }
        myAnimator.SetBool("Transform", wolf);
    }

    #region Player Attacks
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

    private void AttackTrigger(float time) // Enables Attack trigger 
    {
        SoundManager.mySoundManager.PlaySFX("SwordSound", 0.2f);
        attackTrigger.SetActive(true);
        StartCoroutine(AttackTriggerTimer(time));
    }

    IEnumerator AttackTriggerTimer(float time) //Sets Attack timer or time between attack
    {
        yield return new WaitForSeconds(time);
        attackTrigger.SetActive(false);
    }

    public void ShootProjectile()
    {
        SoundManager.mySoundManager.PlaySFX("BulletSound", 0.2f);
        GameObject projectile = Instantiate(projectilePrefab, transform.position, transform.localRotation);
    }     
    #endregion

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Interactable")
        {
            Interact(collision);
        }        
    }
    private void Interact(Collider2D collision)
    {
        Interactable interactableObj = collision.GetComponent<Interactable>();
        if (interactableObj != null && Input.GetButtonDown("Interact"))
        {
            collision.GetComponent<Interactable>().Interact();
            Debug.Log("helloo1111");
        }
    }

    #region Player Take Damage
    public void TakeDamage(int damage) //Interface take damage
    {
        if (health > 0 && playerCanTakeDmg && playerAlive)
        {
            health -= damage;
            StartCoroutine(playerTookDamageIndicator());
            StartCoroutine(playerInvunerableDuration());
            if (health <= 0)
            {
                SoundManager.mySoundManager.PlaySFX("PlayerDeathSound", 1f);
                playerAlive = false;
                myAnimator.SetTrigger("Dead");
                FindObjectOfType<LevelLoader>().RestartLevelAfterAPause();
            }
        }
    }
    IEnumerator playerTookDamageIndicator()
    {
        mySpriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        mySpriteRenderer.color = Color.white;
    }
    IEnumerator playerInvunerableDuration()
    {        
        playerCanTakeDmg = false;
        yield return new WaitForSeconds(playerInvunerabletimer);
        playerCanTakeDmg = true;
    }
    #endregion

}
