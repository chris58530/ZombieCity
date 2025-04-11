public class PassiveHitProxy : IProxy
{
    public float shootRate;
    public void SetShootStart(float rate)
    {
        shootRate = rate;
        listener.BroadCast(PassiveHitEvent.ON_OPEN_PASSIVE_HIT);
    }
}
