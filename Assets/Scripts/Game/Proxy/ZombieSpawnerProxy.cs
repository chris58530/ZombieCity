using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawnerProxy : IProxy
{
    public List<SafeZombieBase> autoHitTarget = new List<SafeZombieBase>();
    public SafeZombieDataSetting zombieDataSetting;
    public float spawnRate = 0.5f;
    public SafeZombieBase hitZombie;

    public void SetZombieInit(SafeZombieDataSetting zombieDataSetting)
    {
        this.zombieDataSetting = zombieDataSetting;
        listener.BroadCast(ZombieSpawnerEvent.ON_ZOMBIE_INIT);
        listener.BroadCast(ZombieSpawnerEvent.START_SPAWN_ZOMBIE);

    }
    public void AddAutoHitTarget(SafeZombieBase zombieBase)
    {
        if (!autoHitTarget.Contains(zombieBase))
        {
            autoHitTarget.Add(zombieBase);
        }
    }
    public void RemoveAutoHitTarget(SafeZombieBase zombieBase)
    {
        if (autoHitTarget.Contains(zombieBase))
        {
            autoHitTarget.Remove(zombieBase);
        }
    }
    public SafeZombieBase GetRamdomHitTarget()
    {
        if (autoHitTarget.Count == 0)
        {
            return null;
        }
        int randomIndex = Random.Range(0, autoHitTarget.Count);
        return autoHitTarget[randomIndex];
    }

    public void SetHitZombie(SafeZombieBase zombieBase)
    {
        this.hitZombie = zombieBase;
        listener.BroadCast(ZombieSpawnerEvent.ON_ZOMBIE_HIT);
    }
}
