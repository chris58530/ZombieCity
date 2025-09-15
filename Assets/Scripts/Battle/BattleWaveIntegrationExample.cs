using UnityEngine;

/// <summary>
/// 新舊系統整合範例 - 展示如何將新的波次系統與現有的 BattleZombieSpawnerView 整合
/// </summary>
public class BattleWaveIntegrationExample : MonoBehaviour
{
    [Header("新系統組件")]
    [SerializeField] private MultiWaveManager multiWaveManager;
    [SerializeField] private WaveRunner waveRunner;
    [SerializeField] private EnemyPool enemyPool;

    [Header("現有系統組件")]
    [SerializeField] private BattleZombieSpawnerView zombieSpawner;

    [Header("整合設定")]
    [SerializeField] private bool useNewWaveSystem = true;
    [SerializeField] private bool hybridMode = false; // 混合模式：同時使用兩套系統

    private void Start()
    {
        // 根據設定選擇使用哪套系統
        if (useNewWaveSystem)
        {
            SetupNewWaveSystem();
        }
        else
        {
            SetupLegacySystem();
        }

        if (hybridMode)
        {
            SetupHybridMode();
        }
    }

    /// <summary>
    /// 設置新的波次系統
    /// </summary>
    private void SetupNewWaveSystem()
    {
        if (multiWaveManager == null)
        {
            Debug.LogError("MultiWaveManager 未設置");
            return;
        }

        // 註冊事件監聽
        multiWaveManager.OnWaveStarted += OnNewWaveStarted;
        multiWaveManager.OnWaveCompleted += OnNewWaveCompleted;
        multiWaveManager.OnAllWavesCompleted += OnAllNewWavesCompleted;
        multiWaveManager.OnStatusChanged += OnNewSystemStatusChanged;

        // 禁用舊系統
        if (zombieSpawner != null)
        {
            zombieSpawner.enabled = false;
        }

        Debug.Log("新波次系統已啟用");
    }

    /// <summary>
    /// 設置舊系統
    /// </summary>
    private void SetupLegacySystem()
    {
        if (zombieSpawner == null)
        {
            Debug.LogError("BattleZombieSpawnerView 未設置");
            return;
        }

        // 啟用舊系統
        zombieSpawner.enabled = true;

        // 禁用新系統
        if (multiWaveManager != null)
        {
            multiWaveManager.enabled = false;
        }

        Debug.Log("舊系統已啟用");
    }

    /// <summary>
    /// 設置混合模式
    /// </summary>
    private void SetupHybridMode()
    {
        // 在混合模式下，可以讓新系統管理波次邏輯
        // 但使用舊系統的實際生成機制

        if (waveRunner != null)
        {
            // 重寫 WaveRunner 的敵人生成邏輯，使其調用舊系統
            waveRunner.OnEnemySpawned += OnNewSystemEnemySpawned;
        }

        Debug.Log("混合模式已啟用");
    }

    /// <summary>
    /// 新系統波次開始回調
    /// </summary>
    private void OnNewWaveStarted(int waveIndex, WaveData waveData)
    {
        Debug.Log($"🚀 新系統：開始第 {waveIndex + 1} 波 - {waveData.waveName}");

        // 可以在這裡觸發 UI 更新、音效等
        UpdateUI($"波次 {waveIndex + 1}: {waveData.waveName}");
    }

    /// <summary>
    /// 新系統波次完成回調
    /// </summary>
    private void OnNewWaveCompleted(int waveIndex, WaveData waveData)
    {
        Debug.Log($"✅ 新系統：完成第 {waveIndex + 1} 波 - {waveData.waveName}");

        // 波次完成後的處理邏輯
        GiveWaveRewards(waveData);
    }

    /// <summary>
    /// 所有波次完成回調
    /// </summary>
    private void OnAllNewWavesCompleted()
    {
        Debug.Log("🎉 所有波次完成！");

        // 戰鬥勝利邏輯
        OnBattleVictory();
    }

    /// <summary>
    /// 新系統狀態變化回調
    /// </summary>
    private void OnNewSystemStatusChanged(string status)
    {
        Debug.Log($"📊 系統狀態：{status}");

        // 更新狀態顯示
        UpdateStatusDisplay(status);
    }

