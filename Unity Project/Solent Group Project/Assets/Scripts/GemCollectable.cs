using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemCollectable : MonoBehaviour , Interactable
{
    public GameManager GM;
    public AudioSource DestroySound;
    private bool Collected;

    public void Interact()
    {
        if(!Collected)
        {
            Collected = true;
            GM.gemsCollected++;
            DestroySound.Play();
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
        GM.CheckCollectedGems();
    }
}
