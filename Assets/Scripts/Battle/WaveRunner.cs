using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;

/// <summary>
/// 波次執行器 - 負責執行單個波次的時間管理和敵人生成
/// </summary>
public class WaveRunner : MonoBehaviour
{
    [Header("執行狀態")]
    [SerializeField] private WaveData currentWave;
    [SerializeField] private bool isRunning = false;
    [SerializeField] private float waveStartTime;
    [SerializeField] private float currentWaveTime;

    [Header("生成設定")]
    [SerializeField] private Transform spawnParent;
    [SerializeField] private EnemyPool enemyPool;

    [Header("調試")]
    [SerializeField] private bool enableDebugLog = true;

    // 事件
    public System.Action<WaveRunner> OnWaveStarted;
    public System.Action<WaveRunner> OnWaveCompleted;
    public System.Action<WaveRunner, SpawnEntry, GameObject> OnEnemySpawned;

    // 內部狀態
    private List<SpawnEntry> pendingEntries = new List<SpawnEntry>();
    private List<GameObject> spawnedEnemies = new List<GameObject>();
    private Dictionary<SpawnEntry, Coroutine> spawnCoroutines = new Dictionary<SpawnEntry, Coroutine>();

    // 屬性
    public WaveData CurrentWave => currentWave;
    public bool IsRunning => isRunning;
    public float CurrentTime => currentWaveTime;
    public int SpawnedEnemyCount => spawnedEnemies.Count;
    public int AliveEnemyCount => spawnedEnemies.Count(e => e != null && e.activeInHierarchy);

    private void Awake()
    {
        if (spawnParent == null)
            spawnParent = transform;

        if (enemyPool == null)
            enemyPool = FindFirstObjectByType<EnemyPool>();
    }

    private void Update()
    {
        if (isRunning && currentWave != null)
        {
            UpdateWaveTime();
            CheckWaveCompletion();
        }
    }

    /// <summary>
    /// 開始執行波次
    /// </summary>
    public void StartWave(WaveData waveData)
    {
        if (isRunning)
        {
            LogWarning("波次已在執行中，請先停止當前波次");
            return;
        }

        if (waveData == null)
        {
            LogError("波次數據為空");
            return;
        }

        string errorMessage;
        if (!waveData.ValidateWaveData(out errorMessage))
        {
            LogError($"波次數據驗證失敗：{errorMessage}");
            return;
        }

        // 初始化波次
        currentWave = waveData;
        isRunning = true;
        waveStartTime = Time.time;
        currentWaveTime = 0f;

        // 復制出怪條目到待處理列表
        pendingEntries.Clear();
        pendingEntries.AddRange(currentWave.spawnEntries);

        // 清理之前的狀態
        ClearSpawnedEnemies();
        StopAllSpawnCoroutines();

        LogInfo($"開始執行波次：{currentWave.waveName}");
        OnWaveStarted?.Invoke(this);

        // 開始時間循環
        StartCoroutine(WaveTimeLoop());
    }

    /// <summary>
    /// 停止當前波次
    /// </summary>
    public void StopWave()
    {
        if (!isRunning)
            return;

        isRunning = false;
        StopAllSpawnCoroutines();

        LogInfo($"停止波次：{currentWave?.waveName}");

        currentWave = null;
    }

    /// <summary>
    /// 暫停/恢復波次
    /// </summary>
    public void PauseWave(bool pause)
    {
        enabled = !pause;

        if (pause)
        {
            StopAllSpawnCoroutines();
            LogInfo("暫停波次");
        }
        else
        {
            LogInfo("恢復波次");
        }
    }

    /// <summary>
    /// 波次時間循環
    /// </summary>
    private IEnumerator WaveTimeLoop()
    {
        while (isRunning && currentWave != null)
        {
            // 檢查是否有需要觸發的 spawn entry
            CheckAndTriggerSpawnEntries();

            yield return null;
        }
    }

    /// <summary>
    /// 更新波次時間
    /// </summary>
    private void UpdateWaveTime()
    {
        currentWaveTime = Time.time - waveStartTime;
    }

    /// <summary>
    /// 檢查並觸發出怪條目
    /// </summary>
    private void CheckAndTriggerSpawnEntries()
    {
        for (int i = pendingEntries.Count - 1; i >= 0; i--)
        {
            var entry = pendingEntries[i];

            if (currentWaveTime >= entry.spawnTime)
            {
                TriggerSpawnEntry(entry);
                pendingEntries.RemoveAt(i);
            }
        }
    }

    /// <summary>
    /// 觸發單個出怪條目
    /// </summary>
    private void TriggerSpawnEntry(SpawnEntry entry)
    {
        if (spawnCoroutines.ContainsKey(entry))
            return;

        LogInfo($"觸發出怪條目：{entry.enemyPrefab.name} x{entry.spawnCount} 在 {entry.spawnTime}s");

        Coroutine spawnCoroutine = StartCoroutine(SpawnEntryCoroutine(entry));
        spawnCoroutines[entry] = spawnCoroutine;
    }

    /// <summary>
    /// 出怪條目協程
    /// </summary>
    private IEnumerator SpawnEntryCoroutine(SpawnEntry entry)
    {
        for (int i = 0; i < entry.spawnCount; i++)
        {
            SpawnEnemy(entry);

            if (i < entry.spawnCount - 1 && entry.spawnInterval > 0)
            {
                yield return new WaitForSeconds(entry.spawnInterval);
            }
        }

        // 移除已完成的協程
        if (spawnCoroutines.ContainsKey(entry))
        {
            spawnCoroutines.Remove(entry);
        }
    }

