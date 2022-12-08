using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct WaveSequence
{
    public GameObject s_prefab_enemy;
    public int s_number;
    public float s_delay;
    // add manual position later to create special effects
}

[Serializable]
public struct Wave
{
    public WaveSequence[] s_sequences;
}

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/WavesScriptableObject", order = 1)]
public class Waves : ScriptableObject
{
    public List<Wave> waves = new List<Wave>();
}
