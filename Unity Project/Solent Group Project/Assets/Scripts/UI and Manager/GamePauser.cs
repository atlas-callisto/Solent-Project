using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePauser : MonoBehaviour
{
    [SerializeField] GameObject pausePanel;
    internal bool gameIsPaused = false;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) PauseGame();
    }
    private void PauseGame()
    {
        Time.timeScale = 0;
        gameIsPaused = true;
        pausePanel.SetActive(true);

    }
    public void ResumeGame()
    {
        Time.timeScale = 1;
        gameIsPaused = false;
        pausePanel.SetActive(false);
    }
}
