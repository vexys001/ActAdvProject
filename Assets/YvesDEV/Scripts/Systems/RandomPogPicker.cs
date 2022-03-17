using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPogPicker: MonoBehaviour
{
    public static RandomPogPicker Instance;
    public PogScriptableObject[] NormalPogsSOs;

    public void Awake()
    {
        Instance = this;
    }

    public PogScriptableObject RandomNormalPogSO()
    {
        return NormalPogsSOs[Mathf.FloorToInt(Random.Range(0, NormalPogsSOs.Length))];
    }
}