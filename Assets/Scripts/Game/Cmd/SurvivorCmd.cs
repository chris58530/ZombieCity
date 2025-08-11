using UnityEngine;
using Zenject;
public class SurvivorCmd : ICommand
{
    [Inject] private SurvivorProxy proxy;
    [Inject] private ClickHitProxy clickHitProxy;
    [Inject] private JsonDataProxy jsonDataProxy;
    public SurvivorDataSetting survivorDataSetting;
    private bool isInitialized = false;

    public override void Execute(MonoBehaviour mono)
    {
        isLazy = true;
    }

    [Listener(GameEvent.ON_GAME_STATE_END)]
    public void OnGameStateEnd()
    {
        // 清理 SurvivorProxy 中的字典，防止在狀態切換時保留舊的引用
        Debug.Log("SurvivorCmd: 清理 Survivor 數據");
        proxy.survivorData.Clear();
    }

    [Listener(GameEvent.ON_GAME_STATE_START)]
    public void OnGameStateStart()
    {
        // 只有在從 battle 返回 game 狀態時才執行，即已經初始化過
        if (isInitialized && survivorDataSetting != null)
        {
            Debug.Log("SurvivorCmd: 從 Battle 返回，重新初始化 Survivor");
            var data = jsonDataProxy.jsonData.survivorInfoData;
            proxy.SetData(survivorDataSetting, data);
        }
    }

    [Listener(FloorEvent.ON_FLOOR_INIT_COMPELET)]
    public void InitSurvivor()
    {
        var data = jsonDataProxy.jsonData.survivorInfoData;
        proxy.SetData(survivorDataSetting, data);
        isInitialized = true;
        Debug.Log("SurvivorCmd: 初次初始化 Survivor 完成");
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
