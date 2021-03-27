using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransistion : MonoBehaviour
{
    [SerializeField] private string levelName;
    [SerializeField] private string spawnLocationName;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            FindObjectOfType<LevelLoader>().LoadLevelWithName(levelName, spawnLocationName);
        }
    }
}
