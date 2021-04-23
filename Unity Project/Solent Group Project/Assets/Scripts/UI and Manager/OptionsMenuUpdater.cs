using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenuUpdater : MonoBehaviour
{
    [SerializeField] Slider SFXVolumeSlider;
    [SerializeField] Slider musicVolumeSlider;

    void Start()
    {
        SFXVolumeSlider.value = GameManager.myGameManager.GetSFXVolume();
        musicVolumeSlider.value = GameManager.myGameManager.GetMusicVolume();
    }

    public void SavePlayerSettings()
    {
        GameManager.myGameManager.UpdatePlayerPrefs(SFXVolumeSlider.value, musicVolumeSlider.value);
    }
}
