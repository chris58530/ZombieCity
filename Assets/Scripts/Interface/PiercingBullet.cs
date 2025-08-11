using UnityEngine;

public class PiercingBullet : BulletBase
{
    [SerializeField] private int maxPierceCount = 3; // 最大貫穿次數
    private int currentPierceCount = 0; // 當前已貫穿次數

    public void SetPierceCount(int count)
    {
        maxPierceCount = count;
        currentPierceCount = 0;
    }

    public override void OnHitTarget(IHittable hittable)
    {
        currentPierceCount++;

        // 對目標造成傷害
        if (hittable is ZombieBase zombie)
        {
            zombie.GetDamaged(1); // 可以設置不同的傷害值
        }

        // 如果達到最大貫穿次數，則通知回收
        if (currentPierceCount >= maxPierceCount)
        {
            onHitCallBack?.Invoke(this);
        }
    }

    public override void OnDespawned()
    {
        base.OnDespawned();
        currentPierceCount = 0;
    }
}
