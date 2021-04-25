using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenuUpdater : MonoBehaviour
{
    [SerializeField] Slider SFXVolumeSlider;
    [SerializeField] Slider musicVolumeSlider;
    [SerializeField] AudioClip SFXVolumeChangeTestAudioClip;
    private bool mouseButtonDown = false; // Used to play SFX
    void Start()
    {
        SFXVolumeSlider.value = GameManager.myGameManager.GetSFXVolume();
        musicVolumeSlider.value = GameManager.myGameManager.GetMusicVolume();
    }
    private void Update()
    {
        SFXVolumeSlider.value = GameManager.myGameManager.GetSFXVolume();
        musicVolumeSlider.value = GameManager.myGameManager.GetMusicVolume();
    }
    public void SavePlayerSettings()
    {
        GameManager.myGameManager.UpdatePlayerPrefs(SFXVolumeSlider.value, musicVolumeSlider.value);
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
