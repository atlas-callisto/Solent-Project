using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirTreads : MonoBehaviour , Interactable
{
    public GameObject interactableHintText;
    private void Awake()
    {
        if (GameManager.myGameManager.airTreaders) Destroy(gameObject);
    }
    public void Interact()
    {
        GameManager.myGameManager.airTreaders = true;
        FindObjectOfType<Player>().doubleJumpSkillAcquired = true;
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            interactableHintText.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            interactableHintText.SetActive(false);
        }
    }
}
