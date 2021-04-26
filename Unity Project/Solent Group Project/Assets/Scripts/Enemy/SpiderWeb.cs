using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderWeb : MonoBehaviour
{
    [SerializeField] private float webSpeed;
    [SerializeField] private int webDamage;
    [SerializeField] private float webLifeDuration;
    [SerializeField] float moveSpeedSlowPercentage;
    [SerializeField] float slowDuration;
    Player playerRef;
    Sprite myWebSprite;

    private void Start()
    {
        myWebSprite = GetComponent<SpriteRenderer>().sprite;
        Destroy(this.gameObject, webLifeDuration);
        playerRef = FindObjectOfType<Player>();
    }
    void Update()
    {
        transform.Translate(Vector3.right * webSpeed * Time.deltaTime);
    }    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamageable iDamageable = collision.GetComponent<IDamageable>();
        if (iDamageable != null)
        {
            iDamageable.TakeDamage(webDamage);
            if (collision.gameObject.tag == "Player")
            {
                playerRef.SlowMoveSpeedDebuff(moveSpeedSlowPercentage, slowDuration);
                WebSLowEffectVisual(collision.gameObject);

            }
            Destroy(this.gameObject);
        }
        if (collision.gameObject.layer == 8) // Colliding with the ground layer
        {
            Destroy(this.gameObject);
        }
    }

    private void WebSLowEffectVisual(GameObject collision)
    {
        var vfx = new GameObject();
        vfx.AddComponent<SpriteRenderer>();
        vfx.GetComponent<SpriteRenderer>().sprite = myWebSprite;
        vfx.GetComponent<SpriteRenderer>().sortingOrder = 10;
        vfx.transform.SetParent(collision.transform);
        vfx.transform.localPosition = Vector3.zero;
        Destroy(vfx, slowDuration);
    }
}
