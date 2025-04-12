using System.Collections.Generic;
using UnityEngine;

public class FloorProxy : IProxy
{
    public FloorDataSetting floorDataSetting;
    private Dictionary<FacilityBase, Vector2> facilityVectorDic = new();
    private List<FacilityBase> allFacilitys = new();
    public FloorBase startFloor;
    public bool isEnabledCollider;
    public FloorProductData floorProductData;
    public void SetFloorProductData(FloorProductData floorProductData)
    {
        this.floorProductData = floorProductData;
      
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

            foreach (var facility in floorData.floorPrefab.GetFacilities())
            {
                if (facilityVectorDic.ContainsKey(facility))
                    continue;
                facilityVectorDic.Add(facility, facility.transform.localPosition);
                allFacilitys.Add(facility);
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
