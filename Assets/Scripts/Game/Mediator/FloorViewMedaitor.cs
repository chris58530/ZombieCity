using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class FloorViewMediator : IMediator
{
    [Inject] private FloorProxy floorProxy;
    [Inject] private PlayerDataProxy playerDataProxy;
    private FloorView floorView;
    private FloorProductData floorProductData;
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
        floorProductData = playerDataProxy.playerData.floorProductData;
        double logOutTime = playerDataProxy.playerData.logOutData.logOutTime;
        floorView.InitFloor(floorDataSetting, floorProductData, logOutTime);
    }
    public void OnInitCompelet()
    {
        listener.BroadCast(FloorEvent.ON_FLOOR_INIT_COMPELET);
    }
    [Listener(FloorEvent.ON_UPDATE_COLLIDER)]
    public void SetCollider()
    {
        bool isEnabledCollider = floorProxy.isEnabledCollider;
        floorView.SetCollider(isEnabledCollider);
    }
    public void SaveFacilities(int flootID, List<FacilityWorkData> facilityWorkData)
    {
        floorProductData.FloorFacility[flootID] = facilityWorkData;
    }
    public void SetFloorProduct(int floorID, int amount)
    {
        floorProductData.FloorProduct[floorID] = amount;
    }
}
