using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //[Header("Player Stats")] // Using static parameters for players at the moment
    //public int playerMaxHealth;
    //public int playerCurrentHealth;
    //public int playerMaxWolfBar;
    //public int playerCurrentWolfBar;

    [Header("Completed Events")]
    public bool werewolfBossDefeated;
    public bool necromancerBossDefeated;
    public bool lordProtectorBossDefeated;
    public bool allGemsCollected;


    [Header("Unlocked Abilities")]
    public bool airTreaders;
    public bool moonsEyeMonacle;
    public static GameManager myGameManager;

    [Header("Progression")]
    public int gemsCollected;
    public int gemsRequired;

    [Header("Boss Arena Mechanic")]
    public bool HasWheelTurned = false;

    private void Awake()
    {
        if (myGameManager != null)
        {
            Destroy(this.gameObject);
            return;
        }
        myGameManager = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public void CheckCollectedGems()
    {
        gemsCollected++;
        if (gemsCollected >= gemsRequired) allGemsCollected = true;
    }

    public enum lockCondition
    {
        WereWolfBossDead,
        NecromancerBossDead,
        LordProtectorDead,
        GemsCollected,
        KeyCollected

    };
    public bool UnLockDoorCondition(lockCondition recievedLockCondition)
    {
        bool unlockdoor = false;
        switch (recievedLockCondition)
        {
            case lockCondition.WereWolfBossDead:
                unlockdoor = werewolfBossDefeated;
                break;

            case lockCondition.NecromancerBossDead:
                unlockdoor = necromancerBossDefeated;
                break;

            case lockCondition.LordProtectorDead:
                unlockdoor = lordProtectorBossDefeated;
                break;

            case lockCondition.GemsCollected:
                unlockdoor = allGemsCollected; //gotta add this
                break;

            case lockCondition.KeyCollected:
                unlockdoor = false; //gotta add this
                break;
        }
        return unlockdoor;

    }


}
