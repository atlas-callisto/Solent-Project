using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicBox : MonoBehaviour
{
    private AudioSource myAudioSource;
    private List<string> ScenesInGame;
    public Dictionary<string, AudioClip> audioDictionaryList= new Dictionary<string, AudioClip>();
    private AudioClip audioClip;
    void Awake()
    {
        SetUpSingleton();
        myAudioSource = GetComponent<AudioSource>();
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string path = SceneUtility.GetScenePathByBuildIndex(i);
            string sceneName = path.Substring(0, path.Length - 6).Substring(path.LastIndexOf('/') + 1);
            ScenesInGame.Add(sceneName);
        }
    }
    void Start()
    {
        myAudioSource.volume = GameManager.myGameManager.GetMusicVolume();
    }
    private void SetUpSingleton()
    {
        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
    public void UpdateMusicVolume(float musicVolume)
    {
        myAudioSource.volume = musicVolume;
    }
}
