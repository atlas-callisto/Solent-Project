﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTrap : MonoBehaviour
{
    [Header("References")]
    public AudioSource ShootSound;
    public GameObject Arrow;

    [Header("Config")]
    // Use these to tune the spawn position of the arrow
    public float xArrowSpawnOffset;
    public float yArrowSpawnOffset;
    public float ReloadCooldown;
    public int Ammo;

    [Header("Internal")]
    private bool IsReloading;

    // Start is called before the first frame update
    void Start()
    {
        // This is a failsafe incase you forget to assign values in the inspector
        if(Ammo <= 0)
        {
            Ammo = 3;
        }
        // This is a failsafe incase you forget to assign values in the inspector
        if (ReloadCooldown <= 0)
        {
            ReloadCooldown = 3;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if(!IsReloading)
            {
                StartCoroutine(Shoot());
            }
        }
    }

    private IEnumerator Shoot()
    {
        if (Ammo > 0)
        {
            ShootSound.Play();

            Instantiate(Arrow, transform.position + new Vector3(xArrowSpawnOffset, yArrowSpawnOffset, 0), transform.rotation);
            Ammo--;
        }

        IsReloading = true;
        yield return new WaitForSeconds(ReloadCooldown);
        IsReloading = false;
    }
}
