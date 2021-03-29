using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackHintUI : MonoBehaviour
{
    public Text AttackHintText;

    void Start()
    {
        AttackHintText.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        AttackHintText.enabled = true;
    }
}
