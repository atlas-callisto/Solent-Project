using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D myRB;
    Animator myAnimator;
    SpriteRenderer mySpriteRenderer;

    public float moveSpeed = 4f;
    public float jumpForce = 5f;
    public float raycastHitDistance = 0.6f;
    public bool isGrounded = false;
    public bool canDoubleJump;
    public bool doubleJumpSkill;
    bool wolf = false;

    // Start is called before the first frame update
    private void Awake()
    {
        myRB = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
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
        myAnimator.SetBool("isWalking", isWalking);

        
        if (horizontalMov > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        if (horizontalMov < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        myRB.velocity = new Vector2(horizontalMov * moveSpeed, myRB.velocity.y);
        Debug.Log(horizontalMov);
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
            myAnimator.SetTrigger("attack");
            //Basic Attack
        }
        if (Input.GetButtonDown("Heavy Attack"))
        {
            myAnimator.SetTrigger("attack");
            //Attack
        }
        if (Input.GetButtonDown("Special Attack"))
        {
            myAnimator.SetTrigger("attack");
            //Attack
        }
    }

    private void CheckForGrounded()
    {
        // This helps to adjust the raycastHitDistance based on the sprite size. Disable this later.
        Debug.DrawRay(transform.position, Vector2.down * raycastHitDistance, Color.green);
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, Vector2.down, raycastHitDistance, LayerMask.GetMask("Ground")); // ground layer must be applied to ground tilemap
        if (hitInfo)
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
        if(Input.GetButton("Interact"))
        {
            collision.GetComponent<Interactable>().Interact();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Interactable")
        {
            Interact(collision);
        }        
    }
}
