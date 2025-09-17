using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class BattleZombieSpawnerView : MonoBehaviour, IView
{
    [Zenject.Inject] private BattleZombieSpawnerViewMediator mediator;
    private BattleZombieSpawnData battleSetting;
    [SerializeField] private BattleZombieLevelData zombieLevelData;
    [SerializeField] private List<BattleZombieBase> zombies;
    [SerializeField] private GameObject root;
    [SerializeField] private float spawnY;
    private Dictionary<int, BattleZombieManager> zombiesManager = new Dictionary<int, BattleZombieManager>();
    private int totalZombieCount = 0;        // 總殭屍數量
    private int remainingZombieCount = 0;    // 剩餘殭屍數量
    private int deadZombieCount = 0;         // 死亡殭屍數量
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
        OnZombieCountUpdated?.Invoke(remainingZombieCount, deadZombieCount, totalZombieCount);
    }

    // 當殭屍死亡時調用
    private void OnZombieDead(BattleZombieBase zombie)
    {
        deadZombieCount++;
        remainingZombieCount--;

        Debug.Log($"Zombie {zombie.id} is dead. Dead: {deadZombieCount}, Remaining: {remainingZombieCount}");

        // 更新UI顯示
        OnZombieCountUpdated?.Invoke(remainingZombieCount, deadZombieCount, totalZombieCount);

        mediator.RequestUpdateZombieCount(remainingZombieCount);

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
        // battleSetting = battleZombieSpawnData;

        // // 設置總殭屍數量和剩餘數量
        // totalZombieCount = battleSetting.GetAllZombieCount();
        // remainingZombieCount = totalZombieCount;
        // deadZombieCount = 0;

        // Debug.Log($"Start Spawning {totalZombieCount} Zombies for Battle");

        // // 通知UI更新
        // OnZombieCountUpdated?.Invoke(remainingZombieCount, deadZombieCount, totalZombieCount);

        // root.SetActive(true);
        // StartCoroutine(SpawnWaves());
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
        // totalZombieCount = battleSetting.GetAllZombieCount();
    }

    private IEnumerator SpawnWaves()
    {
        // float battleStartTime = Time.time;

        // foreach (var wave in battleSetting.waveSettings)
        // {
        //     // 等待直到遊戲總時長達到 triggerSecond
        //     while (Time.time - battleStartTime < wave.triggerSecond)
        //     {
        //         yield return null;
        //     }

        //     // 生成殭屍
        //     foreach (var spawnSetting in wave.zombieSpwnSettings)
        //     {
        //         for (int i = 0; i < spawnSetting.zombieCount; i++)
        //         {
        //             float spawnPosX = UnityEngine.Random.Range(battleSetting.spawnLimitX.x, battleSetting.spawnLimitX.y);
        //             Vector2 spawnPos = new Vector2(spawnPosX, spawnY);

        //             int zombieId = spawnSetting.zombieType.zombieID;

        //             if (!zombiesManager.ContainsKey(zombieId))
        //                 continue;

        //             int hp = zombieLevelData.GetHp(zombieId);
        //             float atk = zombieLevelData.GetAttack(zombieId);
        //             IHittable hittable = mediator.GetCampCar();

        //             zombiesManager[zombieId].SpawnBattleZombie(spawnPos, hittable, hp, atk, OnZombieDead);
        //         }
        //     }
        // }
        yield return null;
    }
}