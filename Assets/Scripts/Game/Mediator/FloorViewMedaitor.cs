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
        Dictionary<int, FloorJsonData> floorInfoData = jsonDataProxy.jsonData.floorInfoData;

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
        floorView.SetAllCollider(isEnabledCollider);
    }
    [Listener(FloorEvent.ON_ADD_PRODUCT)]
    public void AddFloorProduct()
    {
        FloorType floorType = floorProxy.AddProductFloor;
        int amount = floorProxy.AddProductAmount;
        floorView.AddProduct(floorType, amount);
    }
    [Listener(FloorEvent.ON_ADD_LEVEL)]
    public void AddFloorLevel()
    {
        FloorType floorType = floorProxy.AddLevelFloor;
        int level = floorProxy.AddLevelAmount;
        floorView.AddLevel(floorType, level);
    }
    public void SetFloor(FloorBase floor)
    {
        floorProxy.SetFloor(floor);
    }
    public void SetMainFloor(FloorBase floor)
    {
        floorProxy.SetMainFloor(floor);
    }
    public void SaveFloorProduct(FloorType floorType, int amount)
    {
        jsonDataProxy.jsonData.floorInfoData[(int)floorType].productAmount = amount;
        listener.BroadCast(JsonDataEvent.ON_UPDATE_PLAYER_DATA);
    }
    public void SaveFloorLevel(FloorType floorType, int level)
    {
        jsonDataProxy.jsonData.floorInfoData[(int)floorType].level = level;
        listener.BroadCast(JsonDataEvent.ON_UPDATE_PLAYER_DATA);
    }
    public void OnClickSkyWatcher()
    {
        listener.BroadCast(TrasitionBackGroundEvent.ON_TRASITION_BACKGROUND);
        listener.BroadCast(DrawCardEvent.ON_DRAW_CARD_SHOW);
    }
}
