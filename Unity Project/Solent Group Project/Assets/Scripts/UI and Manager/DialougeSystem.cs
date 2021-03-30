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
    private bool doOnce = true; // Stops this script from getting called multiple times

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
        if (!doOnce) return;
        doOnce = false;
        gameObject.SetActive(true);
        Player.playerisTalking = true;
        npcImage.sprite = image;
        npcNameText.text = npcName;
        foreach(string dialouge in dialouges) dialougeList.Add(dialouge);
        dialougeText.text = dialougeList[currentDialougePage];
        currentDialougePage = 0; //page number starts from 0
        totalPages = dialougeList.Count - 1;
    }
    public void CloseDialougeBox()
    {
        Player.playerisTalking = false;
        currentDialougePage = 0;
        dialougeList.Clear();
        doOnce = true;
        gameObject.SetActive(false);
    }

}
