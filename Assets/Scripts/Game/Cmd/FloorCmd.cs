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
    [Listener(GameEvent.ON_INIT_GAME)]
    public void InitFloor()
    {
        // FloorProductData floorProductData = playerDataProxy.GetData().floorProductData;
        var floorProductData= playerDataProxy.GetData().floorInfoData;
        floorProxy.SetFloorProductData(floorProductData);
        floorProxy.SetData(floorDataSetting);
        // floorProxy.SetFloorProductData(floorProductData);

    }
}
