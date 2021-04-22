using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemCollectable : MonoBehaviour , Interactable
{
    public AudioSource DestroySound;
    private bool Collected = false;
    private static List<string> gemCollectables = new List<string>();
    private static bool storeRefOnce = false;

    private void Start()
    {
        if(!storeRefOnce)
        {
            storeRefOnce = true;
            var totalGemsFound = FindObjectsOfType<GemCollectable>();
            foreach (var gem in totalGemsFound) gemCollectables.Add(gem.gameObject.name);
        }
        if (!gemCollectables.Contains(this.gameObject.name)) gameObject.SetActive(false);
    }
    public void Interact()
    {
        if(!Collected)
        {
            Collected = true;
            DestroySound.Play();
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            gemCollectables.Remove(this.gameObject.name);
            GameManager.myGameManager.CheckCollectedGems();
            string gemName = gameObject.name;
            FindObjectOfType<GemCanvas>().UpdateCanvas( int.Parse(gemName.Substring(gemName.Length - 1, 1)) );
        }
    }
}
