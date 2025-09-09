using System;
using UnityEngine;

[Serializable]
public class BattleZombieSpawnData : ScriptableObject
{
    public Vector2 spawnLimitX;
    public WaveSetting[] waveSettings;
    public int GetAllZombieCount()
    {
        int total = 0;

        // 防護檢查：確保 waveSettings 不為空
        if (waveSettings == null)
        {
            return 0;
        }

        for (int waveIndex = 0; waveIndex < waveSettings.Length; waveIndex++)
        {
            var wave = waveSettings[waveIndex];

            // 防護檢查：確保 wave 不為空
            if (wave == null)
            {
                continue;
            }

            // 防護檢查：確保 zombieSpwnSettings 不為空
            if (wave.zombieSpwnSettings == null)
            {
                continue;
            }

            int waveTotal = 0;
            for (int settingIndex = 0; settingIndex < wave.zombieSpwnSettings.Length; settingIndex++)
            {
                var spawnSetting = wave.zombieSpwnSettings[settingIndex];

                // 防護檢查：確保 spawnSetting 不為空
                if (spawnSetting == null)
                {
                    continue;
                }

                waveTotal += spawnSetting.zombieCount;
            }

            total += waveTotal;
        }

        return total;
    }

    // 測試和驗證方法
    [ContextMenu("Test GetAllZombieCount")]
    public void TestGetAllZombieCount()
    {
        int result = GetAllZombieCount();

        // 額外驗證：手動計算一次
        int manualCount = 0;

        if (waveSettings != null)
        {
            for (int i = 0; i < waveSettings.Length; i++)
            {
                if (waveSettings[i]?.zombieSpwnSettings != null)
                {
                    for (int j = 0; j < waveSettings[i].zombieSpwnSettings.Length; j++)
                    {
                        if (waveSettings[i].zombieSpwnSettings[j] != null)
                        {
                            manualCount += waveSettings[i].zombieSpwnSettings[j].zombieCount;
                        }
                    }
                }
            }
        }
    }

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

