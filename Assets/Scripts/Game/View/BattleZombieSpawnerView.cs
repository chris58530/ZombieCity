using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class BattleZombieSpawnerView : MonoBehaviour, IView
{
    [Zenject.Inject] private BattleZombieSpawnerViewMediator mediator;

    [Header("Debug觀察用 實際透過Mediator傳入")]
    [SerializeField] private BattleZombieSpawnData battleSetting;
    [SerializeField] private BattleZombieLevelData zombieLevelData;
    [SerializeField] private List<BattleZombieBase> zombies;
    [SerializeField] private GameObject root;
    [Header("生成位置設定")]
    [SerializeField] private float spawnY;
    [SerializeField] private float spawnRangeX = 7f;  // X軸生成範圍
    [SerializeField] private bool useRandomSpawn = true;  // 是否使用隨機生成
    [SerializeField] private float spawnSpacing = 1.5f;  // 同波次殭屍間距
    private Dictionary<int, BattleZombieManager> zombiesManager = new Dictionary<int, BattleZombieManager>();
    private int totalZombieCount = 0;        // 總殭屍數量
    private int remainingZombieCount = 0;    // 剩餘殭屍數量
    private int deadZombieCount = 0;         // 死亡殭屍數量

    // 波次管理
    private int currentWaveZombieCount = 0;  // 當前波次的殭屍數量
    private int currentWaveDeadCount = 0;    // 當前波次已死亡的殭屍數量
    private bool isWaveCompleted = false;    // 當前波次是否完成

    // 事件：當殭屍數量更新時觸發 (剩餘, 死亡, 總數)
    public event Action<int, int, int> OnZombieCountUpdated;
    private void Awake()
    {
        InjectService.Instance.Inject(this);
        InitializeAllZombies();
    }
    private void OnEnable()
    {
        mediator.Register(this);
    }
    private void OnDisable()
    {
        mediator.DeRegister(this);
    }
    public void ResetView()
    {
        StopAllCoroutines();
        battleSetting = null;
        ResetZombieCount();
        RecycleAll();
        root.SetActive(false);
    }

    // 重置殭屍計數
    private void ResetZombieCount()
    {
        totalZombieCount = 0;
        remainingZombieCount = 0;
        deadZombieCount = 0;
        currentWaveZombieCount = 0;
        currentWaveDeadCount = 0;
        isWaveCompleted = false;
        OnZombieCountUpdated?.Invoke(remainingZombieCount, deadZombieCount, totalZombieCount);
    }

    // 當殭屍死亡時調用
    private void OnZombieDead(BattleZombieBase zombie)
    {
        deadZombieCount++;
        remainingZombieCount--;
        currentWaveDeadCount++;

        Debug.Log($"Zombie {zombie.id} is dead. Wave Dead: {currentWaveDeadCount}/{currentWaveZombieCount}, Total Dead: {deadZombieCount}, Remaining: {remainingZombieCount}");

        // 更新UI顯示
        OnZombieCountUpdated?.Invoke(remainingZombieCount, deadZombieCount, totalZombieCount);

        mediator.RequestUpdateZombieCount(remainingZombieCount);

        // 檢查當前波次是否完成
        if (currentWaveDeadCount >= currentWaveZombieCount)
        {
            isWaveCompleted = true;
            Debug.Log($"當前波次完成！所有 {currentWaveZombieCount} 隻殭屍已死亡");
        }

        // 檢查是否所有殭屍都死了
        if (remainingZombieCount <= 0)
        {
            OnAllZombiesDead();
        }
    }

    // 當所有殭屍都死亡時觸發
    private void OnAllZombiesDead()
    {
        Debug.Log("All zombies are dead! Battle completed!");

        // 發送戰鬥結束事件 - 如果你有事件系統的話
        // GameEventManager.Instance.BroadCast("ON_BATTLE_COMPLETED");

        // 或者通過 Mediator 處理
        // mediator.OnBattleCompleted();
    }

    public void RecycleAll()
    {
        foreach (var manager in zombiesManager.Values)
        {
            if (manager != null)
            {
                manager.ResetView();
            }
        }
    }
    [ContextMenu("Start Spawning")]
    public void StartSpawning(BattleZombieSpawnData battleZombieSpawnData)
    {
        battleSetting = battleZombieSpawnData;

        // 設置總殭屍數量和剩餘數量
        totalZombieCount = battleSetting.GetAllZombieCount();
        remainingZombieCount = totalZombieCount;
        deadZombieCount = 0;

        Debug.Log($"Start Spawning {totalZombieCount} Zombies for Battle");

        // 通知UI更新
        OnZombieCountUpdated?.Invoke(remainingZombieCount, deadZombieCount, totalZombieCount);

        root.SetActive(true);
        StartCoroutine(SpawnWaves());
    }
    public void InitializeAllZombies()
    {
        foreach (var zombie in zombies)
        {
            int zombId = zombie.id;
            if (zombiesManager.ContainsKey(zombId))
            {
                Debug.Log($"ZombieManager with id {zombId} already exists. Skipping initialization.");
                continue;
            }
            BattleZombieManager manager = new GameObject("ZombieManager_" + zombId).AddComponent<BattleZombieManager>();
            manager.InitZombie(zombie, 1, OnZombieDead);

            manager.transform.SetParent(transform);
            zombiesManager.Add(zombId, manager);
        }
    }

    public void SetCounter()
    {
        totalZombieCount = battleSetting.GetAllZombieCount();
    }

    private IEnumerator SpawnWaves()
    {
        if (battleSetting == null || battleSetting.WaveSettings == null)
        {
            Debug.LogError("[BattleSpawner] BattleZombieSpawnData 或 waveSettings 為空");
            yield break;
        }

        for (int waveIndex = 0; waveIndex < battleSetting.WaveSettings.Length; waveIndex++)
        {
            var wave = battleSetting.WaveSettings[waveIndex];

            // 重置當前波次的計數
            currentWaveZombieCount = wave.SpawnData.Length;
            currentWaveDeadCount = 0;
            isWaveCompleted = false;

            Debug.Log($"[BattleSpawner] 開始第 {waveIndex + 1} 波，共 {currentWaveZombieCount} 隻殭屍");

            float waveStartTime = Time.time;

            // 為當前波次的每隻殭屍安排生成時間
            StartCoroutine(SpawnZombiesInWave(wave, waveStartTime, waveIndex + 1));

            // 等待當前波次的所有殭屍死亡
            yield return new WaitUntil(() => isWaveCompleted);

            Debug.Log($"[BattleSpawner] 第 {waveIndex + 1} 波完成！準備下一波...");
        }

        Debug.Log("[BattleSpawner] 所有波次生成完畢！戰鬥結束！");
    }

    private IEnumerator SpawnZombiesInWave(WaveSetting wave, float waveStartTime, int waveNumber)
    {
        foreach (var spawnData in wave.SpawnData)
        {
            // 等待直到達到該殭屍的生成時間
            while (Time.time - waveStartTime < spawnData.spawnSecond)
            {
                yield return null;
            }

            // 生成殭屍
            int zombieId = spawnData.zombiePrefab.id;
            Vector2 spawnPos = new Vector2(0, spawnY);

            int hp = zombieLevelData.GetHp(zombieId) * spawnData.level;
            float atk = zombieLevelData.GetAttack(zombieId) * spawnData.level;

            IHittable hittable = mediator.GetCampCar();

            zombiesManager[zombieId].SpawnBattleZombie(spawnPos, hittable, hp, atk, OnZombieDead);

            Debug.Log($"[BattleSpawner] 第 {waveNumber} 波在 {spawnData.spawnSecond} 秒生成殭屍：ID={zombieId}, Level={spawnData.level}");
        }
    }
}