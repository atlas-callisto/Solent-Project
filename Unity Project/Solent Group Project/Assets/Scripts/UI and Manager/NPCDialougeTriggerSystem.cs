using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialougeTriggerSystem : MonoBehaviour , Interactable
{
    Sprite nPCSprite;
    string nPCName;
    [SerializeField] List<Dialouges> dialougesList = new List<Dialouges>();
    [SerializeField] List<string> dialougeStringList = new List<string>();
    // List of scriptable objs // note to self Use Scriptable objs in the future,
    DialougeSystem dialougeSystemRef;
    
    private void Awake()
    {
        dialougeSystemRef = FindObjectOfType<DialougeSystem>();
    }
    private void Start()
    {
        dialougeStringList = (dialougesList[0].dialouges);
        
    }
    public void Interact()
    {
        // Depending on the condition different Dialouges will be prompt into the system.
        dialougeSystemRef.AddDialougeInfo(nPCSprite, nPCName, dialougeStringList);
    }

}
