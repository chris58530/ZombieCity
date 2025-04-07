using System.Collections.Generic;
using UnityEngine;

public class FloorProxy : IProxy
{
    public FloorDataSetting floorDataSetting;
    private Dictionary<FacilityBase, Vector2> facilityVectorDic = new();
    private List<FacilityBase> allFacilitys = new();
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
                facilityVectorDic.Add(facility, facility.transform.localPosition);
                allFacilitys.Add(facility);
            }
        }
        listener.BroadCast(FloorEvent.ON_FLOOR_INIT);
    }
    public Transform GetRandomUnusedFacilityTransform()
    {
        if (allFacilitys == null || allFacilitys.Count == 0)
            return null;

        var unusedFacilities = allFacilitys.FindAll(f => !f.isUsing);

        if (unusedFacilities.Count == 0)
            return null;

        var selected = unusedFacilities[Random.Range(0, unusedFacilities.Count)];
        return facilityVectorDic.TryGetValue(selected, out var position) ? selected.transform : null;
    }
}
