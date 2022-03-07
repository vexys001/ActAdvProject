using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PogData", menuName = "ScriptableObjects/PogScriptableObject", order = 1)]
public class PogScriptableObject : ScriptableObject
{
    public bool IsKey;
    public Material Material;
}
