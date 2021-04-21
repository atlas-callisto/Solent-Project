using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingRock : MonoBehaviour
{
    [SerializeField] int damage = 1;
    [SerializeField] float lifeDuration = 5f;

    void Start()
    {
        Destroy(this.gameObject, lifeDuration);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamageable iDamageable = collision.GetComponent<IDamageable>();
        if (iDamageable != null)
        {
            iDamageable.TakeDamage(damage);
        }

        // So that rocks break when touching the ground,
        // you will need to remove this when doing animations in the future
        if(collision.gameObject.name == "Rock Catcher")
        {
            Destroy(gameObject);
        }
    }
}
