using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    public Slider healthBar;
    public Slider wolfBar;
    private void Update()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        healthBar.value = (float)Player.currentHealth / (float)Player.maxHealth;

        wolfBar.value = (float)Player.currentWolfBar / (float)Player.maxWolfBar;
    }
}
