using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class FloorViewMediator : IMediator
{
    [Inject] private FloorProxy floorProxy;
    [Inject] private JsonDataProxy jsonDataProxy;
    [Inject] private SurvivorProxy survivorProxy;
    [Inject] private ClickHitProxy clickHitProxy;
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
        //floor 基本設定
        FloorDataSetting floorDataSetting = floorProxy.floorDataSetting;

        //floor 存檔資料
        Dictionary<int, FloorInfoData> floorInfoData = jsonDataProxy.jsonData.floorInfoData;

        //離線時間
        double logOutTime = jsonDataProxy.jsonData.logOutData.logOutTime;
        floorView.InitFloor(floorDataSetting, floorInfoData, logOutTime);
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
    public void RequestShowSurvivor(int survivorID, FloorBase floor, FacilityBase facility)
    {
        LeaingFacilitySurvivor leaingFacilitySurvivor= new LeaingFacilitySurvivor
        {
            survivor = survivorProxy.GetSurvivorByID(survivorID),
            floor = floor,
            facility = facility
        };
        survivorProxy.SetSurvivorLeaveFacility(leaingFacilitySurvivor);
    }
    public void SaveFacilities(FloorType floorType, int order, FacilityData fdata)
    {
        jsonDataProxy.jsonData.floorInfoData[(int)floorType].facilityData[order] = fdata;
    }
    public void SaveFloorProduct(FloorType floorType, int amount)
    {
        jsonDataProxy.jsonData.floorInfoData[(int)floorType].productAmount = amount;
    }
}
