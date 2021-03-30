using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterFlowActivation : WheelActivation, Interactable
{
    private Animator myAnimator;


    public void Update()
    {
        if (WheelActivated == true)
        {
        myAnimator.SetTrigger("WaterFlow");
        }
        
    }
}
