using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// This is called AirTraderHint but it's actually to check the monacle. Don't care. Will fix later.
public class AirTreaderHintUI : MonoBehaviour
{
    public GameManager GM;
    public Text HintText;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GM.moonsEyeMonacle)
        {
            HintText.enabled = false;
        }
        else HintText.enabled = true;
    }
}
