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
        RecycleAll();
        root.SetActive(false);
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
        Debug.Log("Start Spawning Zombies Battle");
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
            manager.InitZombie(zombie, 1, (zombie) =>
              {
                  //   mediator.RequestGetMoney(zombieData.money, zombie.transform);
                  Debug.Log("Zombie " + " is dead.");
              });

            manager.transform.SetParent(transform);
            zombiesManager.Add(zombId, manager);
        }
    }


    private IEnumerator SpawnWaves()
    {
        foreach (var wave in battleSetting.waveSettings)
        {
            yield return new WaitForSeconds(wave.triggerSecond);

            foreach (var spawnSetting in wave.zombieSpwnSettings)
            {
                for (int i = 0; i < spawnSetting.zombieCount; i++)
                {
                    float spanwPosX = UnityEngine.Random.Range(battleSetting.spawnLimitX.x, battleSetting.spawnLimitX.y);
                    Vector2 spawnPos = new Vector2(spanwPosX, spawnY);
                    int zombieId = spawnSetting.zombieType.zombieID;
                    int hp = zombieLevelData.GetHp(zombieId);
                    float atk = zombieLevelData.GetAttack(zombieId);
                    IHittable hittable = mediator.GetCampCar();

                    // 防止字典鍵不存在的錯誤
                    if (!zombiesManager.ContainsKey(zombieId))
                    {
                        Debug.LogWarning($"找不到 ID 為 {zombieId} 的殭屍管理器。請確保該殭屍在 zombies 列表中並正確初始化。");
                        continue; // 跳過這個殭屍的生成
                    }

                    zombiesManager[zombieId].SpawnBattleZombie(spawnPos, hittable, hp, atk);
                }
            }
        }
    }
}
