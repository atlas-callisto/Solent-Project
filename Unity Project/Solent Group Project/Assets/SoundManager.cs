﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private List<string> sfxNameList;
    [SerializeField] private List<AudioClip> sfxFileList;
    private Dictionary<string, AudioClip> sfxCollection = new Dictionary<string, AudioClip>();

    public static SoundManager mySoundManager;
    // public GameObject sfxPrefab;

    AudioSource myAudioSource; // Use Later Maybe

    // Start is called before the first frame update
    void Awake()
    {
        mySoundManager = this;
        myAudioSource = GetComponent<AudioSource>();
    }
    void Start()
    {
        for (int i = 0; i < sfxNameList.Count; i++)
        {
            sfxCollection.Add(sfxNameList[i], sfxFileList[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySFX(string clipName)
    {
        if (sfxCollection.ContainsKey(clipName))
        {
            //AudioSource SFX = Instantiate(sfxPrefab).GetComponent<AudioSource>();
            //SFX.PlayOneShot(sfxCollection[clipName]);
            //Destroy(SFX.gameObject, sfxCollection[clipName].length);
            AudioSource.PlayClipAtPoint(sfxCollection[clipName], this.gameObject.transform.position);
        }
    }
}
