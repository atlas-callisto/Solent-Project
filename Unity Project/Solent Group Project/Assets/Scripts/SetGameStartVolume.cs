using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetGameStartVolume : MonoBehaviour
{
    private static bool doOnce = true;
    public float defaultSFXVolumeWhenGameStarts = 0.5f;
    public float defaultMusicVolumeWhenGameStarts = 0.5f;

    // Start is called before the first frame update
    // Update is called once per frame
    void Update()
    {
        if(doOnce)
        {
            doOnce = false;
            GameManager.myGameManager.UpdatePlayerPrefs(defaultSFXVolumeWhenGameStarts, defaultMusicVolumeWhenGameStarts);
        }        
    }
}
