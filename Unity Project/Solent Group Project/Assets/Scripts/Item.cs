using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour , Interactable
{
    public void Interact()
    {
        FindObjectOfType<CameraScript>().ToggleInvsibleLayer();
        Destroy(gameObject);
    }
}
