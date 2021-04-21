using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTransistion : MonoBehaviour
{
    [SerializeField] private string levelName;
    [SerializeField] private string spawnLocationName;
    private Player playerRef;
    public static bool canTransitionn = true; // this is made so that the script doesnot get called multiple times
    private bool playerIsOnTheDoor;
    void Awake()
    {
        playerRef = FindObjectOfType<Player>();
    }
    void Update()
    {
        if(playerIsOnTheDoor)
        {
            if (playerRef.canInteract && canTransitionn)
            {
                canTransitionn = false; // this is made so that the script doesnot get called multiple times
                FindObjectOfType<LevelLoader>().LoadLevelWithName(levelName, spawnLocationName);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) playerIsOnTheDoor = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) playerIsOnTheDoor = false;
    }
}
