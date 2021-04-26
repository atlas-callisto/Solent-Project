using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialougeSystem : MonoBehaviour
{
    [SerializeField] Image npcImage;
    [SerializeField] Text npcNameText;
    [SerializeField] Text dialougeText;
    private List<string> dialougeList = new List<string>();
    private int currentDialougePage = 0;
    private int totalPages = 2;
    private bool NPCTalking = false;
    private bool doOnce = true; // Stops this script from getting called multiple times
    private IEnumerator typeWriter;
    AudioSource myAudioSource;
    private void Start()
    {
        myAudioSource = GetComponent<AudioSource>();
        gameObject.SetActive(false);
    }
    private void Update()
    {
        SrollDialougeBox();
        PlaySFX();
    }

    private void SrollDialougeBox()
    {
        if(Input.GetKeyDown(KeyCode.D)) // Right page
        {
            currentDialougePage = Mathf.Clamp(currentDialougePage +1, 0 , totalPages);
            PlayDialouge();
        }
        if(Input.GetKeyDown(KeyCode.A)) // Left page
        {
            currentDialougePage = Mathf.Clamp(currentDialougePage -1, 0, totalPages);
            PlayDialouge();
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            CloseDialougeBox();
        }
    }

    private void PlayDialouge()
    {
        string currentPage = dialougeList[currentDialougePage];
        if (typeWriter != null) StopCoroutine(typeWriter);
        typeWriter = StylizedDialougeTypeWriterSystem(currentPage);
        dialougeText.text = "";
        StartCoroutine(typeWriter);
    }
    public void AddDialougeInfo(Sprite image, string npcName, List<string> dialouges)
    {
        if (!doOnce) return;
        doOnce = false;
        gameObject.SetActive(true);
        Player.playerisTalking = true;
        npcImage.sprite = image;
        npcNameText.text = npcName;
        foreach(string dialouge in dialouges) dialougeList.Add(dialouge);
        currentDialougePage = 0; //page number starts from 0
        totalPages = dialougeList.Count - 1;
        PlayDialouge();
    }
    public void CloseDialougeBox()
    {
        Player.playerisTalking = false;
        NPCTalking = false;
        currentDialougePage = 0;
        dialougeList.Clear();
        doOnce = true;
        gameObject.SetActive(false);
    }

    IEnumerator StylizedDialougeTypeWriterSystem(string sentence)
    {
        NPCTalking = true;
        foreach (char alpha in sentence)
        {
            myAudioSource.pitch = Random.Range(0f, 1f);
            dialougeText.text += alpha;
            yield return new WaitForSeconds(0.05f);
        }
        NPCTalking = false;
    }
    private void PlaySFX()
    {
        myAudioSource.volume = GameManager.myGameManager.GetSFXVolume();
        if(NPCTalking && !myAudioSource.isPlaying)myAudioSource.Play();
        else myAudioSource.Stop();
    }

}
