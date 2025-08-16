
public class GunProxy : IProxy
{
    public GunDataSetting gunDataSetting;
    public void RequestStartGun(GunDataSetting dataSetting)
    {
        gunDataSetting = dataSetting;
        listener.BroadCast(GunEvent.ON_GUN_START_SHOOT);
    }
}
