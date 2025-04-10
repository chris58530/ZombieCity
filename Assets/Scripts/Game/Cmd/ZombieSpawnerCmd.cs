using UnityEngine;
using Zenject;

public class ZombieSpawnerCmd : ICommand
{
    [Inject] private ZombieSpawnerProxy proxy;
    [Inject] private ClickHitProxy clickHitProxy;
    [SerializeField] private ZombieDataSetting zombieDataSetting;
    public override void Execute(MonoBehaviour mono)
    {
        isLazy = true;

    }
    [Listener(GameEvent.ON_INIT_GAME)]
    public void StartZombieSpawn()
    {
        proxy.SetZombieInit(zombieDataSetting);
    }
    [Listener(DebugEvent.ON_ZOMBIE_SPAWN)]
    public void SpawnZombie()
    {
        int data = Random.Range(0, zombieDataSetting.zombieData.Length);

        ZombieData zombieData = zombieDataSetting.zombieData[data];

        proxy.OnSpawnZombie(zombieData.zombieInfo.zombieBasePrefab.id);
    }
    [Listener(ClickHitEvent.ON_CLICK_ZOMBIE)]
    public void OnClickHitZombie()
    {
        ZombieBase zombie = clickHitProxy.hitZombie;

        proxy.SetHitZombie(zombie);
    }

}
