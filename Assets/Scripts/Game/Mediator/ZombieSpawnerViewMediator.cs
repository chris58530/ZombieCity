using UnityEngine;
using Zenject;

public class ZombieSpawnerViewMediator : IMediator
{
    [Inject] private ZombieSpawnerProxy proxy;
    private ZombieSpawnerView view;
    public override void Register(IView view)
    {
        base.Register(view);
        this.view = view as ZombieSpawnerView;
    }
    [Listener(ZombieSpawnerEvent.ON_ZOMBIE_INIT)]
    public void OnZombieInit()
    {
        view.Initialize(proxy.zombieDataSetting);
    }
    [Listener(ZombieSpawnerEvent.ON_ZOMBIE_SPAWN)]
    public void OnZombieSpawn()
    {
        int id = proxy.spawnId;
        LogService.Instance.Log($"Zombie ID: {id}");
        view.OnZombieSpawned(id);
    }

    [Listener(ZombieSpawnerEvent.ON_ZOMBIE_HIT)]
    public void OnZombieHit()
    {
        ZombieBase zombie = proxy.hitZombie;
        view.OnZombieHit(zombie);
    }
    public void AddAutoHitTarget(ZombieBase zombieBase, bool isTarget)
    {
        if (isTarget)
            proxy.AddAutoHitTarget(zombieBase);
        else
            proxy.RemoveAutoHitTarget(zombieBase);
    }

}
