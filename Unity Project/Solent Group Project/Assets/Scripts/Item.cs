using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour , Interactable
{
    List<GameObject> movingPlatformList = new List<GameObject>();
    void Awake()
    {
        var movingPlatforms = FindObjectsOfType<MovingPlatform>();
        foreach(MovingPlatform movingPlatform in movingPlatforms)
        {
            movingPlatformList.Add(movingPlatform.gameObject);
        }                 
    }
    void Start()
    {
        if (GameManager.myGameManager.moonsEyeMonacle == true)
        {
            foreach (var movingPlafrom in movingPlatformList)
            {
                movingPlafrom.SetActive(true);
            }
        }
        if (GameManager.myGameManager.moonsEyeMonacle) Destroy(gameObject);
    }
    public void Interact()
    {
        if(movingPlatformList.Count > 0)
        {
            GameManager.myGameManager.moonsEyeMonacle = true;
            foreach (GameObject movingPlatformGameObject in movingPlatformList)
            {
                movingPlatformGameObject.SetActive(true);
            }
        }        
        Destroy(gameObject);
    }
}
