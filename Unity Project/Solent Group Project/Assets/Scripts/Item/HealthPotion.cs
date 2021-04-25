using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : MonoBehaviour
{
    [SerializeField] int healingAmount = 5;
    private bool isTouchingDown;
    public Rigidbody2D potionrb;
    [SerializeField] float groundCheckRadius;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Transform groundCheckDown;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Player.currentHealth = Mathf.Clamp(Player.currentHealth + healingAmount, 0, Player.maxHealth);
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
