using System;
using DG.Tweening;
using UnityEngine;

public class BulletBase : MonoBehaviour,IPoolable
{
    public Action<BulletBase> onHitCallBack;
    public BulletTarget bulletTarget;
    public PathMode pathMode;
    public SpriteRenderer sprite;
    public void SetUp(BulletTarget bulletTarget, PathMode pathMode, Action<BulletBase> onHitCallBack = null)
    {
        this.bulletTarget = bulletTarget;
        this.pathMode = pathMode;
        this.onHitCallBack = onHitCallBack;
    }
     public void SetLayer(string layerName)
    {
        int layer = LayerMask.NameToLayer(layerName);
        gameObject.layer = LayerMask.NameToLayer(layerName);
        sprite.gameObject.layer = layer;
        foreach (Transform child in transform)
        {
            child.gameObject.layer = LayerMask.NameToLayer(layerName);
        }
    }
    public void DoPathMove()
    {
        if (pathMode == PathMode.Straight)
        {
            StraightMove();
        }
        else if (pathMode == PathMode.Curve)
        {
            // Implement curve movement logic here
        }

    }
    private void StraightMove()
    {
               transform.DOMove(transform.position + transform.up * 10, 1f)
            .SetEase(Ease.Linear);
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
public enum PathMode
{
    Straight,
    Curve,
}