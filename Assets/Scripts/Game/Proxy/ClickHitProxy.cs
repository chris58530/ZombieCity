using UnityEngine;

public class ClickHitProxy : IProxy
{
    public ZombieBase hitZombie;
    public SurvivorBase hitSurvivor;
    public Vector3 pickPos;
    public FloorBase clickUpFloor;
    public void HitZombie(ZombieBase hitZombie)
    {
        this.hitZombie = hitZombie;
        listener.BroadCast(ClickHitEvent.ON_CLICK_ZOMBIE);
    }
    public void HitSurvivor(SurvivorBase hitSurvivor, Vector3 pickPos)
    {
        this.pickPos = pickPos;
        this.hitSurvivor = hitSurvivor;
        listener.BroadCast(ClickHitEvent.ON_CLICK_SURVIVOR);
    }
    public void SetSurvivorFloor(FloorBase floor)
    {
        clickUpFloor = floor;
        listener.BroadCast(ClickHitEvent.ON_CLICK_UP);

    }
}
