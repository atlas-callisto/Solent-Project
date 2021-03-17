using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    private Player player;
    public Slider healthBar;
    public Slider wolfBar;
    void Awake()
    {
        player = FindObjectOfType<Player>();
    }

    private void Update()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        healthBar.value = (float)player.currentHealth / (float)player.maxHealth;

        wolfBar.value = (float)player.currentWolfBar / (float)player.maxWolfBar;
    }
}
