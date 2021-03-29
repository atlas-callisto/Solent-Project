using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Acid : MonoBehaviour
{
    [SerializeField] int acidDamage = 1;
    private IDamageable iDamageableObj;

    private void OnTriggerStay2D(Collider2D collision)
    {
        iDamageableObj = collision.GetComponent<IDamageable>();
        if (iDamageableObj != null)
        {
            iDamageableObj.TakeDamage(acidDamage);
        }
    }    
}
