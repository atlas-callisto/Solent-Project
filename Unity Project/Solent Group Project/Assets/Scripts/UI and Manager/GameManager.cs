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

    public bool CheckCollectedGems()
    {
        if (gemsCollected >= gemsRequired)
        {
            return allGemsCollected = true;
        }
        else return allGemsCollected = false;
    }

}
