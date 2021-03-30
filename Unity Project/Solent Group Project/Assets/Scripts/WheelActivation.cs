using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelActivation : MonoBehaviour, Interactable
{
    private Animator myAnimator;

    private void Start()
    {
        myAnimator = GetComponent<Animator>();
    }
    public void Interact()
    {
        myAnimator.SetTrigger("Interact");
    }
}
