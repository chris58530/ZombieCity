using System;
using System.Collections.Generic;
using DG.Tweening;
using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;


public class ZombieSpawnerView : MonoBehaviour, IView
{
    [Inject] private ZombieSpawnerViewMediator mediator;
    [SerializeField] private Vector2 spawnPosX; //左右
    [SerializeField] private Vector2 spawnPosY;// 上下多少範圍
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

            if (GameDefine.IsLock(zombieData.isLock))
            {
                continue;
            }
            int zombId = zombieData.zombieInfo.zombieBasePrefab.id;
            int hp = zombieData.hp;
            ZombieManager zombieManager = new GameObject("ZombieManager_" + zombId).AddComponent<ZombieManager>();
            zombieManager.spawnPosX = spawnPosX;
            zombieManager.spawnPosY = spawnPosY;
            zombieManager.InitZombie(zombiePrefab, hp, () =>
            {
                Debug.Log("Zombie " + zombId + " is dead.");
            });

            zombieManager.transform.SetParent(transform);
            zombiesManager.Add(zombieData.zombieInfo.zombieBasePrefab.id, zombieManager);
        }
    }
    public void OnZombieSpawned(int id)
    {
        if (zombiesManager.ContainsKey(id))
        {
            zombiesManager[id].SpawnZombie();
        }
        else
        {
            Debug.LogWarning("Zombie ID not found in manager.");
        }
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
}
public class ZombieManager : MonoBehaviour
{
    public Vector2 spawnPosX; //左右
    public Vector2 spawnPosY;// 上下多少
    private Action deadCallback;
    private int poolCount = 8;
    private int zombieHp;
    private PoolManager poolManager;
    private Dictionary<ZombieBase, int> zombieHpDic = new();
    private Dictionary<ZombieBase, Tween> zombieMoveTween = new();

    public void InitZombie(ZombieBase zombie, int hp, Action deadCallback)
    {
        this.deadCallback = deadCallback;
        this.zombieHp = hp;
        poolManager = new GameObject("ZombiePool_").AddComponent<PoolManager>();
        poolManager.transform.SetParent(transform);
        poolManager.RegisterPool(zombie, poolCount, poolManager.transform);
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

        zombie.SetDead(() => //表演資料
        {
            deadCallback?.Invoke();//噴錢啥的 邏輯資料
            ResetZombie(zombie);
        });

    }
    public void ResetZombie(ZombieBase zombie)
    {

        zombieHpDic.Remove(zombie);
        zombieMoveTween.Remove(zombie);
        poolManager.Despawn(zombie);
    }
    public void SpawnZombie()
    {
        ZombieBase zombie = poolManager.Spawn<ZombieBase>();
        zombieHpDic.Add(zombie, zombieHp);

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
        zombieMoveTween.Add(zombie, zombie.transform.DOMoveX(xPos, speed).SetEase(Ease.Linear).OnComplete(() =>
        {
            zombieMoveTween[zombie].Kill();
            ResetZombie(zombie);
        }));

    }

}