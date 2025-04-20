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

        view.InitSurvivor(survivorDataSetting, proxy.workingSurvivorData, floorProxy.startFloor);
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
        SaveWorkingSurvivor(proxy.onClickSurvivor.id, false);
        floorProxy.SetCollider(true);
    }
    [Listener(SurvivorEvent.ON_CLICK_SURVIVOR_COMPLETE)]
    public void OnClickSurvivorComplete()
    {
        FloorBase place = clickHitProxy.clickUpFloor;
        view.OnClickSurvivorComplete(proxy.onClickSurvivor, place);
        floorProxy.SetCollider(false);
    }
    [Listener(SurvivorEvent.ON_SURVIVOR_LEAVE_FACILITY)]
    public void OnLeaveFacility()
    {
        LeaingFacilitySurvivor leavingFacilitySurvivor = proxy.leavingFacilitySurvivor;
        SaveWorkingSurvivor(leavingFacilitySurvivor.survivor.id, false);
        view.OnLeaveFacility(leavingFacilitySurvivor);
    }
    public void SaveWorkingSurvivor(int id, bool isWorking)
    {
        jsonDataProxy.jsonData.workingSurvivorData[id] = isWorking;
    }
}
