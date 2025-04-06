using UnityEngine;

public class FloorProxy : IProxy
{
    public FloorDataSetting floorDataSetting;
    public SurvivorDataSetting survivorDataSetting;
    public void SetData(FloorDataSetting floorDataSetting, SurvivorDataSetting survivorDataSetting)
    {
        this.floorDataSetting = floorDataSetting;
        this.survivorDataSetting = survivorDataSetting;
        listener.BroadCast(FloorEvent.ON_FLOOR_INIT);
    }
}
