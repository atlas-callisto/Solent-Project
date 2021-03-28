using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialougeSystem : MonoBehaviour
{
    [SerializeField] Image npcImage;
    [SerializeField] Text npcNameText;
    [SerializeField] Text dialougeText;
    [SerializeField] List<string> dialougeList = new List<string>();
    [SerializeField] private int currentDialougePage = 0;
    [SerializeField] private int totalPages = 2;

    private void Start()
    {
        gameObject.SetActive(false);
    }
    private void Update()
    {
        SrollDialougeBox();
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
        dialougeText.text = dialougeList[currentDialougePage];
    }
    public void AddDialougeInfo(Sprite image, string npcName, List<string> dialouges)
    {
        gameObject.SetActive(true);
        Player.playerisTalking = true;
        npcImage.sprite = image;
        npcNameText.text = npcName;
        foreach(string dialouge in dialouges) dialougeList.Add(dialouge);
        dialougeText.text = dialougeList[currentDialougePage];
        currentDialougePage = 0; //page number starts from 0
        totalPages = dialougeList.Count;
    }
    public void CloseDialougeBox()
    {
        Player.playerisTalking = false;
        currentDialougePage = 0;
        dialougeList.Clear();
        gameObject.SetActive(false);
    }

}
