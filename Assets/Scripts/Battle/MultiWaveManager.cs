using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 多波次管理器 - 管理多個波次的順序執行
/// </summary>
public class MultiWaveManager : MonoBehaviour
{
    [Header("波次設定")]
    [SerializeField] private List<WaveData> waves = new List<WaveData>();
    [SerializeField] private float wavePauseDuration = 3f; // 波次間的暫停時間

    [Header("執行設定")]
    [SerializeField] private WaveRunner waveRunner;
    [SerializeField] private EnemyPool enemyPool;
    [SerializeField] private DOTweenPathManager pathManager;

    [Header("狀態")]
    [SerializeField] private int currentWaveIndex = -1;
    [SerializeField] private bool isRunning = false;
    [SerializeField] private bool isPaused = false;

    // 事件
    public System.Action<int, WaveData> OnWaveStarted;
    public System.Action<int, WaveData> OnWaveCompleted;
    public System.Action OnAllWavesCompleted;
    public System.Action<string> OnStatusChanged;

    // 屬性
    public int CurrentWaveIndex => currentWaveIndex;
    public int TotalWaves => waves.Count;
    public bool IsRunning => isRunning;
    public bool IsPaused => isPaused;
    public WaveData CurrentWave => currentWaveIndex >= 0 && currentWaveIndex < waves.Count ? waves[currentWaveIndex] : null;
    public float Progress => waves.Count > 0 ? (float)(currentWaveIndex + 1) / waves.Count : 0f;

    private void Awake()
    {
        // 自動尋找組件
        if (waveRunner == null)
            waveRunner = FindFirstObjectByType<WaveRunner>();

        if (enemyPool == null)
            enemyPool = FindFirstObjectByType<EnemyPool>();

        if (pathManager == null)
            pathManager = FindFirstObjectByType<DOTweenPathManager>();        // 檢查必要組件
        if (waveRunner == null)
        {
            Debug.LogError("找不到 WaveRunner 組件！");
        }
    }

    private void Start()
    {
        // 註冊事件
        if (waveRunner != null)
        {
            waveRunner.OnWaveCompleted += OnWaveRunnerCompleted;
            waveRunner.OnWaveStarted += OnWaveRunnerStarted;
        }

        // 預載所有波次需要的敵人
        PreloadAllEnemies();
    }

    /// <summary>
    /// 開始所有波次
    /// </summary>
    [ContextMenu("Start All Waves")]
    public void StartAllWaves()
    {
        if (isRunning)
        {
            Debug.LogWarning("波次已在執行中");
            return;
        }

        if (waves.Count == 0)
        {
            Debug.LogError("沒有設定任何波次");
            return;
        }

        isRunning = true;
        isPaused = false;
        currentWaveIndex = -1;

        UpdateStatus("開始多波次戰鬥");

        // 開始第一波
        StartNextWave();
    }

    /// <summary>
    /// 停止所有波次
    /// </summary>
    public void StopAllWaves()
    {
        if (!isRunning)
            return;

        isRunning = false;
        isPaused = false;

        if (waveRunner != null)
        {
            waveRunner.StopWave();
        }

        UpdateStatus("停止多波次戰鬥");

        currentWaveIndex = -1;
    }

    /// <summary>
    /// 暫停/恢復波次
    /// </summary>
    public void PauseWaves(bool pause)
    {
        if (!isRunning)
            return;

        isPaused = pause;

        if (waveRunner != null)
        {
            waveRunner.PauseWave(pause);
        }

        UpdateStatus(pause ? "暫停波次" : "恢復波次");
    }

    /// <summary>
    /// 跳過當前波次
    /// </summary>
    public void SkipCurrentWave()
    {
        if (!isRunning || isPaused)
            return;

        if (waveRunner != null)
        {
            waveRunner.StopWave();
        }

        UpdateStatus("跳過當前波次");

        // 立即開始下一波
        StartNextWave();
    }

    /// <summary>
    /// 開始下一波
    /// </summary>
    private void StartNextWave()
    {
        currentWaveIndex++;

        if (currentWaveIndex >= waves.Count)
        {
            // 所有波次完成
            CompleteAllWaves();
            return;
        }

        WaveData nextWave = waves[currentWaveIndex];

        if (nextWave == null)
        {
            Debug.LogError($"第 {currentWaveIndex + 1} 波的數據為空");
            StartNextWave(); // 跳過空波次
            return;
        }

        UpdateStatus($"準備開始第 {currentWaveIndex + 1} 波：{nextWave.waveName}");

        // 波次間暫停
        if (currentWaveIndex > 0 && wavePauseDuration > 0)
        {
            Invoke(nameof(StartCurrentWave), wavePauseDuration);
        }
        else
        {
            StartCurrentWave();
        }
    }

