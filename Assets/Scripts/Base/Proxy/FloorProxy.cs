using UnityEngine;

public class FloorProxy : IProxy
{
    public FloorDataSetting floorDataSetting;
    public void SetData(FloorDataSetting floorDataSetting)
    {
        this.floorDataSetting = floorDataSetting;
        listener.BroadCast(FloorEvent.ON_FLOOR_INIT);
    }
}
