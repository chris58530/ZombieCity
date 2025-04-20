using System.Collections.Generic;
using UnityEngine;

public class FloorProxy : IProxy
{
    public FloorDataSetting floorDataSetting;
    public FloorBase startFloor;
    public bool isEnabledCollider;
    public Dictionary<int, FloorInfoData>  floorProductData;
    public void SetFloorProductData(Dictionary<int, FloorInfoData>  floorProductData)
    {
        this.floorProductData = floorProductData;
        listener.BroadCast(JsonDataEvent.ON_UPDATE_PLAYER_DATA);
    }
    public void SetData(FloorDataSetting floorDataSetting)
    {
        this.floorDataSetting = floorDataSetting;
        foreach (var floorData in floorDataSetting.floorData)
        {
            if (floorData.isLock)
            {
                continue;
            }
        }
        startFloor = floorDataSetting.mainFloorPrefab;
        listener.BroadCast(FloorEvent.ON_FLOOR_INIT);
    }
    public void SetCollider(bool enabled)
    {
        isEnabledCollider = enabled;
        listener.BroadCast(FloorEvent.ON_UPDATE_COLLIDER);
    }
}
