using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    //Params
    [SerializeField] float moveSpeed = 4f;
    [SerializeField] float jumpForce = 5f;
    [SerializeField] float rayCastHeightOffset = 0.1f;
    [SerializeField] bool isGrounded = false;
    [SerializeField] bool canDoubleJump;
    [SerializeField] bool doubleJumpSkill;
    [SerializeField] int Health = 10;
    bool wolf = false; //Transform to wolf


    //temp
    float timer = 0.2f;

    //Comp Ref
    Rigidbody2D myRB;
    Animator myAnimator;
    SpriteRenderer mySpriteRenderer;
    BoxCollider2D myBoxCollider2D;

    //Ref Objs
    public GameObject attackTrigger;
 
    // Start is called before the first frame update
    private void Awake()
    {
        myRB = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBoxCollider2D = GetComponent<BoxCollider2D>();
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {       
        PlayerMovement();
        PlayerJump();
        PlayerAttack();
        CheckForGrounded();
        Transform();
    }


    private void PlayerMovement()
    {
        float horizontalMov = Input.GetAxisRaw("Horizontal");
        float verticalMov = Input.GetAxisRaw("Vertical");

        bool isWalking = horizontalMov != 0 ? true : false;
        myAnimator.SetBool("IsWalking", isWalking);

        
        if (horizontalMov > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        if (horizontalMov < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
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
            //Basic Attack
        }
        if (Input.GetButtonDown("Heavy Attack"))
        {
            myAnimator.SetTrigger("Attack");
            AttackTrigger(timer);
            //Attack
        }
        if (Input.GetButtonDown("Special Attack"))
        {
            myAnimator.SetTrigger("Attack");
            AttackTrigger(timer);
            //Attack
        }
    }

    private void CheckForGrounded()
    {
        RaycastHit2D hitInfo;
        RaycastHit2D hitInfo2;
        Vector3 offset = new Vector3(0.1f, 0, 0);
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
            myAnimator.SetBool("Transform" , wolf);
            wolf = !wolf;
        }
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

    public void Damage(int damage) //Interface take damage
    {
        Health -= damage;
        if (Health <= 0)
        {
            //Player Dies
            myAnimator.SetBool("Dead", true);
        }
    }


}
