using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicBox : MonoBehaviour
{
    AudioSource audioSource;
    void Awake()
    {
        SetUpSingleton();
    }
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
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
    public void SetVolume(float volume)
    {
        audioSource.volume = volume;
    }
}
