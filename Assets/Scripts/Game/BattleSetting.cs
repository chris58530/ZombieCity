using System;
using UnityEngine;

[Serializable]
public class BattleSetting : ScriptableObject
{
    public float roadMeter;
    public GameObject[] roadPrefab;
    public TriggerZombieSetting[] triggerZombieSettings;
}
