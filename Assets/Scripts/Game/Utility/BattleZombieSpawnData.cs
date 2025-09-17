using System;
using UnityEngine;

[Serializable]
public class BattleZombieSpawnData : ScriptableObject
{
    public WaveSetting[] waveSettings;

    public int GetSingleWaveZombieCount(int waveIndex)
    {
        if (waveIndex < 0 || waveIndex >= waveSettings.Length)
        {
            Debug.LogWarning("Wave index out of range");
            return 0;
        }

        int count = 0;
        foreach (var spawnData in waveSettings[waveIndex].spawnData)
        {
            count++;
        }
        return count;
    }

    public int GetAllZombieCount()
    {
        int totalCount = 0;
        foreach (var wave in waveSettings)
        {
            totalCount += wave.spawnData.Length;
        }
        return totalCount;
    }
}
[Serializable]
public class WaveSetting
{
    public ZombieSpawnData[] spawnData;
    public float spawnSecond; //本波次第幾秒生成

}
[Serializable]
public class ZombieSpawnData
{
    public BattleZombieBase zombiePrefab; //殭屍
    public int level; //殭屍等級 初始化時進去運算血量與攻擊力

}

