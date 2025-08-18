using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleZombieManager : MonoBehaviour
{
    private PoolManager poolManager;
    [SerializeField] private BattleZombieBase zombiePrefab;
    [SerializeField] private Transform zombieParent;
    private HashSet<BattleZombieBase> activeBattleZombies = new HashSet<BattleZombieBase>();
    private int zombieHp;
    public int managerID;
    private int poolCount = 15;

    private Action<BattleZombieBase> deadCallback;
    public void ResetView()
    {
        foreach (var zombie in activeBattleZombies.ToArray())
        {
            if (zombie != null)
            {
                poolManager.Despawn(zombie);
            }
        }
        activeBattleZombies.Clear();
    }

    public BattleZombieBase InitZombie(BattleZombieBase zombie, int hp, Action<BattleZombieBase> deadCallback)
    {
        managerID = zombie.id;
        this.deadCallback = deadCallback;
        this.zombieHp = hp;
        poolManager = new GameObject("ZombiePool_").AddComponent<PoolManager>();
        poolManager.transform.SetParent(transform);
        poolManager.RegisterPool(zombie, poolCount, poolManager.transform);
        return zombie;
    }
    public BattleZombieBase SpawnBattleZombie()
    {
        return poolManager.Spawn<BattleZombieBase>(zombieParent);
    }

    public void ResetBattleZombie(BattleZombieBase zombie)
    {
        poolManager.Despawn(zombie);
    }

    public void SpawnBattleZombie(Vector2 spawnPoint, IHittable campCar, int hp, float atk)
    {
        BattleZombieBase zombie = poolManager.Spawn<BattleZombieBase>(poolManager.transform);
        zombie.SetLayer("Battle");
        zombie.manager = this;
        zombie.hp = hp;
        zombie.deadCallBack = (zombie) =>
        {
            Debug.Log("Zombie is dead.");
        };
        zombie.attack = atk;
        zombie.transform.position = spawnPoint;
        zombie.SetBattleData(campCar);
        activeBattleZombies.Add(zombie);
        zombie.StartMove();
    }
}
