using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using Zenject;

public class ZombieSpawnerViewMediator : IMediator
{
    [Inject] private ZombieSpawnerProxy proxy;
    [Inject] private DropItemProxy dropItemProxy;
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

    public void RequestGetMoney(int money, Transform zombieTransform)
    {
        DropRequest request = new DropRequest(DropItemType.Coin, zombieTransform.position, money);
        dropItemProxy.RequestDropResourceItem(request);

        int getCore = Random.Range(0, 1);
        if(getCore == 0)
        {
            request = new DropRequest(DropItemType.ZombieCore, zombieTransform.position, 1);
            dropItemProxy.RequestDropResourceItem(request);
        }
    }
}
