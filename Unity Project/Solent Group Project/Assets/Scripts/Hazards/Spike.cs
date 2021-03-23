using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    public float timer;
    public float damageTickRate;
    public int damage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
            print("Collision");
        }
    }

}
