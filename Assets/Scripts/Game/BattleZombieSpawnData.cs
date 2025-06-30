using System;
using UnityEngine;

[Serializable]
public class BattleZombieSpawnData : ScriptableObject
{
    public Vector2 spawnLimitX;
    public WaveSetting[] waveSettings;

}
[Serializable]
public class WaveSetting
{
    public float triggerSecond;
    public ZombieSpwnSetting[] zombieSpwnSettings;

}
[Serializable]
public class ZombieSpwnSetting
{
    public ZombieSpawnData zombieType;

    public int zombieCount;
}
[Serializable]
public class ZombieSpawnData
{
    public int zombieID;
    public int level;

}

