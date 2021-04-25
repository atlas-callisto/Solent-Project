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

    public AudioClip SFXVolumeChangeTestAudioClip;
    private bool mouseButtonDown = false; // Used to play SFX

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
        SFXVolumeSlider.value = GameManager.myGameManager.GetSFXVolume();
        musicVolumeSlider.value = GameManager.myGameManager.GetMusicVolume();
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
        pausePanel.SetActive(false);
        optionsPanel.SetActive(true);
        SFXVolumeSlider.value = GameManager.myGameManager.GetSFXVolume();
        musicVolumeSlider.value = GameManager.myGameManager.GetMusicVolume();        
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
    public void SaveChangesInMusicVolumeSLider()
    {
        GameManager.myGameManager.UpdatePlayerPrefs(SFXVolumeSlider.value, musicVolumeSlider.value);
    }
    public void SaveChangesInSFXVolumeSLider()
    {
        GameManager.myGameManager.UpdatePlayerPrefs(SFXVolumeSlider.value, musicVolumeSlider.value);
        if (Input.GetMouseButton(0)) mouseButtonDown = true;
        else mouseButtonDown = false;
        PlaySFX();
    }
    private void PlaySFX()
    {
        if (!mouseButtonDown) return;
        var sfx = new GameObject();
        sfx.AddComponent<AudioSource>();
        sfx.GetComponent<AudioSource>().PlayOneShot(SFXVolumeChangeTestAudioClip, GameManager.myGameManager.GetSFXVolume());
        Destroy(sfx, SFXVolumeChangeTestAudioClip.length);
    }
}
