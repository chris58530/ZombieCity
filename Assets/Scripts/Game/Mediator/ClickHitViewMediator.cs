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
    public void OnClickSurvivor(SurvivorBase survivor, Vector3 pickPos)
    {
        clickHitProxy.HitSurvivor(survivor, pickPos);
        listener.BroadCast(CameraEvent.CLOSE_CAMERA_SWIPE);

    }
    public void OnClickUp(FloorBase floor)
    {
        clickHitProxy.SetSurvivorFloor(floor);
        listener.BroadCast(CameraEvent.OPEN_CAMERA_SWIPE);
    }
}
