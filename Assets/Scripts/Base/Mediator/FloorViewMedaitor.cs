using UnityEngine;
using Zenject;

public class FloorViewMedaitor : IMediator
{
    [Inject] private FloorProxy floorProxy;
    private FloorView floorView;

    public override void DeRegister(IView view)
    {
        base.DeRegister(view);
    }
    public override void Register(IView view)
    {
        base.Register(view);
        floorView = view as FloorView;
    }
    [Listener(FloorEvent.ON_FLOOR_INIT)]
    public void InitFloor()
    {
        FloorDataSetting floorDataSetting = floorProxy.floorDataSetting;
        floorView.InitFloor(floorDataSetting);
        // Additional initialization logic for the floor view can be added here
    }

}
