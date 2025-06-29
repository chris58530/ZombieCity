using System;
using UnityEngine;

[Serializable]
public class BattleZombieSpawnData : ScriptableObject
{
    public Vector2 spawnLimitX;
    public WaveSetting[] waveSettings;

}
