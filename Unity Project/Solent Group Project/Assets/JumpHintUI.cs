using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JumpHintUI : MonoBehaviour
{
    public GameManager GM;
    public Text JumpHint;

    void Start()
    {
        JumpHint.enabled = false;
    }

    void Update()
    {
        if (GM.airTreaders)
        {
            JumpHint.enabled = true;
        }
        else
        {
            JumpHint.enabled = false;
        }
    }
}
