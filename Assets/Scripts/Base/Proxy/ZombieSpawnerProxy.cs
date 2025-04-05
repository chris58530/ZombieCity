using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawnerProxy : IProxy
{
    public List<ZombieBase> zombies = new List<ZombieBase>();
    public ZombieDataSetting zombieDataSetting;
    public int spawnId;
    public float spawnRate = 0.5f;
    public void SetZombieInit(ZombieDataSetting zombieDataSetting)
    {
        this.zombieDataSetting = zombieDataSetting;
        listener.BroadCast(ZombieSpawnerEvent.ON_ZOMBIE_INIT);

    }
    public void OnSpawnZombie(int id)
    {
        LogService.Instance.Log($"Zombie ID: {id}");

        this.spawnId = id;
        listener.BroadCast(ZombieSpawnerEvent.ON_ZOMBIE_SPAWN);
    }
    public void AddZombie(ZombieBase zombieBase)
    {
        zombies.Add(zombieBase);
    }
}
