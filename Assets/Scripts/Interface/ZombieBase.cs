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
    public float speed = 1.0f;
    public float moveDuration = 5f;
    public float attackDistance = 1.0f;
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
    public bool ismoving = false;
    public IHittable chaseTarget;
    public virtual void FixedUpdate()
    {
        if (!ismoving) return;
        if (NotInAttackRange())
        {
            Move();
        }
        else
        {
            Attack();
        }
    }
    public void LateUpdate()
    {

    }
    public void SetBattleData(IHittable hittable)
    {
        chaseTarget = hittable;
    }
    public bool NotInAttackRange()
    {
        return Vector2.Distance(transform.position, chaseTarget.GetFixedPosition()) > attackDistance;
    }
    public void StartMove()
    {
        ismoving = true;
    }
    public virtual void Move()
    {
        transform.position = Vector2.MoveTowards(transform.position, chaseTarget.GetFixedPosition(), speed * Time.fixedDeltaTime);
    }
    public virtual void Attack()
    {
        Vector2 targetPosition = chaseTarget.GetFixedPosition();
        Vector2 currentPosition = transform.position;
        float totalDistance = Vector2.Distance(currentPosition, targetPosition);
        Vector2 direction = (targetPosition - currentPosition).normalized;
        float distanceToMove = totalDistance - attackDistance;
        Vector2 intermediateTarget = currentPosition + direction * distanceToMove;
        ismoving = false;
        transform.DOMove(intermediateTarget, speed/3).SetEase(Ease.Linear).OnComplete(() =>
        {
            if (chaseTarget != null)
            {
                chaseTarget.Hit();
            }
            StepBack();
        });

    }
    public void StepBack()
    {
        float stepDistance = speed * 3f;
        Vector2 targetPosition = (Vector2)transform.position + Vector2.up * speed * 3f;
        transform.DOMove(targetPosition, 3f).SetEase(Ease.Linear).OnComplete(() =>
        {
            ismoving = true;
        });
    }
    public virtual void Idle()
    {

    }
    public virtual void GetHurt()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

    }
    private void OnTriggerExit2D(Collider2D collision)
    {

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
