using UnityEngine;

public class ClickHitProxy : IProxy
{
    public ZombieBase hitZombie;
    public SurvivorBase hitSurvivor;
    public void HitZombie(ZombieBase hitZombie)
    {
        this.hitZombie = hitZombie;
        listener.BroadCast(ClickHitEvent.ON_CLICK_ZOMBIE);
    }
    public void HitSurvivor(SurvivorBase survivor)
    {
        this.hitSurvivor = survivor;
        listener.BroadCast(ClickHitEvent.ON_CLICK_SURVIVOR);
    }
}
