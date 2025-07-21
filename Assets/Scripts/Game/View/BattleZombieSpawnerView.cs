using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class BattleZombieSpawnerView : MonoBehaviour, IView
{
    [Zenject.Inject] private BattleZombieSpawnerViewMediator mediator;
    [SerializeField] private BattleZombieSpawnData battleSetting;
    [SerializeField] private BattleZombieLevelData zombieLevelData;
    [SerializeField] private List<ZombieBase> zombies;
    [SerializeField] private GameObject root;
    [SerializeField] private float spawnY;
    private Dictionary<int, ZombieManager> zombiesManager = new Dictionary<int, ZombieManager>();

    private void Awake()
    {
        InjectService.Instance.Inject(this);
        InitializeAllZombies();
        ResetView();
    }
    private void OnEnable()
    {
        mediator.Register(this);
    }
    private void OnDisable()
    {
        mediator.DeRegister(this);
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
            ZombieManager manager = new GameObject("ZombieManager_" + zombId).AddComponent<ZombieManager>();
            manager.InitZombie(zombie, 1, (zombie) =>
              {
                  //   mediator.RequestGetMoney(zombieData.money, zombie.transform);
                  Debug.Log("Zombie " + " is dead.");
              });

            manager.transform.SetParent(transform);
            zombiesManager.Add(zombId, manager);
        }
    }

    public void ResetView()
    {
        root.SetActive(false);
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
                    zombiesManager[zombieId].SpawnBattleZombie(spawnPos, hittable, hp, atk);
                }
            }
        }
    }
}
