using UnityEngine;
using Zenject;
public class SurvivorViewMediator : IMediator
{
    [Inject] private SurvivorProxy proxy;
    [Inject] private FloorProxy floorProxy;
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
        view.InitSurvivor(survivorDataSetting);
    }


    [Listener(SurvivorEvent.ON_SURVIVOR_MOVE)]
    public void MoveSurvivor()
    {
       
    }
    public void SetSurvivorNextPosition()
    {
        listener.BroadCast(SurvivorEvent.ON_SURVIVOR_MOVE);
    }
}
