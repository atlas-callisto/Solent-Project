using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    int currentScene = 0;

    void Start()
    {
        RestoreTimeScale();
        currentScene = SceneManager.GetActiveScene().buildIndex;
    }

    IEnumerator RestartLevelAfterPause()
    {
        Time.timeScale = 0.3f;
        yield return new WaitForSeconds(1);
        RestoreTimeScale();
        SceneManager.LoadScene(currentScene);
    }
    public void StartGame()
    {
        RestoreTimeScale();
        SceneManager.LoadScene("level1");
    }
    public void RestartLevel()
    {
        RestoreTimeScale();
        SceneManager.LoadScene(currentScene);
    }
    IEnumerator LoadNextLevel()
    {
        currentScene++;

        Time.timeScale = 0.3f;
        yield return new WaitForSeconds(1);
        RestoreTimeScale();
        //if (currentScene == SceneManager.sceneCountInBuildSettings)
        //{
        //    LoadMainMenu();
        //}
        SceneManager.LoadScene(currentScene);
    }
    public void LoadOptionsMenu()
    {
        RestoreTimeScale();
        SceneManager.LoadScene("OptionsMenu");
    }

    public void LoadMainMenu()
    {
        RestoreTimeScale();
        SceneManager.LoadScene("MainMenu");
    }

    IEnumerator LoadGameOver()
    {
        Time.timeScale = 0.3f;
        yield return new WaitForSeconds(1);
        RestoreTimeScale();
        SceneManager.LoadScene("GameOverScene");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    private void RestoreTimeScale()
    {
        Time.timeScale = 1f;
    }
}
