using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionCounter : MonoBehaviour
{
    private void OnDestroy()
    {
        FindObjectOfType<NecroMancerBoss>().minionCounter--;
    }
}
