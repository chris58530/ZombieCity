using System;
using UnityEngine;

[Serializable]
public class BattleZombieSpawnData : ScriptableObject
{
    public WaveSetting[] waveSettings;

}
[Serializable]
public class WaveSetting
{
    public ZombieSpwnSetting[] zombieSpwnSettings;

}
[Serializable]
public class ZombieSpwnSetting
{
    public ZombieSpawnData spawnData;

    public float spawnSecond; //本波次第幾秒生成
}
[Serializable]
public class ZombieSpawnData
{
    public BattleZombieBase zombiePrefab; //殭屍
    public int level; //殭屍等級 初始化時進去運算血量與攻擊力

}

