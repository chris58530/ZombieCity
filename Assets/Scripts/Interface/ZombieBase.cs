using System;
using DG.Tweening;
using UnityEngine;

public class ZombieBase : MonoBehaviour, IPoolable
{
    public int id;
    public AnimationView animationView;
    public SpriteRenderer sprite;

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
    public void SetDead(Action callBack)
    {
        sprite.color = Color.black;

        DOVirtual.DelayedCall(0.5f, () =>
         {
             callBack?.Invoke();
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
    public void Reset()
    {
        animationView.Hide();
        sprite.color = Color.white;
        transform.position = Vector2.zero;
        DOTween.Kill(GetHashCode());

    }
}
