using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemplarSpear : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    public bool playerWasHit = false;
    private TemplarKnight myTemplarKnightRef;

    void Start()
    {
        myTemplarKnightRef = GetComponentInParent<TemplarKnight>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamageable iDamageable = collision.GetComponent<IDamageable>();
        if (iDamageable != null)
        {
            iDamageable.TakeDamage(damage);
            if (collision.gameObject.GetComponent<Player>())
            {
                playerWasHit = true;
                myTemplarKnightRef.StopChargingTowardsPlayer();
                collision.GetComponent<Player>().KnockBackEffect(GetKnockBackDirection(collision.gameObject));
                gameObject.SetActive(false);
            }
        }
    }
    private Vector2 GetKnockBackDirection(GameObject collision)
    {
        if (collision.transform.position.x > this.transform.position.x) return Vector2.right;
        else return Vector2.left;
    }
    public void FinishedAttacking() //called by animation event
    {
        this.gameObject.SetActive(false);
    }
}
