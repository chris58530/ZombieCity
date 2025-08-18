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
        if (hittable is SafeZombieBase zombie)
        {
            zombie.GetDamaged(1); // 可以設置不同的傷害值
        }
        else if (hittable is BattleZombieBase battleZombie)
        {
            battleZombie.GetDamaged(1); // 可以設置不同的傷害值
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

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.TryGetComponent(out IHittable hittable)) return;
        if (!HitTarget(hittable)) return;

        OnHitTarget(hittable);

        // 貫穿子彈不會在第一次命中時就回收，除非達到最大貫穿次數
        // 回收邏輯在 OnHitTarget 中處理
    }
}
