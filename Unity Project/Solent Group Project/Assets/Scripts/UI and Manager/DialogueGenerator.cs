using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueGenerator : MonoBehaviour
{
    [Header("References")]
    public Text DialogueText;
    public Text NameText;
    public Text PageNumberText;
    public GameObject DialoguePanel;

    [Header("Config")]
    public string NPC_Name;

    [Header("Sentences")]
    public string Page1_Dialogue;
    public string Page2_Dialogue;
    public string Page3_Dialogue;

    [Header("Internal")]
    private int CurrentPage;
    private string CurrentSentence;

    // Start is called before the first frame update
    void Start()
    {
        CurrentPage = 1;
        NameText.text = NPC_Name;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Input.GetButtonDown("Interact"))
        {
            ReadNextPage();
        }
    }

    private void ReadNextPage()
    {
        DialoguePanel.SetActive(true);

        if (CurrentPage > 3)
        {
            CloseDialogue();
        }

        // Yes, this is all hard coded. If you want more pages, 
        // I'll make a system that lets you have as many as you want.
        // But it's tedious as hell and I don't want to do it, if not needed.
        switch (CurrentPage)
        {
            case 1:
                CurrentSentence = Page1_Dialogue;
                break;

            case 2:
                CurrentSentence = Page2_Dialogue;
                break;

            case 3:
                CurrentSentence = Page3_Dialogue;
                break;

            default:
                CurrentSentence = "Error, invalid page assigned";
                break;
        }  UpdatePage();
    }

    private void UpdatePage()
    {
        if (CurrentSentence == "")
        {
            CloseDialogue();
        }
        else 
        {
            DialogueText.text = CurrentSentence;
            PageNumberText.text = "Page " + CurrentPage;
            CurrentPage++;
        }
    }

    private void CloseDialogue()
    {
        CurrentPage = 1;
        DialoguePanel.SetActive(false);
    }
}
