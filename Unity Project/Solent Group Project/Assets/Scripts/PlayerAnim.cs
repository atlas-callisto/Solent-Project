using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    Animator myAnimator;
    BoxCollider2D myBoxCollider2D;
    bool isGrounded = false;
    float rayCastHeightOffset = 0.1f;
    bool canDoubleJump;
    void Awake()
    {
        myAnimator = GetComponent<Animator>();
        myBoxCollider2D = GetComponent<BoxCollider2D>();
    }
    public void Idle()
    {

    }
    public void Walk(bool isWalking)
    {
        myAnimator.SetBool("IsWalking", isWalking);
    }
    public void Attack()
    {

        myAnimator.SetTrigger("Attack");
    }

    public void TransformToWolf(bool wolf)
    {
        myAnimator.SetBool("Transform", wolf);
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
            Debug.Log("Hello" + Time.deltaTime);
            isGrounded = true;
            canDoubleJump = true;
        }
        else
        {
            isGrounded = false;
        }

    }

}
