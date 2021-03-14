using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HiddenAreaTileScript : MonoBehaviour
{
    public GameObject HubHiddenArea;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == ("Player"))
        {
            HubHiddenArea.GetComponent<TilemapRenderer>().enabled = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        HubHiddenArea.GetComponent<TilemapRenderer>().enabled = true;
    }
}
