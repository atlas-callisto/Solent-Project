using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirTreads : MonoBehaviour , Interactable
{
    public void Interact()
    {
        FindObjectOfType<Player>().doubleJumpSkillAcquired = true;
        Destroy(gameObject);
    }
}
