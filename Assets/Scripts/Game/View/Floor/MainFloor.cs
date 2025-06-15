using UnityEngine;

public class MainFloor : FloorBase
{
    public override void Init(FacilityAnimationDataSetting animationDataSetting, FloorView floorView)
    {
        this.floorView = floorView;

    }
    public void OnClickSkyWatcher()
    {
        floorView.OnClickSkyWatcher();
    }
    public void OnClickCampCar()
    {

        floorView.OnClickCampCar();
    }
}
