using System.Collections.Generic;
using UnityEngine;

public class SurvivorProxy : IProxy
{
    public SurvivorDataSetting survivorDataSetting;
    public SurvivorBase onClickSurvivor;
    public LeaingFacilitySurvivor leavingFacilitySurvivor;
    public Dictionary<int, SurvivorBase> survivorData = new();
    public Dictionary<int, bool> workingSurvivorData = new();
    public Dictionary<int, SurvivorJsonData> survivorInfoData = new();

    public void SetData(SurvivorDataSetting survivorDataSetting, Dictionary<int, SurvivorJsonData> survivorInfoData)
    {
        this.survivorInfoData = survivorInfoData;
        this.survivorDataSetting = survivorDataSetting;
        listener.BroadCast(SurvivorEvent.ON_SURVIVOR_INIT);
    }
    public void SetClickSurvivor(SurvivorBase survivor)
    {
        onClickSurvivor = survivor;
        listener.BroadCast(SurvivorEvent.ON_CLICK_SURVIVOR);
    }
    public SurvivorBase GetSurvivorByID(int id)
    {
        return survivorData[id];
    }
    public void SetSurvivorLeaveFacility(LeaingFacilitySurvivor leavingFacilitySurvivor)
    {
        this.leavingFacilitySurvivor = leavingFacilitySurvivor;
        listener.BroadCast(SurvivorEvent.ON_SURVIVOR_LEAVE_FACILITY);

    }
    public void SetClickSurvivorUp()
    {
        listener.BroadCast(SurvivorEvent.ON_CLICK_SURVIVOR_COMPLETE);
    }

    public int SetStayingFloorSurvivorId;
    public int AddLevelSurvivorId;
    public FloorType StayingFloor;
    public int AddLevelAmount;
    public void SetStayingFloor(int id, FloorType floorType)
    {
        SetStayingFloorSurvivorId = id;
        StayingFloor = floorType;
        listener.BroadCast(SurvivorEvent.ON_SET_SURVIVOR_STAYINGFLOOR);
    }
    public void AddLevel(int id, int amount)
    {
        AddLevelSurvivorId = id;
        AddLevelAmount = amount;
        listener.BroadCast(SurvivorEvent.ON_ADD_SURVIVOR_LEVEL);
    }

}
public class LeaingFacilitySurvivor
{
    public SurvivorBase survivor;
    public FloorBase floor;
    public FacilityBase facility;
}
