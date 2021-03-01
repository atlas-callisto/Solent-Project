using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    int currentScene = 0;

    void Start()
    {
        currentScene = SceneManager.GetActiveScene().buildIndex;
    }

    private IEnumerator RestartLevelAfterPause()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(currentScene);
    }
    public void StartGame()
    {
        SceneManager.LoadScene("level1");
    }
    public void RestartLevelAfterAPause()
    {
        StartCoroutine(RestartLevelAfterPause());
    }
    public void RestartLevel()
    {
        SceneManager.LoadScene(currentScene);
    }
    IEnumerator LoadNextLevel()
    {
        currentScene++;
        yield return new WaitForSeconds(2);
        //if (currentScene == SceneManager.sceneCountInBuildSettings)
        //{
        //    LoadMainMenu();
        //}
        SceneManager.LoadScene(currentScene);
    }
    public void LoadOptionsMenu()
    {
        SceneManager.LoadScene("OptionsMenu");
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    IEnumerator LoadGameOver()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("GameOverScene");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
