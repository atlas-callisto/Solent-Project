using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderWeb : MonoBehaviour
{
    [SerializeField] private float webSpeed;
    [SerializeField] private int webDamage;
    [SerializeField] private float webLifeDuration;
    [SerializeField] private bool WebIsDying;
    [SerializeField] float moveSpeedSlowPercentage;
    [SerializeField] float slowDuration;
    Player playerRef;
    Sprite myWebSprite;

    private void Start()
    {
        myWebSprite = GetComponent<SpriteRenderer>().sprite;
        StartCoroutine(Die(webLifeDuration));
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
            StartCoroutine(Die(0));
        }
        if (collision.gameObject.layer == 8) // Colliding with the ground layer
        {
            StartCoroutine(Die(0));
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

    // If you want a web to shrink immediately on impact, set delay to 0
    private IEnumerator Die(float Delay)
    {
        if (!WebIsDying)
        {
            WebIsDying = true;
            yield return new WaitForSeconds(Delay);

            for (int i = 0; i < 100; i++)
            {
                transform.localScale -= new Vector3(0.01f, 0.01f, 0.01f);
                yield return new WaitForSeconds(0.001f);
            }
            Destroy(gameObject);
        }
    }
}
