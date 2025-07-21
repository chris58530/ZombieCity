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
    [Header("Battle")]
    public float attack;
    public float moveDuration = 5f;
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
        int layer = LayerMask.NameToLayer(layerName);

        gameObject.layer = LayerMask.NameToLayer(layerName);
        sprite.gameObject.layer = layer;
        foreach (Transform child in transform)
        {
            child.gameObject.layer = LayerMask.NameToLayer(layerName);
        }
    }
    #region Battle專用
    private Tween moveTween;
    public virtual void Move(IHittable hittable, Action callBack = null)
    {
        Vector2 targetPosition = hittable.GetFixedPosition();
        moveTween = transform.DOMove(targetPosition, moveDuration)
           .OnComplete(() =>
           {
               callBack?.Invoke();
           });
    }
    public virtual void Attack(IHittable hittable)
    {
        Debug.Log("Attack " + hittable.GetFixedPosition());
        moveTween?.Kill();
        hittable.Hit();
        Idle();
    }
    public virtual void Idle()
    {

    }
    public virtual void GetHurt()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<IHittable>(out IHittable hittable))
        {
            moveTween?.Kill();
            Attack(hittable);
        }
        ;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<IHittable>(out IHittable hittable))
        {
            Move(hittable);
        }
    }
    #endregion
    public void Reset()
    {
        IsDead = false;
        animationView.Hide();
        sprite.color = Color.white;
        transform.position = Vector2.zero;
        DOTween.Kill(GetHashCode());

    }
}
