using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialougeTriggerSystem : MonoBehaviour , Interactable
{
    Sprite nPCSprite;
    string nPCName;
    [SerializeField] List<string> dialougeList = new List<string>();
    // List of scriptable objs // note to self Use Scriptable objs in the future,
    DialougeSystem dialougeSystemRef;

    private void Awake()
    {
        dialougeSystemRef = FindObjectOfType<DialougeSystem>();
    }
    private void Start()
    {
        //for(int i = 0; i < dialougeList.Count; i++ )
        //{
        //    print("Dialouge added");
        //}
    }
    public void Interact()
    {
        // Depending on the condition different Dialouges will be prompt into the system.
        dialougeSystemRef.AddDialougeInfo(nPCSprite, nPCName, dialougeList);
    }
}
