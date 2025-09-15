using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 波次數據 - ScriptableObject，用於儲存一波的所有設定
/// </summary>
[CreateAssetMenu(fileName = "New Wave Data", menuName = "Battle System/Wave Data")]
public class WaveData : ScriptableObject
{
    [Header("波次基本資訊")]
    [Tooltip("波次名稱（用於識別）")]
    public string waveName = "Wave 1";

    [Tooltip("波次描述")]
    [TextArea(2, 4)]
    public string waveDescription = "";

    [Tooltip("波次持續時間（秒）- 0表示無限制")]
    public float waveDuration = 30f;

    [Header("出怪設定")]
    [Tooltip("所有出怪條目")]
    public List<SpawnEntry> spawnEntries = new List<SpawnEntry>();

    [Header("波次完成條件")]
    [Tooltip("完成條件類型")]
    public WaveCompletionType completionType = WaveCompletionType.AllEnemiesDefeated;

    [Tooltip("時間限制完成（僅當完成條件為TimeLimit時有效）")]
    public float timeLimit = 60f;

    [Header("獎勵設定")]
    [Tooltip("完成獎勵金幣")]
    public int rewardCoins = 100;

    [Tooltip("完成獎勵經驗")]
    public int rewardExp = 50;

    /// <summary>
    /// 獲取波次總敵人數量
    /// </summary>
    public int GetTotalEnemyCount()
    {
        int totalCount = 0;
        foreach (var entry in spawnEntries)
        {
            totalCount += entry.spawnCount;
        }
        return totalCount;
    }

    /// <summary>
    /// 獲取波次總持續時間（基於最後一個spawn entry）
    /// </summary>
    public float GetCalculatedDuration()
    {
        float maxTime = 0f;
        foreach (var entry in spawnEntries)
        {
            float entryEndTime = entry.spawnTime + (entry.spawnCount - 1) * entry.spawnInterval;
            if (entryEndTime > maxTime)
                maxTime = entryEndTime;
        }

        // 如果設定了波次持續時間，取較大值
        return waveDuration > 0 ? Mathf.Max(maxTime, waveDuration) : maxTime;
    }

    /// <summary>
    /// 獲取指定時間應該觸發的spawn entries
    /// </summary>
    public List<SpawnEntry> GetEntriesAtTime(float currentTime)
    {
        List<SpawnEntry> entries = new List<SpawnEntry>();

        foreach (var entry in spawnEntries)
        {
            if (Mathf.Approximately(entry.spawnTime, currentTime) ||
                (entry.spawnTime <= currentTime && entry.spawnTime + Time.fixedDeltaTime > currentTime))
            {
                entries.Add(entry);
            }
        }

        return entries;
    }

    /// <summary>
    /// 驗證波次數據的有效性
    /// </summary>
    public bool ValidateWaveData(out string errorMessage)
    {
        errorMessage = "";

        if (spawnEntries.Count == 0)
        {
            errorMessage = "波次沒有任何出怪條目";
            return false;
        }

        for (int i = 0; i < spawnEntries.Count; i++)
        {
            var entry = spawnEntries[i];

            if (entry.enemyPrefab == null)
            {
                errorMessage = $"第 {i + 1} 個出怪條目缺少敵人預製體";
                return false;
            }

            if (entry.spawnCount <= 0)
            {
                errorMessage = $"第 {i + 1} 個出怪條目的生成數量必須大於 0";
                return false;
            }

            if (entry.spawnTime < 0)
            {
                errorMessage = $"第 {i + 1} 個出怪條目的生成時間不能為負數";
                return false;
            }

            if (entry.pathType == PathType.DOTweenPath && entry.doTweenPath == null)
            {
                errorMessage = $"第 {i + 1} 個出怪條目選擇了 DOTweenPath 但未指定路徑";
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// 在 Inspector 中預覽波次資訊
    /// </summary>
    [ContextMenu("Preview Wave Info")]
    public void PreviewWaveInfo()
    {
        Debug.Log($"=== 波次預覽：{waveName} ===");
        Debug.Log($"總敵人數量：{GetTotalEnemyCount()}");
        Debug.Log($"計算持續時間：{GetCalculatedDuration():F1} 秒");
        Debug.Log($"出怪條目數量：{spawnEntries.Count}");

        for (int i = 0; i < spawnEntries.Count; i++)
        {
            var entry = spawnEntries[i];
            Debug.Log($"  條目 {i + 1}: {entry.enemyPrefab?.name} x{entry.spawnCount} 在 {entry.spawnTime}s");
        }
    }
}

/// <summary>
/// 波次完成條件
/// </summary>
public enum WaveCompletionType
{
    AllEnemiesDefeated,  // 所有敵人被擊敗
    TimeLimit,           // 時間限制
    PlayerReachGoal,     // 玩家到達目標
    Custom               // 自定義條件
}