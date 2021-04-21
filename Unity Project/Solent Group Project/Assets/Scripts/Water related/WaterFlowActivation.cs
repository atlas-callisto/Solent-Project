using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterFlowActivation : MonoBehaviour
{
    private Animator myAnimator;

    void Start()
    {
        myAnimator = GetComponent<Animator>();
    }
    public void ActivateWaterFlowAnimation()
    {
        myAnimator.SetTrigger("WaterFlow");
    }
}
