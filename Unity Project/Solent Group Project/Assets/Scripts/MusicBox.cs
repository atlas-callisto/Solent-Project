using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MusicBox : MonoBehaviour
{
    private AudioSource myAudioSource;
    private AudioClip myAudioClip;
    private Dictionary<string, AudioClip> myAudioDictionary = new Dictionary<string, AudioClip>();
    //Volume to adjust music is in player prefs but can be accessed by a function 

    public static MusicBox myMusicBox;
    private void Awake()
    {
        if (myMusicBox != null)
        {
            Destroy(this.gameObject);
            return;
        }
        myMusicBox = this;
        DontDestroyOnLoad(this.gameObject);        
    }

    void Start()
    {
        myAudioSource = GetComponent<AudioSource>();
        UpdateMusic();
        myMusicBox.myAudioSource.Play();
        myMusicBox.myAudioSource.volume = GameManager.myGameManager.GetMusicVolume();
    }

    private static void UpdateMusic()
    {
        myMusicBox.myAudioDictionary  = FindObjectOfType<MusicLibrary>().myAudioClipDicLib;

        string currentLevelName = SceneManager.GetActiveScene().name;
        foreach (var levelName in myMusicBox.myAudioDictionary.Keys)
        {
            if(levelName == currentLevelName)
            {
                myMusicBox.myAudioClip = myMusicBox.myAudioDictionary[levelName];
            }
        }
        print(myMusicBox.myAudioClip);
        if(myMusicBox.myAudioSource.clip != myMusicBox.myAudioClip)
        {
            myMusicBox.myAudioSource.clip = myMusicBox.myAudioClip;
            myMusicBox.myAudioSource.Play();
        }        
    }

    private void SetUpSingleton()
    {
        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }
    public void UpdateMusicVolume(float musicVolume)
    {
        myMusicBox.myAudioSource.volume = musicVolume;
    }
}
