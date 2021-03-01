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

    public Image healthBarImage;
    public Image wolfBarImage;
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
        if (player.currentHealth <= 0) healthBarImage.enabled = false;
        else healthBarImage.enabled = true;

        wolfBar.value = (float)player.currentWolfBar / (float)player.maxWolfBar;
        if (player.currentWolfBar <= 0) wolfBarImage.enabled = false;
        else wolfBarImage.enabled = true;
    }
}
