using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialouges")]
public class Dialouges : ScriptableObject
{
    [SerializeField] public List<string> dialouges = new List<string>();
}
