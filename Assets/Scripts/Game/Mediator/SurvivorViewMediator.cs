using UnityEngine;
using Zenject;
public class SurvivorViewMediator : IMediator
{
    [Inject] private SurvivorProxy proxy;
    [Inject] private FloorProxy floorProxy;
    [Inject] private ClickHitProxy clickHitProxy;
    private SurvivorView view;
    public override void DeRegister(IView view)
    {
        base.DeRegister(view);
    }
    public override void Register(IView view)
    {
        base.Register(view);
        this.view = view as SurvivorView;
    }
    [Listener(SurvivorEvent.ON_SURVIVOR_INIT)]
    public void Init()
    {
        SurvivorDataSetting survivorDataSetting = proxy.survivorDataSetting;
        
        view.InitSurvivor(survivorDataSetting,floorProxy.startFloor);
    }

    [Listener(SurvivorEvent.ON_CLICK_SURVIVOR)]
    public void OnClickSurvivor()
    {
        Vector3 pickPos = clickHitProxy.pickPos;
        view.OnClickSurvivor(proxy.onClickSurvivor, pickPos);

        floorProxy.SetCollider(true);
    }
    public void SetSurvivorNextPosition()
    {
        // listener.BroadCast(SurvivorEvent.ON_SURVIVOR_MOVE);
    }
    [Listener(SurvivorEvent.ON_CLICK_SURVIVOR_COMPLETE)]
    public void OnClickSurvivorComplete()
    {
        FloorBase place = clickHitProxy.clickUpFloor;
        view.OnClickSurvivorComplete(proxy.onClickSurvivor, place);
        floorProxy.SetCollider(false);


    }
}
