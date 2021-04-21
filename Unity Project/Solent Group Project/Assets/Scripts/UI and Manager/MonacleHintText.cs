using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonacleHintText : MonoBehaviour
{
    public GameManager GM;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GM.moonsEyeMonacle)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }
}
