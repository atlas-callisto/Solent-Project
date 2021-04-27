using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonacleHintText : MonoBehaviour
{
    public GameManager GM;

    // I have to do this per update because it's just fucking dumb
    private void LateUpdate()
    {
        if(GM.moonsEyeMonacle)
        {
            gameObject.SetActive(false);
        }
    }
}
