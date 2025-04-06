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
            zombiesManager[id].MoveZombie();
        }
        else
        {
            Debug.LogWarning("Zombie ID not found in manager.");
        }
    }
    public void OnZombieHit(int id)
    {

    }
}
public class ZombieManager : MonoBehaviour
{
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
            if (zombieHpDic[zombie] <= 0)
            {
                zombieHpDic.Remove(zombie);
                poolManager.Despawn(zombie);
                zombieMoveTween[zombie].Kill();
                deadCallback?.Invoke();
            }
        }
        else
        {
            Debug.LogWarning("Zombie not found in dictionary.");
        }
    }
    public void MoveZombie()
    {
        ZombieBase zombie = poolManager.Spawn<ZombieBase>();
        zombieHpDic.Add(zombie, zombieHp);
        zombie.transform.position = new Vector2(UnityEngine.Random.Range(-2f, 2f), 2);
        zombieMoveTween.Add(zombie, zombie.transform.DOMoveY(UnityEngine.Random.Range(-6f, -7f), 2f).SetEase(Ease.Linear));

    }

}