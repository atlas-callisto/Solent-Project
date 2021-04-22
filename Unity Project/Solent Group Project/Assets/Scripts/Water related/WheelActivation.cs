using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelActivation : MonoBehaviour, Interactable
{
    private Animator myAnimator;
    public GameObject interactableHintText;
    public static bool WheelActivated;

    public void Start()
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
