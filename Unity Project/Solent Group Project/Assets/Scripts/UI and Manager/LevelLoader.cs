﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    int currentScene = 0;
    float sceneTransitionDelay = 0f;
    string playerSpwanLocationName;
    GameObject playerRef;
    
    public void NewGame() // Load new game
    {
        SceneManager.LoadScene("level1"); 
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        playerRef = FindObjectOfType<Player>().gameObject;
        if (GameObject.Find(playerSpwanLocationName)) playerRef.transform.position = GameObject.Find(playerSpwanLocationName).transform.position;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void RestartLevel()
    {
        currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene);
    }
    public void RestartLevelAfterAPause()
    {
        StartCoroutine(RestartLevelAfterPause());
    }
    private IEnumerator RestartLevelAfterPause()
    {
        currentScene = SceneManager.GetActiveScene().buildIndex;
        yield return new WaitForSeconds(sceneTransitionDelay);
        SceneManager.LoadScene(currentScene);
    }
   
    public void LoadLevelWithName(string levelName , string spawnLocation)
    {
        playerSpwanLocationName = spawnLocation;
        StartCoroutine(LoadNextLevel(levelName));
    }
    private IEnumerator LoadNextLevel(string levelName)
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene(levelName);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void LoadOptionsMenu()
    {
        SceneManager.LoadScene("OptionsMenu");
    }   

    IEnumerator LoadGameOver()
    {
        yield return new WaitForSeconds(sceneTransitionDelay);
        SceneManager.LoadScene("GameOverScene");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
