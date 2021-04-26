using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[ExecuteAlways]
public class MusicLibrary : MonoBehaviour
{
    public bool clearMusicLibrary = false;
    private bool updateMusicDictionary = false;
    private List<string> sceneNamesList = new List<string>();
    private List<AudioClip> audioClipList = new List<AudioClip>();

    [System.Serializable]
    public struct audioClipLibrary
    {
        public string levelName; public AudioClip audioClip;
        public audioClipLibrary(string levelName, AudioClip audioClip)
        {
            this.levelName = levelName;
            this.audioClip = audioClip;
        }
    }

    [SerializeField]
    public List<audioClipLibrary> myAudioClipStructLib = new List<audioClipLibrary>();

    public Dictionary<string, AudioClip> myAudioClipDicLib = new Dictionary<string, AudioClip>();
    void Start() // this sets up the library and is called before MusicBox script
    {
        updateMusicDictionary = true;
        UpdateAudioLibrary();
        if (updateMusicDictionary)
        {
            updateMusicDictionary = !updateMusicDictionary;
            myAudioClipDicLib.Clear();            
            UpdateAudioDictionary();
        }
    }

    void Update() // this is for updaing the library in edit mode
    {
        UpdateAudioLibrary();
        if (updateMusicDictionary)
        {
            updateMusicDictionary = !updateMusicDictionary;
            myAudioClipDicLib.Clear();
            UpdateAudioDictionary();
        }
    }
    private void UpdateAudioLibrary()
    {
        if(clearMusicLibrary)
        {
            sceneNamesList.Clear();
            myAudioClipStructLib.Clear();
            myAudioClipDicLib.Clear();
            return;
        }
        if (myAudioClipStructLib.Count > 0) return;
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string path = SceneUtility.GetScenePathByBuildIndex(i);
            string sceneName = path.Substring(0, path.Length - 6).Substring(path.LastIndexOf('/') + 1);
            sceneNamesList.Add(sceneName);
        }
        for (int i = 0; i <sceneNamesList.Count; i ++)
        {
            audioClipLibrary myACL = new audioClipLibrary(sceneNamesList[i], null);
            myAudioClipStructLib.Add(myACL);
        }
    }

    private void UpdateAudioDictionary()
    {
        for(int i = 0; i < myAudioClipStructLib.Count; i++)
        {
            string levelName = myAudioClipStructLib[i].levelName;
            AudioClip audioClip = myAudioClipStructLib[i].audioClip;
            myAudioClipDicLib.Add(levelName, audioClip);
        }
    }
    
}
