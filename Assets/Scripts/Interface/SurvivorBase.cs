using UnityEngine;
using DG.Tweening;
using System;

public class SurvivorBase : MonoBehaviour
{
    public int id;
    public AnimationView animationView;
    public SpriteRenderer sprite;
    public bool isBusy;
    public SurvivorBase GetZombie()
    {
        return this;
    }
    public void SetBusy(bool isBusy, Action callBack = null)
    {
        this.isBusy = isBusy;
        DOVirtual.DelayedCall(1, () =>
        {
            callBack?.Invoke();

        }).SetId(GetHashCode());


    }
    public void Hit()
    {
        sprite.color = Color.red;
        DOVirtual.DelayedCall(0.1f, () =>
        {
            sprite.color = Color.white;
        }).SetId(GetHashCode());
    }
    public void SetFlip(bool isFlip)
    {
        sprite.flipX = isFlip;
    }
}
