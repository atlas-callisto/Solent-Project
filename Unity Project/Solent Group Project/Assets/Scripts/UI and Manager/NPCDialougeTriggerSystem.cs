using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialougeTriggerSystem : MonoBehaviour , Interactable
{
    [SerializeField] Sprite nPCSprite;
    [SerializeField] string nPCName;
    [SerializeField] List<Dialouges> dialougesList = new List<Dialouges>();
    private List<string> dialougeStringList = new List<string>();
    // List of scriptable objs // note to self Use Scriptable objs in the future,
    DialougeSystem dialougeSystemRef;
    private void Awake()
    {
        dialougeSystemRef = FindObjectOfType<DialougeSystem>();
        dialougeStringList = (dialougesList[0].dialouges);        
    }
    public void Interact()
    {
        // Depending on the condition different Dialouges will be prompt into the system.
        print(dialougeSystemRef);
        dialougeSystemRef.AddDialougeInfo(nPCSprite, nPCName, dialougeStringList);
    }

}
