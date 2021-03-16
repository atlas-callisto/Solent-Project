using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour , Interactable
{
    GameObject movingPlatform;
    void Awake()
    {
        movingPlatform = FindObjectOfType<MovingPlatform>().gameObject;
    }
    public void Interact()
    {
        movingPlatform.SetActive(true);
        Destroy(gameObject);
    }
}
