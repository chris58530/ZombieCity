using UnityEngine;
using Zenject;

public class FloorCmd : ICommand
{
    [Inject] private FloorProxy floorProxy;
    [SerializeField] private FloorDataSetting floorDataSetting;
    [SerializeField] private SurvivorDataSetting survivorDataSetting;
    public override void Execute(MonoBehaviour mono)
    {
        isLazy = true;
    }
    [Listener(GameEvent.ON_INIT_GAME)]
    [Listener(DebugEvent.ON_DEBUG_EVENT)]
    public void InitFloor()
    {
        // parse json data 

        floorProxy.SetData(floorDataSetting,survivorDataSetting);
    }
}
