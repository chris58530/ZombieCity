using UnityEngine;
using Zenject;

public class ZombieSpawnerCmd : ICommand
{
    [Inject] private ZombieSpawnerProxy proxy;
    [Inject] private ClickHitProxy clickHitProxy;
    [SerializeField] private SafeZombieDataSetting zombieDataSetting;
    public override void Execute(MonoBehaviour mono)
    {
        isLazy = true;
    }
    [Listener(GameEvent.ON_GAME_STATE_START)]
    public void StartZombieSpawn()
    {
        proxy.SetZombieInit(zombieDataSetting);
    }

    [Listener(GameEvent.ON_GAME_STATE_END)]
    private void StopSpawn()
    {
        listener.BroadCast(ZombieSpawnerEvent.STOP_SPAWN_ZOMBIE);
        SetComplete();
    }
    [Listener(ClickHitEvent.ON_CLICK_ZOMBIE)]
    public void OnClickHitZombie()
    {
        SafeZombieBase zombie = clickHitProxy.hitZombie;
        proxy.SetHitZombie(zombie);
    }

}
