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
        myMusicBox.myAudioSource.volume = GameManager.myGameManager.GetMusicVolume();
        UpdateMusicDictionary();
    }
    public void UpdateAudioSource()
    {
        List<string> levelList = new List<string>();
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string path = SceneUtility.GetScenePathByBuildIndex(i);
            string sceneName = path.Substring(0, path.Length - 6).Substring(path.LastIndexOf('/') + 1);
            levelList.Add(sceneName);
        }
        string currentLevelName = SceneManager.GetActiveScene().name;
        foreach (var levelName in levelList)
        {
            if (levelName == currentLevelName)
            {
                myMusicBox.myAudioClip = myMusicBox.myAudioDictionary[levelName];
            }
        }
        if (myMusicBox.myAudioSource.clip != myMusicBox.myAudioClip)
        {
            myMusicBox.myAudioSource.clip = myMusicBox.myAudioClip;
            myMusicBox.myAudioSource.Play();
        }
    }
    public void UpdateMusicDictionary()
    {        
        myMusicBox.myAudioDictionary = GetComponent<MusicLibrary>().myAudioClipDicLib;
        string currentLevelName = SceneManager.GetActiveScene().name;
        foreach (var levelName in myMusicBox.myAudioDictionary.Keys)
        {
            if(levelName == currentLevelName)
            {
                myMusicBox.myAudioClip = myMusicBox.myAudioDictionary[levelName];
            }
        }
        if (!myMusicBox.myAudioClip)
        {
            print("Audio clip missing in the MusicLibrary script for this " + currentLevelName + " level.");
            return;
        }
        if(myMusicBox.myAudioSource.clip != myMusicBox.myAudioClip)
        {
            myMusicBox.myAudioSource.clip = myMusicBox.myAudioClip;
            myMusicBox.myAudioSource.Play();
        }        
    }

    public void UpdateMusicVolume(float musicVolume)
    {
        myMusicBox.myAudioSource.volume = musicVolume;
    }
}
