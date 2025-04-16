using UnityEngine;
using Zenject;

public class FloorViewMedaitor : IMediator
{
    [Inject] private FloorProxy floorProxy;
    [Inject]private PlayerDataProxy playerDataProxy;
    private FloorView floorView;

    public override void DeRegister(IView view)
    {
        base.DeRegister(view);
    }
    public override void Register(IView view)
    {
        base.Register(view);
        floorView = view as FloorView;
    }
    [Listener(FloorEvent.ON_FLOOR_INIT)]
    public void Init()
    {
        FloorDataSetting floorDataSetting = floorProxy.floorDataSetting;
        FloorProductData floorProductData = playerDataProxy.playerData.floorProductData;
        double logOutTime = playerDataProxy.playerData.logOutData.logOutTime;
        floorView.InitFloor(floorDataSetting,floorProductData,logOutTime);
    }
    public void OnInitCompelet(){
        listener.BroadCast(FloorEvent.ON_FLOOR_INIT_COMPELET);
    }
    [Listener(FloorEvent.ON_UPDATE_COLLIDER)]
    public void SetCollider()
    {
        bool isEnabledCollider = floorProxy.isEnabledCollider;
        floorView.SetCollider(isEnabledCollider);
    }

}
