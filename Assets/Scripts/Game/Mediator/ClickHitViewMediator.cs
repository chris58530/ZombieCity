using UnityEngine;
using Zenject;

public class ClickHitViewMediator : IMediator
{
    [Inject] private ClickHitProxy clickHitProxy;
    private ClickHitView view;
    public override void Register(IView view)
    {
        this.view = view as ClickHitView;
    }

    public void OnEnableClickController()
    {
        view.OnEnableClickController();
    }
    public void OnClickZombie(ZombieBase zombie)
    {
        clickHitProxy.HitZombie(zombie);
    }
    public void OnClickSurvivor(SurvivorBase survivor)
    {
        clickHitProxy.HitSurvivor(survivor);
    }
}
