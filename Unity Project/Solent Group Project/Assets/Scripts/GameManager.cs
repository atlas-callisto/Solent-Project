using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    //bool gameOver = false;
    [SerializeField] Text healtText = null; //Slider?????
    [SerializeField] Text staminaText = null; //Slider?????
    [SerializeField] Text ammoText = null; //Text??

    [SerializeField] int health = 5;
    [SerializeField] float stamina = 5;
    [SerializeField] int bulletAmmo = 0;
       
    // Start is called before the first frame update
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

    // Update is called once per frame
    void Update()
    {
        UpdateDisplay();
    }
    private void UpdateDisplay()
    {
        return; // currently disabled.
        healtText.text = "Health: " + health.ToString();
        staminaText.text = "Stamina: " + stamina.ToString();
        ammoText.text = "Bullet: " + bulletAmmo.ToString();
    }

    public void CheckLevelCompleteStatus()
    {
        
    }
    
}
