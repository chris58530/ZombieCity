using UnityEngine;

public class ClickHitProxy : IProxy
{
    public ZombieBase hitZombie;
    public void HitZombie(ZombieBase hitZombie)
    {
        this.hitZombie = hitZombie;
        listener.BroadCast(ClickHitEvent.ON_CLICK_ZOMBIE);
    }
}
