using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransformHintUX : MonoBehaviour
{
    public GameManager GM;
    public Text HintText;

    void Start()
    {
        HintText.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            HintText.enabled = true;
        }
    }
}
