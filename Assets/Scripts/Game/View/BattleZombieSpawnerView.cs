using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class BattleZombieSpawnerView : MonoBehaviour, IView
{
    [Zenject.Inject] private BattleZombieSpawnerViewMediator mediator;
    [SerializeField] private BattleZombieSpawnData battleSetting;
    [SerializeField] private List<ZombieBase> zombies;
    [SerializeField] private GameObject root;
    private ZombieManager zombieManager;
    //todo use this to spawn zombies objectpool
    private void Awake()
    {
        InjectService.Instance.Inject(this);
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
    public void StartSpawning()
    {
        Debug.Log("Start Spawning Zombies Battle");
        root.SetActive(true);
        StartCoroutine(SpawnWaves());
    }
    public void SetBattleSetting(BattleZombieSpawnData setting)
    {
        battleSetting = setting;
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
                    Vector2 spawnPos = new Vector2(
                        UnityEngine.Random.Range(battleSetting.spawnLimitX.x, battleSetting.spawnLimitX.y),
                        0f // 固定 Y 軸座標，可依需求調整
                    );

                    ZombieBase prefab = zombies.Find(z => z.id == spawnSetting.zombieType.zombieID);
                    if (prefab != null)
                    {
                        ZombieBase zombie = Instantiate(prefab, spawnPos, Quaternion.identity);
                        zombie.ChangeLayer("Battle");
                    }
                    else
                    {
                        Debug.LogWarning($"Zombie prefab with id {spawnSetting.zombieType.zombieID} not found.");
                    }

                }
            }
        }
    }
}
