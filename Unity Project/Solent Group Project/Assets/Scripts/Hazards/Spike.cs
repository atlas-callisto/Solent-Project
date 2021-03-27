using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    public float timer;
    public float damageTickRate;
    public int damage;

    private void OnCollisionStay2D(Collision2D collision)
    {
        IDamageable iDamageableObj;
        iDamageableObj = collision.gameObject.GetComponent<IDamageable>();
        if (iDamageableObj != null)
        {
            if (timer <= damageTickRate)
            {
                timer += Time.deltaTime;
                if (timer >= damageTickRate)
                {
                    timer = 0;
                    iDamageableObj.TakeDamage(damage);
                }
            }
        }
    }

}
