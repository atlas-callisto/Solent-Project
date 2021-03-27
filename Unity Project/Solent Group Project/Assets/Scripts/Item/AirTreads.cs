using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirTreads : MonoBehaviour , Interactable
{
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
}
