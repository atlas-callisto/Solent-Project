using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelActivation : MonoBehaviour, Interactable
{
    private Animator myAnimator;
    public bool WheelActivated;

    private void Start()
    {
        myAnimator = GetComponent<Animator>();
    }
    public void Interact()
    {
        myAnimator.SetTrigger("Interact");
        WheelActivated = true;
        var fountains = FindObjectsOfType<WaterFlowActivation>();
        foreach(var fountain in fountains)
        {
            fountain.ActivateWaterFlowAnimation();
        }
    }
}
