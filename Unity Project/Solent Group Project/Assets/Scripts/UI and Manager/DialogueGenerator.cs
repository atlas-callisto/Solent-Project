using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueGenerator : MonoBehaviour
{
    public Text DialogueText;
    public GameObject DialoguePanel;
    public string Dialogue;
    public int DialogueCloseDelay;

    // Start is called before the first frame update
    void Start()
    {
        DialogueText.text = Dialogue;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Input.GetButtonDown("Interact"))
        {
            ReadOutDialogue();
        }
    }

    private void ReadOutDialogue()
    {
        DialoguePanel.SetActive(true);
        StartCoroutine(CloseDialogue());
    }

    private IEnumerator CloseDialogue()
    {
        yield return new WaitForSeconds(DialogueCloseDelay);
        DialoguePanel.SetActive(false);
    }
}
