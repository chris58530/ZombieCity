using System.Collections.Generic;
using UnityEngine;

public class FloorProxy : IProxy
{
    public FloorDataSetting floorDataSetting;
    public FloorBase startFloor;
    public bool isEnabledCollider;
    public FloorProductData floorProductData;
    public void SetFloorProductData(FloorProductData floorProductData)
    {
        this.floorProductData = floorProductData;
        listener.BroadCast(PlayerDataEvent.ON_UPDATE_PLAYER_DATA);
    }
    public void SetData(FloorDataSetting floorDataSetting)
    {
        this.floorDataSetting = floorDataSetting;
        foreach (var floorData in floorDataSetting.floorData)
        {
            if (floorData.isLocked)
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
