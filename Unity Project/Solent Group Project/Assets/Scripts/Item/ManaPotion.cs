using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaPotion : MonoBehaviour
{
    [SerializeField] int manaAmount = 5;
    private bool isTouchingDown;
    public Rigidbody2D potionrb;
    [SerializeField] float groundCheckRadius;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Transform groundCheckDown;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Player.currentWolfBar = Mathf.Clamp(Player.currentWolfBar + manaAmount, 0, Player.maxWolfBar);
            Destroy(this.gameObject);
        }
    }

    private void Update()
    {
        isTouchingDown = Physics2D.OverlapCircle(groundCheckDown.position, groundCheckRadius, groundLayer);
        if (isTouchingDown)
        {
            potionrb.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        }
    }
}
