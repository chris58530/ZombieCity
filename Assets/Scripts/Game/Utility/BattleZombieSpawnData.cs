using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Battle Zombie Spawn Data", menuName = "ZombieCity/Battle Zombie Spawn Data")]
[Serializable]
public class BattleZombieSpawnData : ScriptableObject
{
    [Header("波次設定")]
    [SerializeField] private WaveSetting[] waveSettings;

    public WaveSetting[] WaveSettings => waveSettings;

    [Space(10)]
    [Header("預覽資訊 (自動計算)")]
    [TextArea(2, 4)]
    public string previewInfo = "波次數量和殭屍總數會在編輯時自動更新";

    private void OnValidate()
    {
        if (waveSettings != null)
        {
            int totalWaves = waveSettings.Length;
            int totalZombies = GetAllZombieCount();
            previewInfo = $"總波次數: {totalWaves}\n總殭屍數: {totalZombies}\n\n" +
                         GetWavesSummary();
        }
    }

    private string GetWavesSummary()
    {
        if (waveSettings == null || waveSettings.Length == 0)
            return "尚未設定波次";

        string summary = "";
        for (int i = 0; i < waveSettings.Length; i++)
        {
            var wave = waveSettings[i];
            int zombieCount = wave.SpawnData?.Length ?? 0;
            summary += $"第{i + 1}波: {zombieCount}隻殭屍\n";
        }
        return summary;
    }

    public int GetSingleWaveZombieCount(int waveIndex)
    {
        if (waveIndex < 0 || waveIndex >= waveSettings.Length)
        {
            Debug.LogWarning("Wave index out of range");
            return 0;
        }

        int count = 0;
        foreach (var spawnData in waveSettings[waveIndex].SpawnData)
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
            totalCount += wave.SpawnData.Length;
        }
        return totalCount;
    }
}
[Serializable]
public class WaveSetting
{
    [Header("此波次的殭屍設定")]
    [SerializeField] private ZombieSpawnData[] spawnData;

    public ZombieSpawnData[] SpawnData => spawnData;

    [Space(5)]
    [TextArea(1, 2)]
    public string waveNote = "可在此加入波次備註";
}

[Serializable]
public class ZombieSpawnData
{
    [Header("殭屍設定")]
    public BattleZombieBase zombiePrefab; //殭屍

    [Space(5)]
    [Header("生成時機與等級")]
    [Range(0f, 30f)]
    public float spawnSecond; //本波次第幾秒生成

    [Range(1, 10)]
    public int level = 1; //殭屍等級 初始化時進去運算血量與攻擊力

    [Space(5)]
    [TextArea(1, 1)]
    public string zombieNote = "殭屍備註";
}

