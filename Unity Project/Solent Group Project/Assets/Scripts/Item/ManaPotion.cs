using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaPotion : MonoBehaviour
{
    [SerializeField] int manaAmount = 5;
    private bool isTouchingDown;
    public Rigidbody2D potionrb;
    [SerializeField] float groundCheckRadius;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Transform groundCheckDown;
    [SerializeField] AudioClip potionConsumeSFX;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Player.currentWolfBar = Mathf.Clamp(Player.currentWolfBar + manaAmount, 0, Player.maxWolfBar);
            PlaySFX(potionConsumeSFX);
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
    protected void PlaySFX(AudioClip clipName)
    {
        var sfx = new GameObject();
        sfx.AddComponent<AudioSource>();
        sfx.GetComponent<AudioSource>().PlayOneShot(clipName, GameManager.myGameManager.GetSFXVolume());
        Destroy(sfx, clipName.length);
    }
}
