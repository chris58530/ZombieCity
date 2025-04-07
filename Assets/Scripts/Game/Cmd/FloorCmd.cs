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
    public void InitFloor()
    {
        floorProxy.SetData(floorDataSetting);
    }
}
