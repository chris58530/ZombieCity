using UnityEngine;
using Zenject;
public class SurvivorCmd : ICommand
{
    [Inject] private SurvivorProxy proxy;
    [Inject] private ClickHitProxy clickHitProxy;
    [Inject] private JsonDataProxy jsonDataProxy;
    public SurvivorDataSetting survivorDataSetting;
    public override void Execute(MonoBehaviour mono)
    {
        isLazy = true;
    }
    [Listener(FloorEvent.ON_FLOOR_INIT_COMPELET)]
    public void InitSurvivor()
    {
        var data = jsonDataProxy.jsonData.workingSurvivorData;
        proxy.SetData(survivorDataSetting, data);
    }
    [Listener(ClickHitEvent.ON_CLICK_SURVIVOR)]
    public void OnClickSurvivor()
    {
        SurvivorBase survivor = clickHitProxy.hitSurvivor;
        proxy.SetClickSurvivor(survivor);
    }
    [Listener(ClickHitEvent.ON_CLICK_UP)]
    public void OnClickUp()
    {
        proxy.SetClickSurvivorUp();
    }

}
