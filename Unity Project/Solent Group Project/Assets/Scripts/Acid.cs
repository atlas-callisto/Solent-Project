using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Acid : MonoBehaviour
{
    [SerializeField] int acidDamage = 1;
    [SerializeField] float acidDamageTickRate = 3f;
    [SerializeField] float timer = 0;
    [SerializeField] IDamageable iDamageableObj;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        iDamageableObj = collision.GetComponent<IDamageable>();
        if (iDamageableObj != null)
        {
            if (timer < acidDamageTickRate)
            {
                timer += Time.deltaTime;
                Debug.Log(timer);
                if (timer >= acidDamageTickRate)
                {
                    Debug.Log("Acid Damage called");
                    timer = 0;
                    iDamageableObj.TakeDamage(acidDamage);
                }
            }
        }
    }
}