    /// <summary>
    /// 生成單個敵人
    /// </summary>
    private void SpawnEnemy(SpawnEntry entry)
    {
        GameObject enemy = null;

        // 從物件池獲取敵人
        if (enemyPool != null)
        {
            enemy = enemyPool.GetEnemy(entry.enemyPrefab);
        }
        else
        {
            // 備用方案：直接實例化
            enemy = Instantiate(entry.enemyPrefab, spawnParent);
        }

        if (enemy == null)
        {
            LogError($"無法生成敵人：{entry.enemyPrefab.name}");
            return;
        }

        // 設置生成位置
        enemy.transform.position = entry.GetSpawnPosition();

        // 應用屬性覆寫
        ApplyEnemyOverrides(enemy, entry);

        // 設置移動路徑
        SetupEnemyPath(enemy, entry);

        // 註冊敵人
        spawnedEnemies.Add(enemy);

        LogInfo($"生成敵人：{entry.enemyPrefab.name} 在位置 {enemy.transform.position}");
        OnEnemySpawned?.Invoke(this, entry, enemy);
    }

    /// <summary>
    /// 應用敵人屬性覆寫
    /// </summary>
    private void ApplyEnemyOverrides(GameObject enemy, SpawnEntry entry)
    {
        // HP覆寫
        if (entry.overrideHP)
        {
            var healthComponent = enemy.GetComponent<IHealth>();
            if (healthComponent != null)
            {
                healthComponent.SetMaxHealth(entry.customHP);
            }
        }

        // 速度覆寫
        if (entry.overrideSpeed)
        {
            var movementComponent = enemy.GetComponent<IMovement>();
            if (movementComponent != null)
            {
                movementComponent.SetSpeedMultiplier(entry.customSpeedMultiplier);
            }
        }

        // 標籤設置
        if (!string.IsNullOrEmpty(entry.enemyTag))
        {
            enemy.tag = entry.enemyTag;
        }
    }

    /// <summary>
    /// 設置敵人移動路徑
    /// </summary>
    private void SetupEnemyPath(GameObject enemy, SpawnEntry entry)
    {
        var tween = entry.CreatePathTween(enemy.transform);

        if (tween != null)
        {
            // 當路徑完成時，可以觸發額外邏輯
            tween.OnComplete(() =>
            {
                OnEnemyPathCompleted(enemy, entry);
            });
        }
        else
        {
            LogWarning($"無法為敵人 {enemy.name} 創建路徑動畫");
        }
    }

    /// <summary>
    /// 敵人路徑完成回調
    /// </summary>
    private void OnEnemyPathCompleted(GameObject enemy, SpawnEntry entry)
    {
        LogInfo($"敵人 {enemy.name} 完成路徑移動");

        // 可以在這裡添加到達終點的邏輯
        // 例如：對玩家造成傷害、回收敵人等
    }

    /// <summary>
    /// 檢查波次完成條件
    /// </summary>
    private void CheckWaveCompletion()
    {
        if (!isRunning || currentWave == null)
            return;

        bool isCompleted = false;

        switch (currentWave.completionType)
        {
            case WaveCompletionType.AllEnemiesDefeated:
                isCompleted = AliveEnemyCount == 0 && pendingEntries.Count == 0 && spawnCoroutines.Count == 0;
                break;

            case WaveCompletionType.TimeLimit:
                isCompleted = currentWaveTime >= currentWave.timeLimit;
                break;

            case WaveCompletionType.PlayerReachGoal:
                // 這裡需要根據實際遊戲邏輯實現
                break;

            case WaveCompletionType.Custom:
                // 這裡可以調用自定義完成條件檢查
                break;
        }

        if (isCompleted)
        {
            CompleteWave();
        }
    }

    /// <summary>
    /// 完成波次
    /// </summary>
    private void CompleteWave()
    {
        LogInfo($"波次完成：{currentWave.waveName}");

        var completedWave = currentWave;
        StopWave();

        OnWaveCompleted?.Invoke(this);
    }

    /// <summary>
    /// 清理生成的敵人
    /// </summary>
    private void ClearSpawnedEnemies()
    {
        foreach (var enemy in spawnedEnemies)
        {
            if (enemy != null)
            {
                if (enemyPool != null)
                {
                    enemyPool.ReturnEnemy(enemy);
                }
                else
                {
                    Destroy(enemy);
                }
            }
        }
        spawnedEnemies.Clear();
    }

    /// <summary>
    /// 停止所有生成協程
    /// </summary>
    private void StopAllSpawnCoroutines()
    {
        foreach (var coroutine in spawnCoroutines.Values)
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }
        }
        spawnCoroutines.Clear();
    }

    /// <summary>
    /// 移除已死亡的敵人
    /// </summary>
    public void RemoveDeadEnemy(GameObject enemy)
    {
        spawnedEnemies.Remove(enemy);

        if (enemyPool != null)
        {
            enemyPool.ReturnEnemy(enemy);
        }
    }

    // 日誌方法
    private void LogInfo(string message)
    {
        if (enableDebugLog)
            Debug.Log($"[WaveRunner] {message}");
    }

    private void LogWarning(string message)
    {
        if (enableDebugLog)
            Debug.LogWarning($"[WaveRunner] {message}");
    }

    private void LogError(string message)
    {
        Debug.LogError($"[WaveRunner] {message}");
    }

    private void OnDestroy()
    {
        StopWave();
    }
}

// 介面定義（如果專案中沒有的話）
public interface IHealth
{
    void SetMaxHealth(int maxHealth);
}

public interface IMovement
{
    void SetSpeedMultiplier(float multiplier);
}