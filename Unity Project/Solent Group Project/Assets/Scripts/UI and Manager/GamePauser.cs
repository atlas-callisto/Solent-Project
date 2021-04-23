using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePauser : MonoBehaviour
{
    [SerializeField] GameObject pausePanel;
    [SerializeField] GameObject optionsPanel;
    [SerializeField] GameObject quitWarningPanel;
    [SerializeField] Slider SFXVolumeSlider;
    [SerializeField] Slider musicVolumeSlider;
    public bool gameIsPaused = false;

    void Start()
    {
        gameIsPaused = false;
    }
    void Update()
    {
        if (Player.playerisTalking) return;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(!gameIsPaused)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }

        }
    }

    public void PauseGame()
    {
        gameIsPaused = true;
        Cursor.visible = true;
        Time.timeScale = 0;
        pausePanel.SetActive(true);
        optionsPanel.SetActive(false);
        quitWarningPanel.SetActive(false);
    }
    public void ResumeGame()
    {
        gameIsPaused = false;
        Cursor.visible = false;
        Time.timeScale = 1;
        pausePanel.SetActive(false);
        optionsPanel.SetActive(false);
        quitWarningPanel.SetActive(false);
    }
    public void OptionsMenu()
    {
        SFXVolumeSlider.value = GameManager.myGameManager.GetSFXVolume();
        musicVolumeSlider.value = GameManager.myGameManager.GetMusicVolume();

        pausePanel.SetActive(false);
        optionsPanel.SetActive(true);
    }
    public void QuitWarningMenu()
    {
        quitWarningPanel.SetActive(true);
    }
    public void SaveAndBack()
    {
        GameManager.myGameManager.UpdatePlayerPrefs(SFXVolumeSlider.value, musicVolumeSlider.value);
        pausePanel.SetActive(true);
        optionsPanel.SetActive(false);
    }
}
