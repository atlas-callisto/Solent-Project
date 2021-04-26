using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenuUpdater : MonoBehaviour
{
    [SerializeField] Slider SFXVolumeSlider;
    [SerializeField] Slider musicVolumeSlider;
    [SerializeField] List<AudioClip> SFXVolumeChangeTestAudioClip;
    private bool mouseButtonDown = false; // Used to play SFX
    AudioSource myAudioSource;
    void Start()
    {
        myAudioSource = GetComponent<AudioSource>();
        SFXVolumeSlider.value = GameManager.myGameManager.GetSFXVolume();
        musicVolumeSlider.value = GameManager.myGameManager.GetMusicVolume();
    }
    private void Update()
    {
        //SFXVolumeSlider.value = GameManager.myGameManager.GetSFXVolume();
        //musicVolumeSlider.value = GameManager.myGameManager.GetMusicVolume();
    }
    public void SavePlayerSettings()
    {
        GameManager.myGameManager.UpdatePlayerPrefs(SFXVolumeSlider.value, musicVolumeSlider.value);
    }
    public void SaveChangesInMusicVolumeSLider()
    {
        MusicBox.myMusicBox.UpdateMusicVolume(musicVolumeSlider.value);
    }
    public void SaveChangesInSFXVolumeSLider()
    {
        if (SFXVolumeSlider.value == GameManager.myGameManager.GetSFXVolume()) return; // stops sfx from playing in the beggning
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