    /// <summary>
    /// 開始當前波次
    /// </summary>
    private void StartCurrentWave()
    {
        if (!isRunning || currentWaveIndex < 0 || currentWaveIndex >= waves.Count)
            return;

        WaveData currentWave = waves[currentWaveIndex];

        UpdateStatus($"開始第 {currentWaveIndex + 1} 波：{currentWave.waveName}");

        // 啟動波次執行器
        if (waveRunner != null)
        {
            waveRunner.StartWave(currentWave);
        }

        OnWaveStarted?.Invoke(currentWaveIndex, currentWave);
    }

    /// <summary>
    /// 波次執行器開始回調
    /// </summary>
    private void OnWaveRunnerStarted(WaveRunner runner)
    {
        Debug.Log($"波次執行器開始：{runner.CurrentWave?.waveName}");
    }

    /// <summary>
    /// 波次執行器完成回調
    /// </summary>
    private void OnWaveRunnerCompleted(WaveRunner runner)
    {
        if (!isRunning)
            return;

        WaveData completedWave = runner.CurrentWave;

        UpdateStatus($"完成第 {currentWaveIndex + 1} 波：{completedWave?.waveName}");

        OnWaveCompleted?.Invoke(currentWaveIndex, completedWave);

        // 開始下一波
        StartNextWave();
    }

    /// <summary>
    /// 完成所有波次
    /// </summary>
    private void CompleteAllWaves()
    {
        isRunning = false;
        isPaused = false;

        UpdateStatus("所有波次完成！");

        OnAllWavesCompleted?.Invoke();

        Debug.Log("🎉 恭喜！所有波次完成！");
    }

    /// <summary>
    /// 預載所有敵人
    /// </summary>
    private void PreloadAllEnemies()
    {
        if (enemyPool == null)
            return;

        HashSet<GameObject> uniqueEnemies = new HashSet<GameObject>();

        // 收集所有不重複的敵人預製體
        foreach (var wave in waves)
        {
            if (wave == null) continue;

            foreach (var entry in wave.spawnEntries)
            {
                if (entry.enemyPrefab != null)
                {
                    uniqueEnemies.Add(entry.enemyPrefab);
                }
            }
        }

        // 預載每種敵人
        foreach (var enemy in uniqueEnemies)
        {
            enemyPool.PreloadPool(enemy, 5); // 每種敵人預載 5 個
        }

        Debug.Log($"預載了 {uniqueEnemies.Count} 種敵人到物件池");
    }

    /// <summary>
    /// 更新狀態訊息
    /// </summary>
    private void UpdateStatus(string message)
    {
        Debug.Log($"[MultiWaveManager] {message}");
        OnStatusChanged?.Invoke(message);
    }

    /// <summary>
    /// 獲取總敵人數量
    /// </summary>
    public int GetTotalEnemyCount()
    {
        int total = 0;
        foreach (var wave in waves)
        {
            if (wave != null)
            {
                total += wave.GetTotalEnemyCount();
            }
        }
        return total;
    }

    /// <summary>
    /// 獲取總預估時間
    /// </summary>
    public float GetTotalEstimatedTime()
    {
        float total = 0f;
        foreach (var wave in waves)
        {
            if (wave != null)
            {
                total += wave.GetCalculatedDuration();
            }
        }

        // 加上波次間的暫停時間
        if (waves.Count > 1)
        {
            total += (waves.Count - 1) * wavePauseDuration;
        }

        return total;
    }

    /// <summary>
    /// 添加波次
    /// </summary>
    public void AddWave(WaveData wave)
    {
        if (wave != null && !waves.Contains(wave))
        {
            waves.Add(wave);
            Debug.Log($"添加波次：{wave.waveName}");
        }
    }

    /// <summary>
    /// 移除波次
    /// </summary>
    public void RemoveWave(WaveData wave)
    {
        if (waves.Remove(wave))
        {
            Debug.Log($"移除波次：{wave.waveName}");
        }
    }

    /// <summary>
    /// 清空所有波次
    /// </summary>
    public void ClearWaves()
    {
        waves.Clear();
        Debug.Log("清空所有波次");
    }

    private void OnDestroy()
    {
        // 清理事件
        if (waveRunner != null)
        {
            waveRunner.OnWaveCompleted -= OnWaveRunnerCompleted;
            waveRunner.OnWaveStarted -= OnWaveRunnerStarted;
        }
    }

    // 編輯器專用方法
#if UNITY_EDITOR
    [ContextMenu("Create Sample Waves")]
    private void CreateSampleWaves()
    {
        // 這個方法在編輯器中用於快速創建範例波次
        Debug.Log("請在專案視窗中手動創建 WaveData 資源檔案");
    }

    [ContextMenu("Validate All Waves")]
    private void ValidateAllWaves()
    {
        for (int i = 0; i < waves.Count; i++)
        {
            var wave = waves[i];
            if (wave == null)
            {
                Debug.LogError($"第 {i + 1} 波數據為空");
                continue;
            }

            string errorMessage;
            if (!wave.ValidateWaveData(out errorMessage))
            {
                Debug.LogError($"第 {i + 1} 波驗證失敗：{errorMessage}");
            }
            else
            {
                Debug.Log($"第 {i + 1} 波驗證通過：{wave.waveName}");
            }
        }
    }
#endif
}