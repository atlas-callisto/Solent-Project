using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonacleHintText : MonoBehaviour
{
    public GameManager GM;

    // I had to add both enter & exit, because it wasn't always being detected by OnEnter
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GM.moonsEyeMonacle)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (GM.moonsEyeMonacle)
        {
            gameObject.SetActive(false);
        }
    }
}
