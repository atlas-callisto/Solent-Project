using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    //Params    
    public static int maxHealth = 10; // Making static variable so that it can persists through different game levels
    public static int currentHealth = 10;
    public static float maxWolfBar = 10;
    public static float currentWolfBar = 10;
    public static float wolfBarRegenRate = 0.1f;
    public static float wolfDegeneRate = 1f;
    public static bool initializePlayerStats = true;

    [Header("Stats")] // Couldnot expose static variables into the inspector so this is a work around
    [SerializeField] public int playerMaxHealth = 10;
    [SerializeField] public int playerCurrentHealth = 10;
    [SerializeField] public float playerMaxWolfBar = 10;
    [SerializeField] public float playerCurrentWolfBar = 10;
    [SerializeField] public float playerWolfBarRegenRate = 0.1f;
    [SerializeField] public float playerWolfDegeneRate = 1f;
    [SerializeField] private float collisionKnockBackForce = 500f;
    [SerializeField] private float playerInvunerabletimer = 0.3f; // timer to stop player playing from getting damage after taking a hit


    [Header("Cool Downs")]
    [SerializeField] private float basicAttackCoolDown = 2f;
    [SerializeField] private float heavyAttackCoolDown = 5f;
    [SerializeField] private float specialAttakCoolDown = 10f;
    [Tooltip("It is a pause after attacks so that player cannot spam multiple attacks at once")] [SerializeField] private float afterAttackPause = 2f;

    [Header("Human Stats")]
    [SerializeField] float humanMoveSpeed = 4f;
    [SerializeField] float humanJumpForce = 5f;

    [Header("Wolf Stats")]
    [SerializeField] float wolfMoveSpeed = 6f;
    [SerializeField] float wolfJumpForce = 8f;
    
    private float jumpForce = 5f;
    private bool slowDebuff = false;

    //Ref Objs
    [Header("Audio SFX")]
    public AudioClip humanAttackSFX;
    public AudioClip deathSFX;
    public AudioClip takingDamageSFX;
    public AudioClip shootingSFX;
    public AudioClip wolfRoarSFX;

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
    [Tooltip("This delay gives enough time to show the player death animation before restarting the level")]
    public float playerRespawnDelay;
    private bool canAttack = true; //this is to stop player from using multiple attacks at once;

    internal bool wolf = false; //Transform to wolf, also called by moonlight script
    internal bool playerAlive = true;
    public static bool playerisTalking = false;

    internal bool isGrounded = false; // is modified by playerGroundCheck
    internal bool canDoubleJump = false;// is modified by playerGroundCheck
    internal bool canInteract = false;

    internal bool doubleJumpSkillAcquired = false; // enable double jump after skill is unlocked??? for later use
    private bool playerCanTakeDmg = true;    

    //Comp Ref
    private Rigidbody2D myRB;
    private Animator myAnimator;
    private SpriteRenderer mySpriteRenderer;
    private void Awake()
    {
        myRB = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        if(initializePlayerStats)
        {
            maxHealth = playerMaxHealth;
            currentHealth = playerCurrentHealth;
            maxWolfBar = playerMaxWolfBar;
            currentWolfBar = playerCurrentWolfBar;
            wolfBarRegenRate = playerWolfBarRegenRate;
            wolfDegeneRate = playerWolfDegeneRate;
            initializePlayerStats = false;
        }
    }   
    void Start()
    {
        doubleJumpSkillAcquired = GameManager.myGameManager.airTreaders;
        basicAttackTimer = basicAttackCoolDown;
        heavyAttackTimer = heavyAttackCoolDown;
        specialAttackTimer = specialAttakCoolDown;
    }    
    void Update()
    {
        if (!playerAlive)
        {
            myRB.velocity = new Vector2(0, myRB.velocity.y); // Stops player corpse from sliding
            return;
        }
        Interaction(); 
        if (playerisTalking)
        {
            myRB.velocity = new Vector2(0, myRB.velocity.y); // Stops player from continusously moving
            return; // Stops user control when player is talking to NPC
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
        if (Input.GetButtonDown("Transform")) wolf = !wolf;
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
        if (!canAttack) return; // stops player from spaming multiple attacks at once.
        if (Input.GetButtonDown("Basic Attack"))  // 1 = Basic attack, 2 = heavy attack, 3 = special attack
        {
            if (basicAttackTimer >= basicAttackCoolDown) AttackWithType(1);
        }
        if (Input.GetButtonDown("Heavy Attack"))
        {
            if (heavyAttackTimer >= heavyAttackCoolDown) AttackWithType(2);
        }
        if (!wolf)        
        {            
            if (Input.GetButtonDown("Special Attack"))
            {
                if (specialAttackTimer >= specialAttakCoolDown) ShootProjectile();
            }
        }
        else // If player has transformed to wolf
        {
            if (Input.GetButtonDown("Special Attack"))
            {
                if (specialAttackTimer >= specialAttakCoolDown)
                {
                    PlaySFX(wolfRoarSFX);
                    myAnimator.SetTrigger("Roar");
                    fearDebuffApplier.SetActive(true);
                    specialAttackTimer = 0;
                    StartCoroutine(AttackCoolDown());
                    // StartCoroutine(UseWolfSpecialAttack());
                }
            }
        }       
    }
    public void FinishedScreaming() // Just like me after doing the code for this attack // Will be called through Animation Event
    {
        fearDebuffApplier.SetActive(false);
    }
    private IEnumerator AttackCoolDown()
    {
        canAttack = false;
        yield return new WaitForSeconds(afterAttackPause);
        canAttack = true;
    }
    private void AttackWithType(int attackType)
    {
        StartCoroutine(AttackCoolDown());
        myAnimator.SetTrigger("Attack");
        if (!wolf)
        {
            PlaySFX(humanAttackSFX);
            playerWep.gameObject.SetActive(true);
            playerWep.SetDamage(attackType);
            playerWep.Attack(attackType);
        }
        if(wolf)
        {
            //PlaySFX(wolfAttackSFX)
            playerWolfWep.gameObject.SetActive(true);
            playerWolfWep.SetDamage(attackType);
            playerWolfWep.Attack(attackType);
        }
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
        //shooting animation
        PlaySFX(shootingSFX);
        GameObject projectile = Instantiate(projectilePrefab, transform.position, transform.localRotation);
        specialAttackTimer = 0;
        StartCoroutine(AttackCoolDown());
    }
    #endregion

    #region Interact System
    private void Interaction()
    {
        if (Input.GetButtonDown("Interact")) canInteract = true;
        else if (Input.GetButtonUp("Interact")) canInteract = false;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Interactable")
        {
            Interactable interactableObj = collision.GetComponent<Interactable>();
            if (interactableObj != null && canInteract)
            {
                collision.GetComponent<Interactable>().Interact();
            }
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
                PlaySFX(deathSFX);                
                playerAlive = false;
                myAnimator.SetTrigger("Dead");          
                FindObjectOfType<LevelLoader>().RestartLevelAfterAPause();
            }
        }
    }
    public void KnockBackEffect(Vector2 direction)
    {
        myRB.AddForce(collisionKnockBackForce * direction);
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
}
