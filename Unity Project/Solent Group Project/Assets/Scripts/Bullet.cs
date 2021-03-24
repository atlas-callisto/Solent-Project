using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float bulletSpeed;
    [SerializeField] private int bulletDamage;
    [SerializeField] private float bulletLifeDuration;
    void Start()
    {
        Destroy(this.gameObject, bulletLifeDuration);
    }
    void Update()
    {
        transform.Translate(Vector3.right * bulletSpeed * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamageable iDamageable = collision.GetComponent<IDamageable>();
        if (iDamageable != null)
        {
            iDamageable.TakeDamage(bulletDamage);
            if (collision.gameObject.GetComponent<Player>())
            {
                collision.gameObject.GetComponent<Player>().KnockBackEffect(collision.gameObject.transform.position - this.transform.position);
            }
            Destroy(this.gameObject);
        }
        if (collision.gameObject.layer == 8) // Colliding with the ground layer
        {
            Destroy(this.gameObject);
        }
    }
}
