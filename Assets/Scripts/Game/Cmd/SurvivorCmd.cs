using UnityEngine;
using Zenject;
public class SurvivorCmd : ICommand
{
    [Inject] private SurvivorProxy proxy;
    [Inject] private ClickHitProxy clickHitProxy;
    public SurvivorDataSetting survivorDataSetting;
    public override void Execute(MonoBehaviour mono)
    {
        isLazy = true;
    }
    [Listener(GameEvent.ON_INIT_GAME)]
    public void InitSurvivor()
    {
        proxy.SetData(survivorDataSetting);
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
