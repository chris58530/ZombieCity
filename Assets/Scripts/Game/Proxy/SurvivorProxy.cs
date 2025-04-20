using System.Collections.Generic;
using UnityEngine;

public class SurvivorProxy : IProxy
{
    public SurvivorDataSetting survivorDataSetting;
    public SurvivorBase onClickSurvivor;
    public LeaingFacilitySurvivor leavingFacilitySurvivor;
    public Dictionary<int, SurvivorBase> survivorData = new();
    public Dictionary<int, bool> workingSurvivorData = new();

    public void SetData(SurvivorDataSetting survivorDataSetting, Dictionary<int, bool> data)
    {
        this.workingSurvivorData = data; //json data
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
}
public class LeaingFacilitySurvivor
{
    public SurvivorBase survivor;
    public FloorBase floor;
    public FacilityBase facility;
}
