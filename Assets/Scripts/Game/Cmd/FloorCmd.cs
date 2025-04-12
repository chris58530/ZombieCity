using UnityEngine;
using Zenject;

public class FloorCmd : ICommand
{
    [Inject] private FloorProxy floorProxy;
    [SerializeField] private FloorDataSetting floorDataSetting;
    [SerializeField] private SurvivorDataSetting survivorDataSetting;
    [Inject]private PlayerDataProxy playerDataProxy;
    public override void Execute(MonoBehaviour mono)
    {
        isLazy = true;
    }
    [Listener(GameEvent.ON_INIT_GAME)]
    public void InitFloor()
    {
        // FloorProductData floorProductData = playerDataProxy.GetData().floorProductData;
        floorProxy.SetData(floorDataSetting);
        // floorProxy.SetFloorProductData(floorProductData);

    }
}
