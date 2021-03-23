using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Completed Events")]
    public bool werewolfBossDefeated;
    public bool necromancerBossDefeated;
    public bool lordProtectorBossDefeated;

    [Header("Unlocked Abilities")]
    public bool airTreaders;
    public bool moonsEyeMonacle;


    private void Awake()
    {
        SetUpSingleton();
    }
    private void SetUpSingleton()
    {
        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }    
}
