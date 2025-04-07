using UnityEngine;

public class SurvivorProxy : IProxy
{
    public SurvivorDataSetting survivorDataSetting;
    public SurvivorBase onClickSurvivor;
    public int movingSurvivorId;
    public void SetData(SurvivorDataSetting survivorDataSetting)
    {
        this.survivorDataSetting = survivorDataSetting;
        listener.BroadCast(SurvivorEvent.ON_SURVIVOR_INIT);
    }
    public void SetClickSurvivor(SurvivorBase survivor)
    {
        onClickSurvivor = survivor;
        listener.BroadCast(SurvivorEvent.ON_CLICK_SURVIVOR);

    }
}
