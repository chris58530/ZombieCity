using System;
using UnityEngine;

public class BulletBase : MonoBehaviour,IPoolable
{
    public Action<BulletBase> onHitCallBack;
    public BulletTarget bulletTarget;
    public void SetUp(BulletTarget bulletTarget, Action<BulletBase> onHitCallBack = null)
    {
        this.bulletTarget = bulletTarget;
        this.onHitCallBack = onHitCallBack;
    }
    public bool IsTarget(IHittable hittable)
    {
        switch (bulletTarget)
        {
            case BulletTarget.Zombie:
                return hittable is ZombieBase;
            case BulletTarget.Player:
            case BulletTarget.Car:
                return hittable is BattleCampCarController;
            case BulletTarget.None:
            default:
                return false;
        }
    }
    public void OnDespawned()
    {
    }

    public void OnSpawned()
    {
    }
    private void OTriggerEnter2D(Collider2D collision)
    {
        if (!collision.TryGetComponent(out IHittable hittable)) return;
        if (!IsTarget(hittable)) return;
        hittable.Hit();
        onHitCallBack?.Invoke(this);
    }
}
public enum BulletTarget
{
    Zombie,
    Player,
    Car,
    None
}