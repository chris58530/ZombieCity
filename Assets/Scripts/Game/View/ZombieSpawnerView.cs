using System;
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
    public void Initialize(ZombieDataSetting data)
    {
        if (data == null || data.zombieData == null || data.zombieData.Length == 0)
        {
            Debug.LogWarning("No zombie data available to spawn.");
            return;
        }

        foreach (var zombieData in data.zombieData)
        {
            ZombieBase zombiePrefab = zombieData.zombieInfo.zombieBasePrefab;

            if (zombieData.isLock)
            {
                continue;
            }
            int zombId = zombieData.zombieInfo.zombieBasePrefab.id;
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
            Debug.Log("Start spawning zombies.");
            int spawnId = UnityEngine.Random.Range(0, zombiesManager.Count);
            if (zombiesManager.ContainsKey(spawnId))
            {
                zombiesManager[spawnId].SpawnZombie();
            }
        }).SetLoops(-1, LoopType.Restart);
    }
    public void StopSpawnZombie()
    {
        spawnTween?.Kill();
    }
    public void OnZombieHit(ZombieBase zombie)
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
    public void AddAutoHitTarget(ZombieBase zombieBase, bool isTarget)
    {
        mediator.AddAutoHitTarget(zombieBase, isTarget);
    }
}
public class ZombieManager : MonoBehaviour
{
    public Vector2 spawnPosX; //左右
    public Vector2 spawnPosY;// 上下多少
    private Action<ZombieBase> deadCallback;
    private int poolCount = 15;
    private int zombieHp;
    public Action<ZombieBase, bool> isAutoHitTarget;
    private PoolManager poolManager;
    private Dictionary<ZombieBase, int> zombieHpDic = new();
    private Dictionary<ZombieBase, Tween> zombieMoveTween = new();

    public ZombieBase InitZombie(ZombieBase zombie, int hp, Action<ZombieBase> deadCallback)
    {
        this.deadCallback = deadCallback;
        this.zombieHp = hp;
        poolManager = new GameObject("ZombiePool_").AddComponent<PoolManager>();
        poolManager.transform.SetParent(transform);
        poolManager.RegisterPool(zombie, poolCount, poolManager.transform);
        return zombie;
    }
    public void HitZombie(ZombieBase zombie)
    {
        if (zombieHpDic.ContainsKey(zombie))
        {
            zombieHpDic[zombie] -= 1;
            zombie.Hit();
            if (zombieHpDic[zombie] <= 0)
            {
                KillZombie(zombie);
            }
        }
        else
        {
            Debug.LogWarning("Zombie not found in dictionary.");
        }
    }
    public void KillZombie(ZombieBase zombie)
    {
        zombieMoveTween[zombie].Kill();
        AddAutoHitTarget(zombie, false);

        zombie.Kill(() => //表演資料
        {
            deadCallback?.Invoke(zombie);//噴錢啥的 邏輯資料
            ResetZombie(zombie);
        });

    }
    public void ResetZombie(ZombieBase zombie)
    {
        AddAutoHitTarget(zombie, false);

        zombieHpDic.Remove(zombie);
        zombieMoveTween.Remove(zombie);
        poolManager.Despawn(zombie);
    }
    public void SpawnZombie()
    {
        ZombieBase zombie = poolManager.Spawn<ZombieBase>(poolManager.transform);
        zombie.manager = this;
        zombieHpDic.Add(zombie, zombieHp);
        DOVirtual.DelayedCall(3f, () =>
          {
              AddAutoHitTarget(zombie, true);

          }).SetId(zombie.GetHashCode());
        float[] xFloat = new float[2] { spawnPosX.x, spawnPosX.y };
        int x = UnityEngine.Random.Range(0, xFloat.Length);
        Vector2 spawnPos = new Vector2(xFloat[x], UnityEngine.Random.Range(spawnPosY.x, spawnPosY.y));
        zombie.transform.position = spawnPos;
        bool isFlip = GameDefine.IsFlipByWorld(xFloat[x]);
        MoveZombie(zombie, isFlip);
    }
    public void MoveZombie(ZombieBase zombie, bool isFlip)
    {
        zombie.SetFlip(isFlip);
        float speed = UnityEngine.Random.Range(10, 20f);
        float xPos = isFlip ? spawnPosX.x : spawnPosX.y;
        DOVirtual.DelayedCall(8f, () =>
          {
              AddAutoHitTarget(zombie, false);

          }).SetId(zombie.GetHashCode());
        zombieMoveTween.Add(zombie, zombie.transform.DOMoveX(xPos, speed).SetEase(Ease.Linear).OnComplete(() =>
        {

            zombieMoveTween[zombie].Kill();
            ResetZombie(zombie);
        }));

    }
    public void AddAutoHitTarget(ZombieBase zombie, bool isTarget)
    {
        isAutoHitTarget?.Invoke(zombie, isTarget);
        zombie.SetIsTarget(isTarget);
    }

}