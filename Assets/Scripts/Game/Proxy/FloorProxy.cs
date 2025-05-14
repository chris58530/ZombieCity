using System.Collections.Generic;
using UnityEngine;

public class FloorProxy : IProxy
{
    public FloorDataSetting floorDataSetting;
    public FloorBase startFloor;
    public Dictionary<int, FloorBase> floorBaseDic = new();
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
            floorBaseDic[(int)floorData.floorPrefab.floorType] = floorData.floorPrefab;
        }
        startFloor = floorDataSetting.mainFloorPrefab;
        listener.BroadCast(FloorEvent.ON_FLOOR_INIT);
    }
    public void SetMainFloor(FloorBase floor)
    {
        startFloor = floor;
    }
    public void SetFloor(FloorBase floor)
    {
        if (floorBaseDic.ContainsKey((int)floor.floorType))
        {
            floorBaseDic[(int)floor.floorType] = floor;
        }
        else
        {
            floorBaseDic.Add((int)floor.floorType, floor);
        }
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
    public void AddProduct(FloorType floorType, int id)
    {
        AddProductFloor = floorType;
        AddProductAmount = id/10;
        //TODO 根據id查找對應數量 = (基數＋等級倍率)
        //新增SO Setting 
        listener.BroadCast(FloorEvent.ON_ADD_PRODUCT);
    }
    public void AddProductSP(FloorType floorType, int amount)
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
