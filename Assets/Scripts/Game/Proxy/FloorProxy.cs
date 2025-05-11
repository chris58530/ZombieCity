using System.Collections.Generic;
using UnityEngine;

public class FloorProxy : IProxy
{
    public FloorDataSetting floorDataSetting;
    public FloorBase startFloor;
    public bool isEnabledCollider;
    public Dictionary<int, FloorJsonData> floorInfoData;
    public void SetFloorInfoData(Dictionary<int, FloorJsonData> floorInfoData)
    {
        this.floorInfoData = floorInfoData;
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

    public FloorType AddProductFloor;
    public FloorType AddLevelFloor;
    public int AddProductAmount;
    public int AddLevelAmount;
    public void AddProduct(FloorType floorType, int amount)
    {
        AddProductFloor = floorType;
        AddProductAmount = amount;
        listener.BroadCast(FloorEvent.ON_ADD_PRODUCT);
    }
    public void AddLevel(FloorType floorType, int amount)
    {
        AddLevelFloor = floorType;
        AddLevelAmount = amount;
        listener.BroadCast(FloorEvent.ON_ADD_LEVEL);
    }
}
