using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransistion : MonoBehaviour
{
    [SerializeField] private string levelName;
    [SerializeField] private string spawnLocationName;

    private LevelLoader levelLoaderRef;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            levelLoaderRef = FindObjectOfType<LevelLoader>();
            levelLoaderRef.LoadLevelWithName(levelName, spawnLocationName);           
        }
    }

}
