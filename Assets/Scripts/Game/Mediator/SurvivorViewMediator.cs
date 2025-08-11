using System.Collections.Generic;
using UnityEngine;
using Zenject;
public class SurvivorViewMediator : IMediator
{
    [Inject] private SurvivorProxy proxy;
    [Inject] private FloorProxy floorProxy;
    [Inject] private ClickHitProxy clickHitProxy;
    [Inject] private JsonDataProxy jsonDataProxy;
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
        Dictionary<int, SurvivorJsonData> survivorJsonData = jsonDataProxy.jsonData.survivorInfoData;
        view.InitSurvivor(survivorDataSetting, survivorJsonData, floorProxy.startFloor, floorProxy.floorBaseDic);
    }
    public void SetSurvivorDic(int id, SurvivorBase survivor)
    {
        proxy.survivorData[id] = survivor;
    }
    [Listener(SurvivorEvent.ON_CLICK_SURVIVOR)]
    public void OnClickSurvivor()
    {
        Vector3 pickPos = clickHitProxy.pickPos;
        view.OnClickSurvivor(proxy.onClickSurvivor, pickPos);
        floorProxy.SetCollider(true);
    }
    [Listener(SurvivorEvent.ON_CLICK_SURVIVOR_COMPLETE)]
    public void OnClickSurvivorComplete()
    {
        FloorBase place = clickHitProxy.clickUpFloor;
        view.OnClickSurvivorComplete(proxy.onClickSurvivor, place);
        floorProxy.SetCollider(false);
    }
    [Listener(SurvivorEvent.ON_ADD_SURVIVOR_LEVEL)]

    public void AddSurviorLevel()
    {
        int id = proxy.AddLevelSurvivorId;
        int amount = proxy.AddLevelAmount;
        view.AddLevel(id, amount);
    }
    [Listener(SurvivorEvent.ON_SET_SURVIVOR_STAYINGFLOOR)]

    public void SetSurvivorStayingFloor()
    {
        int id = proxy.SetStayingFloorSurvivorId;
        FloorType floorType = proxy.StayingFloor;
        view.SetStayingFloor(id, floorType);
    }

    public void SaveSurvivorLevel(int id, int level)
    {
        jsonDataProxy.jsonData.survivorInfoData[id].level = level;
    }
    public void SaveSurvivorStayingFloor(int id, FloorType floor)
    {
        jsonDataProxy.jsonData.survivorInfoData[id].stayingFloor = (int)floor;
    }
}
