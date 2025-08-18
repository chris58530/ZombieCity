using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Zenject;


public class ZombieSpawnerView : MonoBehaviour, IView
{
    [Inject] private ZombieSpawnerViewMediator mediator;
    [SerializeField] private Vector2 spawnPosX; //左右
    [SerializeField] private Vector2 spawnPosY;// 上下多少範圍
    private Tween spawnTween;
    private Dictionary<int, ZombieManager> zombiesManager = new Dictionary<int, ZombieManager>();
    public void Awake()
    {
        InjectService.Instance.Inject(this);
    }
    private void OnEnable()
    {
        mediator.Register(this);
    }
    private void OnDisable()
    {
        mediator.DeRegister(this);
    }
    public void Initialize(SafeZombieDataSetting data)
    {
        if (data == null || data.zombieData == null || data.zombieData.Length == 0)
        {
            Debug.LogWarning("No zombie data available to spawn.");
            return;
        }

        foreach (var zombieData in data.zombieData)
        {
            SafeZombieBase zombiePrefab = zombieData.zombieInfo.zombieBasePrefab;

            if (zombieData.isLock)
            {
                continue;
            }
            int zombId = zombieData.zombieInfo.zombieBasePrefab.id;
            if (zombiesManager.ContainsKey(zombId))
            {
                Debug.Log($"ZombieManager with id {zombId} already exists. Skipping initialization.");
                continue;
            }
            int hp = zombieData.hp;
            ZombieManager zombieManager = new GameObject("ZombieManager_" + zombId).AddComponent<ZombieManager>();
            zombieManager.isAutoHitTarget = AddAutoHitTarget;
            zombieManager.spawnPosX = spawnPosX;
            zombieManager.spawnPosY = spawnPosY;
            zombieManager.InitZombie(zombiePrefab, hp, (zombie) =>
              {
                  mediator.RequestGetMoney(zombieData.money, zombie.transform);
                  Debug.Log("Zombie " + " is dead.");
              });

            zombieManager.transform.SetParent(transform);
            zombiesManager.Add(zombieData.zombieInfo.zombieBasePrefab.id, zombieManager);
        }
    }
    public void StartSpanwZombie()
    {
        spawnTween = DOVirtual.DelayedCall(1f, () =>
        {
            if (zombiesManager.Count == 0)
            {
                Debug.LogWarning("No zombie managers available.");
                return;
            }

            Debug.Log("Start spawning zombies.");
            var keys = new List<int>(zombiesManager.Keys);
            int spawnId = keys[UnityEngine.Random.Range(0, keys.Count)];
            zombiesManager[spawnId].SpawnZombie();

        }).SetLoops(-1, LoopType.Restart);
    }
    public void StopSpawnZombie()
    {
        Debug.Log("Stop spawning zombies.");
        spawnTween?.Kill();
        foreach (var manager in zombiesManager.Values)
        {
            manager.ResetView();
        }
    }
    public void OnZombieHit(SafeZombieBase zombie)
    {
        foreach (var manager in zombiesManager.Values)
        {
            if (manager == null)
            {
                Debug.LogWarning("ZombieManager is null.");
                continue;
            }
            manager.HitZombie(zombie);
        }
    }
    public void AddAutoHitTarget(SafeZombieBase zombieBase, bool isTarget)
    {
        mediator.AddAutoHitTarget(zombieBase, isTarget);
    }
    public void SetBattleData(int hp, float atk)
    {

    }
}
