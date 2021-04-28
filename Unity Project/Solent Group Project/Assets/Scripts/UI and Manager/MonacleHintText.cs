using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonacleHintText : MonoBehaviour
{
    public GameManager GM;
    public Text MyText;

    // I have to do this per update because it's just fucking dumb
    void Update()
    {
        if(GM.moonsEyeMonacle)
        {
            MyText.enabled = false;
        }
    }
}
