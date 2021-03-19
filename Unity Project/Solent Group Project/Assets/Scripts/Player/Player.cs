using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    //Params
    [Header("Stats")]
    [SerializeField] public int maxHealth = 10;
    [SerializeField] public int currentHealth = 10;
    [SerializeField] public float maxWolfBar = 10;
    [SerializeField] public float currentWolfBar = 10;
    [SerializeField] public float wolfBarRegenRate = 0.1f;

    [Header("Human Stats")]
    [SerializeField] float humanMoveSpeed = 4f;
    [SerializeField] float humanJumpForce = 5f;
    [Header("Wolf Stats")]
    [SerializeField] float wolfMoveSpeed = 6f;
    [SerializeField] float wolfJumpForce = 8f;
    [SerializeField] float screamDuration = 2f; // Remove it later... Use the animation for the scream duration


    private float jumpForce = 5f;
    private bool slowDebuff = false;

    //Ref Objs
    [Header("Objects Ref")]
    public GameObject attackTrigger;    //Attack hitbox child
    public GameObject projectilePrefab; // bullet to spwan during attack 2
    public PlayerWeapon playerWep;
    public PlayerWolfWeapon playerWolfWep;
    public GameObject fearDebuffApplier;

    [Header("Cool Downs")]
    [SerializeField] private float basicAttackCoolDown = 2f;
    [SerializeField] private float heavyAttackCoolDown = 5f;
    [SerializeField] private float specialAttakCoolDown = 10f;
    private float basicAttackTimer = 0f;
    private float heavyAttackTimer = 0f;
    private float specialAttackTimer = 0f;
    private float currentMoveSpeed;


    internal bool wolf = false; //Transform to wolf, also called by moonlight script
    private bool playerAlive = true;

    internal bool isGrounded = false; // is modified by playerGroundCheck
    internal bool canDoubleJump = false;// is modified by playerGroundCheck

    internal bool doubleJumpSkillAcquired = false; // enable double jump after skill is unlocked??? for later use
    private bool playerCanTakeDmg = true;
    private float timer = 0.2f; //timer to enable hitbox duration to match attack animation
    private float playerInvunerabletimer = 1.5f; // timer to stop player playing from getting damage after taking a hit

    //Comp Ref
    private Rigidbody2D myRB;
    private Animator myAnimator;
    private SpriteRenderer mySpriteRenderer;
    

    void Start()
    {
        basicAttackTimer = basicAttackCoolDown;
        heavyAttackTimer = heavyAttackCoolDown;
        specialAttackTimer = specialAttakCoolDown;
    }
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
        CoolDownChecker();
        PlayerAttack();
        Transform();
        AdjustTransformStats();
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
        myRB.velocity = new Vector2(horizontalMov * currentMoveSpeed, myRB.velocity.y);
    }
    private void PlayerJump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            myRB.velocity = new Vector2(myRB.velocity.x, jumpForce);
        }
        else if (Input.GetButtonDown("Jump") && canDoubleJump && doubleJumpSkillAcquired)
        {
            myRB.velocity = new Vector2(myRB.velocity.x, jumpForce);
            canDoubleJump = false;
        }
    }
    #endregion

    #region
    private void Transform() // Wolf Transformation
    {
        if (Input.GetButtonDown("Transform"))
        {
            wolf = !wolf;
        }
        myAnimator.SetBool("Transform", wolf);
    }

    private void AdjustTransformStats()
    {
        if(wolf)
        {
            jumpForce = wolfJumpForce;
            if(!slowDebuff) currentMoveSpeed = wolfMoveSpeed;
        }
        else if(!wolf)
        {
            jumpForce = humanJumpForce;
            if (!slowDebuff) currentMoveSpeed = humanMoveSpeed;
        }
    }



    #endregion

    #region Player Attacks
    private void PlayerAttack()
    {
        if(!wolf)
        {
            if (Input.GetButtonDown("Basic Attack"))  // 1 = Basic attack, 2 = heavy attack, 3 = special attack
            {
                if(basicAttackTimer >= basicAttackCoolDown)
                {
                    myAnimator.SetTrigger("Attack");
                    AttackTrigger(timer);

                    AttackWithType(1);// 1 = Basic attack, sending the type of attack it is supposed to be
                }
                
            }
            if (Input.GetButtonDown("Heavy Attack"))
            {
                if (heavyAttackTimer >= heavyAttackCoolDown)
                {
                    myAnimator.SetTrigger("Attack");
                    AttackTrigger(timer);

                    AttackWithType(2); // 2 = heavy attack, Sending the type of attack it is supposed to be
                }
                
            }
            if (Input.GetButtonDown("Special Attack"))
            {
                if (specialAttackTimer >= specialAttakCoolDown)
                {
                    //shooting animation                    
                    ShootProjectile();

                    specialAttackTimer = 0;
                }
            }
        }
        else // If player has transformed to wolf
        {
            if (Input.GetButtonDown("Basic Attack"))
            {
                if (basicAttackTimer >= basicAttackCoolDown)
                {
                    playerWolfWep.gameObject.SetActive(true);
                    playerWolfWep.SetDamage(1);
                    playerWolfWep.Attack(1);
                }
            }
            if (Input.GetButtonDown("Heavy Attack"))
            {
                if (heavyAttackTimer >= heavyAttackCoolDown)
                {
                    playerWolfWep.gameObject.SetActive(true);
                    playerWolfWep.SetDamage(2);
                    playerWolfWep.Attack(2);
                }
            }
            if (Input.GetButtonDown("Special Attack"))
            {
                if (specialAttackTimer >= specialAttakCoolDown)
                {
                    // fearDebuffApplier.SetActive(true); // Later On I won't need Coroutine
                    StartCoroutine(UseWolfSpecialAttack());
                }
            }
        }       
    }
    public void FinishedScreaming() // Just like me after doing the code for this attack // Will be called through Animation Event
    {
        fearDebuffApplier.SetActive(false);
    }
    private IEnumerator UseWolfSpecialAttack()
    {
        fearDebuffApplier.SetActive(true);
        yield return new WaitForSeconds(screamDuration);
        fearDebuffApplier.SetActive(false);
    }
    private void AttackWithType(int attackType)
    {
        playerWep.gameObject.SetActive(true);
        playerWep.SetDamage(attackType); // 2 = heavy attack, Sending the type of attack it is supposed to be
        playerWep.Attack(attackType);
        switch (attackType) //Sets the attack timer to 0 depending on the type of attack used
        {
            case 1:
                basicAttackTimer = 0;
                break;

            case 2:
                heavyAttackTimer = 0;
                break;

            case 3:
                specialAttackTimer = 0;
                break;
                
            default:
                break;
        }
    }
    private void AttackTrigger(float time) // Enables Attack trigger, Will be adjusted later
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

    private void CoolDownChecker()
    {
        basicAttackTimer  = Mathf.Clamp(basicAttackTimer + Time.deltaTime ,0,basicAttackCoolDown);//Clamping values for precise UI display
        heavyAttackTimer = Mathf.Clamp(heavyAttackTimer + Time.deltaTime, 0, heavyAttackCoolDown);
        specialAttackTimer = Mathf.Clamp(specialAttackTimer + Time.deltaTime, 0, specialAttakCoolDown);
        if(wolf) //Player is in wolf form, decreases wolf bar in wolf form
        {
            currentWolfBar = Mathf.Clamp(currentWolfBar -= Time.deltaTime, 0 , maxWolfBar);
        }
        else // Slowly Regenerate wolf bar when not in wolf form
        {
            currentWolfBar = Mathf.Clamp(currentWolfBar += Time.deltaTime * wolfBarRegenRate, 0, maxWolfBar);
        }
        if(currentWolfBar == 0) // Automatically transform into human when wolf bar reaches 0
        {
            wolf = false;
        }
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
        }
    }

    #region Player Take Damage
    public void TakeDamage(int damage) //Interface take damage
    {
        if (currentHealth > 0 && playerCanTakeDmg && playerAlive)
        {
            currentHealth -= damage;
            KnockBackEffect();
            StartCoroutine(playerTookDamageIndicator());
            StartCoroutine(playerInvunerableDuration());
            if (currentHealth <= 0)
            {
                myRB.velocity = new Vector2(0, myRB.velocity.y); // Stops player corpse from sliding
                SoundManager.mySoundManager.PlaySFX("PlayerDeathSound", 1f);
                playerAlive = false;
                myAnimator.SetTrigger("Dead");
                
                // Needs fixing - keeps not assigning reference
                //FindObjectOfType<LevelLoader>().RestartLevelAfterAPause();
            }
        }
    }
    private void KnockBackEffect()
    {
        myRB.AddForce(new Vector2(100f, 100f));
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

    public void SlowMoveSpeedDebuff(float SlowPercentage, float slowDuration)
    {
        StartCoroutine(SlowMoveSpeed(SlowPercentage, slowDuration));
    }
    private IEnumerator SlowMoveSpeed(float SlowPercentage, float slowDuration)
    {
        slowDebuff = true;
        currentMoveSpeed = currentMoveSpeed * (SlowPercentage/100);
        yield return new WaitForSeconds(slowDuration);
        slowDebuff = false;
    }


}