    /// <summary>
    /// 新系統敵人生成回調（混合模式用）
    /// </summary>
    private void OnNewSystemEnemySpawned(WaveRunner runner, SpawnEntry entry, GameObject enemy)
    {
        Debug.Log($"👾 生成敵人：{enemy.name}");

        // 在混合模式下，可以在這裡添加與舊系統的兼容邏輯
        // 例如：將敵人註冊到舊系統的管理器中
        if (hybridMode && zombieSpawner != null)
        {
            // 假設舊系統有一個註冊敵人的方法
            // zombieSpawner.RegisterEnemy(enemy);
        }
    }

    /// <summary>
    /// 給予波次獎勵
    /// </summary>
    private void GiveWaveRewards(WaveData waveData)
    {
        if (waveData == null) return;

        // 給予金幣和經驗
        Debug.Log($"💰 獲得獎勵：金幣 +{waveData.rewardCoins}, 經驗 +{waveData.rewardExp}");

        // 這裡應該調用實際的獎勵系統
        // GameManager.Instance.AddCoins(waveData.rewardCoins);
        // GameManager.Instance.AddExp(waveData.rewardExp);
    }

    /// <summary>
    /// 戰鬥勝利處理
    /// </summary>
    private void OnBattleVictory()
    {
        Debug.Log("🏆 戰鬥勝利！");

        // 戰鬥勝利後的處理
        // 例如：顯示勝利畫面、保存進度、回到主選單等

        // 停用系統
        if (multiWaveManager != null)
        {
            multiWaveManager.enabled = false;
        }

        if (zombieSpawner != null)
        {
            zombieSpawner.enabled = false;
        }
    }

    /// <summary>
    /// 更新 UI 顯示
    /// </summary>
    private void UpdateUI(string waveInfo)
    {
        // 這裡應該調用實際的 UI 更新邏輯
        Debug.Log($"🖥️ UI 更新：{waveInfo}");
    }

    /// <summary>
    /// 更新狀態顯示
    /// </summary>
    private void UpdateStatusDisplay(string status)
    {
        // 這裡應該調用實際的狀態顯示邏輯
        Debug.Log($"📱 狀態顯示：{status}");
    }

    /// <summary>
    /// 手動開始戰鬥（供外部調用）
    /// </summary>
    [ContextMenu("開始戰鬥")]
    public void StartBattle()
    {
        if (useNewWaveSystem && multiWaveManager != null)
        {
            multiWaveManager.StartAllWaves();
        }
        else if (zombieSpawner != null)
        {
            // 假設舊系統有開始方法
            // zombieSpawner.StartBattle();
            Debug.Log("啟動舊系統戰鬥（需要實現具體邏輯）");
        }
    }

    /// <summary>
    /// 停止戰鬥
    /// </summary>
    public void StopBattle()
    {
        if (multiWaveManager != null)
        {
            multiWaveManager.StopAllWaves();
        }

        if (zombieSpawner != null)
        {
            zombieSpawner.ResetView();
        }

        Debug.Log("戰鬥已停止");
    }

    /// <summary>
    /// 暫停/恢復戰鬥
    /// </summary>
    public void PauseBattle(bool pause)
    {
        if (multiWaveManager != null)
        {
            multiWaveManager.PauseWaves(pause);
        }

        Debug.Log(pause ? "戰鬥已暫停" : "戰鬥已恢復");
    }

    private void OnDestroy()
    {
        // 清理事件監聽
        if (multiWaveManager != null)
        {
            multiWaveManager.OnWaveStarted -= OnNewWaveStarted;
            multiWaveManager.OnWaveCompleted -= OnNewWaveCompleted;
            multiWaveManager.OnAllWavesCompleted -= OnAllNewWavesCompleted;
            multiWaveManager.OnStatusChanged -= OnNewSystemStatusChanged;
        }

        if (waveRunner != null)
        {
            waveRunner.OnEnemySpawned -= OnNewSystemEnemySpawned;
        }
    }
}