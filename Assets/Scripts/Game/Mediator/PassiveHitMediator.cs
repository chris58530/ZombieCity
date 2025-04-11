using Zenject;

public class PassiveHitMediator : IMediator
{
    [Zenject.Inject] private PassiveHitProxy proxy;
    [Inject]private ZombieSpawnerProxy zombieSpawnerProxy;
    private PassiveHitView view;

    public override void Register(IView view)
    {
        base.Register(view);
        this.view = view as PassiveHitView;
    }
    public override void DeRegister(IView view)
    {
        base.DeRegister(view);
    }
    [Listener(PassiveHitEvent.ON_OPEN_PASSIVE_HIT)]
    public void OnOpenPassiveHit()
    {
        view.SetShootStart(proxy.shootRate);
    }
    public ZombieBase GetHitTarget()
    {
        return zombieSpawnerProxy.GetRamdomHitTarget();
    }
}
