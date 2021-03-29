using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransistion : MonoBehaviour
{
    [SerializeField] private string levelName;
    [SerializeField] private string spawnLocationName;
    private Player playerRef;
    public static bool canTransitionn = true;
    void Start()
    {
        playerRef = FindObjectOfType<Player>();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if (playerRef.canInteract && canTransitionn)
            {
                canTransitionn = false;
                FindObjectOfType<LevelLoader>().LoadLevelWithName(levelName, spawnLocationName);
            }
        }
    }
}
