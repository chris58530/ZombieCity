using UnityEngine;
using Zenject;

public class FloorCmd : ICommand
{
    [Inject] private FloorProxy floorProxy;
    [SerializeField] private FloorDataSetting floorDataSetting;
    [SerializeField] private SurvivorDataSetting survivorDataSetting;
    [Inject]private JsonDataProxy playerDataProxy;
    public override void Execute(MonoBehaviour mono)
    {
        isLazy = true;
    }
    [Listener(GameEvent.ON_GAME_STATE_START)]
    public void InitFloor()
    {
        // FloorProductData floorProductData = playerDataProxy.GetData().floorProductData;
        var floorInfoData= playerDataProxy.GetData().floorInfoData;
        floorProxy.SetFloorInfoData(floorInfoData);
        floorProxy.SetData(floorDataSetting);
        // floorProxy.SetFloorProductData(floorProductData);

    }
}
