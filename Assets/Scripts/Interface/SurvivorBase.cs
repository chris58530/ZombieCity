using UnityEngine;
using DG.Tweening;
using System;
using Lean.Touch;

public class SurvivorBase : MonoBehaviour
{
    public int id;
    public AnimationView animationView;
    public SpriteRenderer sprite;
    public bool isBusy;
    private bool isDragging = false;
    

    public SurvivorBase GetSurvivor()
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
    public void OnPick(Vector3 pickPosition)
    {
        transform.position = new Vector3(pickPosition.x, pickPosition.y, -0.01f);
    }
    public void SetCollider(bool isCollider)
    {
        Collider2D collider = GetComponent<Collider2D>();
        collider.enabled = isCollider;

    }
    public void OnDrop(Vector3 dropPos)
    {
        transform.localScale = Vector3.one;
        transform.position = dropPos;

    }


}
