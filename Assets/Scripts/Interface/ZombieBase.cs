using System;
using DG.Tweening;
using UnityEngine;

public class ZombieBase : MonoBehaviour, IPoolable
{
    public int id;
    public AnimationView animationView;
    public SpriteRenderer sprite;
    public bool isFresh;
    public ZombieManager manager;
    public bool IsDead { get; private set; } = false;
    public ZombieBase GetZombie()
    {
        return this;
    }
    public void Hit()
    {
        sprite.color = Color.red;
        DOVirtual.DelayedCall(0.1f, () =>
        {
            sprite.color = Color.white;
        }).SetId(GetHashCode());
    }
    public void SetIsTarget(bool isTarget)
    {
        // if (isTarget)
        // {
        //     sprite.color = Color.red;
        // }
        // else
        // {
        //     sprite.color = Color.white;
        // }
    }

    public void Kill(Action callBack = null)
    {
        if (IsDead) return;
        IsDead = true;

        sprite.color = Color.black;

        DOVirtual.DelayedCall(0.2f, () =>
         {
             callBack?.Invoke();
             callBack = null;
         }).SetId(GetHashCode());
    }
    public void OnSpawned()
    {
        Reset();
        animationView.Show();
    }
    public void SetFlip(bool isFlip)
    {
        sprite.flipX = isFlip;
    }

    public void OnDespawned()
    {
        Reset();
    }
    public void ChangeLayer(string layerName)
    {
        gameObject.layer = LayerMask.NameToLayer(layerName);
        sprite.sortingLayerName = layerName;
        foreach (Transform child in transform)
        {
            child.gameObject.layer = LayerMask.NameToLayer(layerName);
        }
    }
    public void Reset()
    {
        IsDead = false;
        animationView.Hide();
        sprite.color = Color.white;
        transform.position = Vector2.zero;
        DOTween.Kill(GetHashCode());

    }
}
