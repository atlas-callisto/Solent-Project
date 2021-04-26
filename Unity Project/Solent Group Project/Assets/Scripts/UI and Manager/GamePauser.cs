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

    public List<AudioClip> SFXVolumeChangeTestAudioClip;
    private bool mouseButtonDown = false; // Used to play SFX
    private AudioSource myAudioSource;

    void Start()
    {
        myAudioSource = GetComponent<AudioSource>();
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
        pausePanel.SetActive(true);
        optionsPanel.SetActive(false);
        quitWarningPanel.SetActive(false);
        Time.timeScale = 0;
    }
    public void ResumeGame()
    {
        gameIsPaused = false;
        Cursor.visible = false;
        pausePanel.SetActive(false);
        optionsPanel.SetActive(false);
        quitWarningPanel.SetActive(false);
        MusicBox.myMusicBox.UpdateMusicVolume(GameManager.myGameManager.GetMusicVolume()); 
        // this is to stop a bug where when player press escape, the music volume doesnot revert back
        Time.timeScale = 1;
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
        MusicBox.myMusicBox.UpdateMusicVolume(musicVolumeSlider.value);
    }
    public void SaveChangesInSFXVolumeSLider()
    {
        if(SFXVolumeSlider.value == GameManager.myGameManager.GetSFXVolume()) return; // stops sfx from playing in the beggning
        myAudioSource.volume = SFXVolumeSlider.value;
        if (myAudioSource.isPlaying) return;
        else
        {
            int i = Random.Range(0, SFXVolumeChangeTestAudioClip.Count);
            AudioClip cliptoPlay = SFXVolumeChangeTestAudioClip[i];
            myAudioSource.clip = cliptoPlay;
            myAudioSource.Play();
        }
    }   
}
