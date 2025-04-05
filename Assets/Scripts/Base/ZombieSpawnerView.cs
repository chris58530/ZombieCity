using System;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using UnityEngine;
using Zenject;


public class ZombieSpawnerView : MonoBehaviour, IView
{
    [Inject] private ZombieSpawnerViewMediator mediator;
    private ZombieManager zombieManager;
    private Dictionary<int, PoolManager> zombiesPool = new Dictionary<int, PoolManager>();
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

        zombieManager = new GameObject("ZombieManager").AddComponent<ZombieManager>();
        zombieManager.transform.SetParent(transform);


        foreach (var zombieData in data.zombieData)
        {
            ZombieBase zombiePrefab = zombieData.zombieInfo.zombieBasePrefab;

            if (GameDefine.IsLock(zombieData.isLock))
            {
                continue;
            }

            PoolManager pool = new GameObject("ZombiePool_" + zombieData.zombieInfo.zombieBasePrefab.id).AddComponent<PoolManager>();
            pool.transform.SetParent(zombieManager.transform);
            pool.RegisterPool(zombiePrefab, 10, pool.transform);
            zombiesPool.Add(zombieData.zombieInfo.zombieBasePrefab.id, pool);
        }
    }
    public void SpawnZombie(int id)
    {
        Debug.Log($"Zombie ID: {id}");

        if (!zombiesPool.TryGetValue(id, out var pool))
        {
            Debug.LogError($"No zombie ID: {id}");
            return;
        }
        ZombieBase zombie = pool.Spawn<ZombieBase>();
        zombie.transform.position = new Vector2(UnityEngine.Random.Range(-2f, 2f), -10);
        zombieManager.MoveZombie(zombie);
    }
}
public class ZombieManager : MonoBehaviour
{
    public void MoveZombie(ZombieBase zombie, Action callback = null)
    {

    }
}