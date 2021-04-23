using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class DoorLock : MonoBehaviour
{
    [Header("Parameters")]
    public bool doorIsUnLocked = true;
    public string lockedReasonMessage;
    public GameManager.lockCondition myLockCondition;

    [Header("Object Ref")]
    public GameObject interactMsg;
    public GameObject doorLockedMsg;   

    private LevelTransistion myLevelTransistionScript;

    private void Start()
    {
        myLevelTransistionScript = GetComponent<LevelTransistion>();
        myLevelTransistionScript.enabled = doorIsUnLocked;
        doorLockedMsg.GetComponentInChildren<TextMeshProUGUI>().text = lockedReasonMessage;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (!doorIsUnLocked) doorIsUnLocked = GameManager.myGameManager.UnLockDoorCondition(myLockCondition);
            myLevelTransistionScript.enabled = doorIsUnLocked;
            if (doorIsUnLocked)interactMsg.SetActive(true);
            if (!doorIsUnLocked) doorLockedMsg.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            interactMsg.SetActive(false);
            doorLockedMsg.SetActive(false);
        }
    }
}
