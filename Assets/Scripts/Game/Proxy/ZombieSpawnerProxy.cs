using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawnerProxy : IProxy
{
    public List<ZombieBase> autoHitTarget = new List<ZombieBase>();
    public ZombieDataSetting zombieDataSetting;
    public float spawnRate = 0.5f;
    public ZombieBase hitZombie;

    public void SetZombieInit(ZombieDataSetting zombieDataSetting)
    {
        this.zombieDataSetting = zombieDataSetting;
        listener.BroadCast(ZombieSpawnerEvent.ON_ZOMBIE_INIT);
        listener.BroadCast(ZombieSpawnerEvent.START_SPAWN_ZOMBIE);

    }
    public void AddAutoHitTarget(ZombieBase zombieBase)
    {
        if (!autoHitTarget.Contains(zombieBase))
        {
            autoHitTarget.Add(zombieBase);
        }
    }
    public void RemoveAutoHitTarget(ZombieBase zombieBase)
    {
        if (autoHitTarget.Contains(zombieBase))
        {
            autoHitTarget.Remove(zombieBase);
        }
    }
    public ZombieBase GetRamdomHitTarget()
    {
        if (autoHitTarget.Count == 0)
        {
            return null;
        }
        int randomIndex = Random.Range(0, autoHitTarget.Count);
        return autoHitTarget[randomIndex];
    }

    public void SetHitZombie(ZombieBase zombieBase)
    {
        this.hitZombie = zombieBase;
        listener.BroadCast(ZombieSpawnerEvent.ON_ZOMBIE_HIT);
    }
}
