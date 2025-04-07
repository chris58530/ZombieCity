using UnityEngine;

public class SurvivorProxy : IProxy
{
    public SurvivorDataSetting survivorDataSetting;
    public int movingSurvivorId;
    public void SetData(SurvivorDataSetting survivorDataSetting)
    {
        this.survivorDataSetting = survivorDataSetting;
        listener.BroadCast(SurvivorEvent.ON_SURVIVOR_INIT);
    }
}
