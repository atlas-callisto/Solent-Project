using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour, IDamageable
{
    //Params
    [Header("Stats")]
    [SerializeField] public int maxHealth = 10;
    [SerializeField] public int currentHealth = 10;
    [SerializeField] public float maxWolfBar = 10;
    [SerializeField] public float currentWolfBar = 10;
    [SerializeField] public float wolfBarRegenRate = 0.1f;
    [SerializeField] public float wolfDegeneRate = 1f;

    [Header("Cool Downs")]
    [SerializeField] private float basicAttackCoolDown = 2f;
    [SerializeField] private float heavyAttackCoolDown = 5f;
    [SerializeField] private float specialAttakCoolDown = 10f;

    [Header("Human Stats")]
    [SerializeField] float humanMoveSpeed = 4f;
    [SerializeField] float humanJumpForce = 5f;

    [Header("Wolf Stats")]
    [SerializeField] float wolfMoveSpeed = 6f;
    [SerializeField] float wolfJumpForce = 8f;
    [Tooltip("Duration of the Wolf Special Attack")] [SerializeField] float screamDuration = 2f; // Remove it later... Use the animation for the scream duration
       
    private float jumpForce = 5f;
    private bool slowDebuff = false;

    //Ref Objs
    [Header("Audio SFX")]
    public AudioClip attackSFX;
    public AudioClip deathSFX;
    public AudioClip takingDamageSFX;
    public AudioClip shootingSFX;

    [Header("Objects Ref")]
    public GameObject projectilePrefab; // bullet to spwan during attack 2
    public PlayerWeapon playerWep;
    public PlayerWolfWeapon playerWolfWep;
    public GameObject fearDebuffApplier;

    //Params, Internal
    private float basicAttackTimer = 0f;
    private float heavyAttackTimer = 0f;
    private float specialAttackTimer = 0f;
    private float currentMoveSpeed;
    public float playerRespawnDelay;


    internal bool wolf = false; //Transform to wolf, also called by moonlight script
    public bool playerAlive = true;

    internal bool isGrounded = false; // is modified by playerGroundCheck
    internal bool canDoubleJump = false;// is modified by playerGroundCheck

    internal bool doubleJumpSkillAcquired = false; // enable double jump after skill is unlocked??? for later use
    private bool playerCanTakeDmg = true;
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
        if (!playerAlive)
        {
            StartCoroutine(Die());            
            return;
        }

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

    #region Wolf Transformation
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
                    PlaySFX(attackSFX);
                    AttackWithType(1);// 1 = Basic attack, sending the type of attack it is supposed to be
                }                
            }
            if (Input.GetButtonDown("Heavy Attack"))
            {
                if (heavyAttackTimer >= heavyAttackCoolDown)
                {
                    myAnimator.SetTrigger("Attack");
                    PlaySFX(attackSFX);
                    AttackWithType(2); // 2 = heavy attack, Sending the type of attack it is supposed to be
                }                
            }
            if (Input.GetButtonDown("Special Attack"))
            {
                if (specialAttackTimer >= specialAttakCoolDown)
                {
                    //shooting animation
                    PlaySFX(shootingSFX);
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
        }
    }

    private void CoolDownChecker()
    {
        basicAttackTimer  = Mathf.Clamp(basicAttackTimer + Time.deltaTime ,0,basicAttackCoolDown);//Clamping values for precise UI display
        heavyAttackTimer = Mathf.Clamp(heavyAttackTimer + Time.deltaTime, 0, heavyAttackCoolDown);
        specialAttackTimer = Mathf.Clamp(specialAttackTimer + Time.deltaTime, 0, specialAttakCoolDown);
        if(wolf) //Player is in wolf form, decreases wolf bar in wolf form
        {
            currentWolfBar = Mathf.Clamp(currentWolfBar -= ( Time.deltaTime * wolfDegeneRate), 0 , maxWolfBar);
        }
        else // Slowly Regenerate wolf bar when not in wolf form
        {
            currentWolfBar = Mathf.Clamp(currentWolfBar += (Time.deltaTime * wolfBarRegenRate), 0, maxWolfBar);
        }
        if(currentWolfBar == 0) // Automatically transform into human when wolf bar reaches 0
        {
            wolf = false;
        }
    }

    public void ShootProjectile()
    {
        PlaySFX(shootingSFX);
        GameObject projectile = Instantiate(projectilePrefab, transform.position, transform.localRotation);
    }
    #endregion

    #region Interact System
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Interactable")
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
    #endregion

    #region Player Take Damage
    public void TakeDamage(int damage) //Interface take damage
    {
        if (currentHealth > 0 && playerCanTakeDmg && playerAlive)
        {
            currentHealth -= damage;
            StartCoroutine(playerTookDamageIndicator());
            StartCoroutine(playerInvunerableDuration());
            if (currentHealth <= 0)
            {
                myRB.velocity = new Vector2(0, myRB.velocity.y); // Stops player corpse from sliding
                PlaySFX(deathSFX);                
                playerAlive = false;
                myAnimator.SetTrigger("Dead");                
                // Needs fixing - keeps not assigning reference
                //FindObjectOfType<LevelLoader>().RestartLevelAfterAPause();
            }
        }
    }
    public void KnockBackEffect(Vector2 direction)
    {
        myRB.AddForce(100f * direction);
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

    #region Debuff
    public void SlowMoveSpeedDebuff(float SlowPercentage, float slowDuration)
    {
        StartCoroutine(SlowMoveSpeed(SlowPercentage, slowDuration));
    }
    private IEnumerator SlowMoveSpeed(float SlowPercentage, float slowDuration)
    {
        slowDebuff = true;
        currentMoveSpeed = currentMoveSpeed * (SlowPercentage / 100);
        yield return new WaitForSeconds(slowDuration);
        slowDebuff = false;
    }
    #endregion

    private void PlaySFX(AudioClip clipName)
    {
        AudioSource.PlayClipAtPoint(clipName, Camera.main.transform.position, 0.5f);
    }
    private IEnumerator Die()
    {
        currentHealth = 0;
        yield return new WaitForSeconds(playerRespawnDelay);

        Scene scene = SceneManager.GetActiveScene(); 
        SceneManager.LoadScene(scene.name);
    }
}
