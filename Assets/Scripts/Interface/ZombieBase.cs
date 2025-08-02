using System;
using DG.Tweening;
using UnityEngine;

public class ZombieBase : MonoBehaviour, IPoolable, IHittable
{
    public int id;
    public AnimationView animationView;
    public SpriteRenderer sprite;
    public bool isFresh;
    public ZombieManager manager;
    public bool IsDead { get; private set; } = false;
    public Collider2D collider2D;
    [Header("Battle")]
    public float attack;
    public float speed = 1.0f;
    public float moveDuration = 5f;
    public float startAttackDistance = 3.0f;
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
    #region Battle專用
    public bool isMoving = false;
    public IHittable chaseTarget;
    public Action<ZombieBase> deadCallBack;
    public int hp;
    public virtual void FixedUpdate()
    {
        if (!isMoving) return;
        if (NotInAttackRange)
        {
            Move();
        }
        else
        {
            Attack();
            return;
        }
    }
    public void LateUpdate()
    {

    }
    public void SetBattleData(IHittable hittable)
    {
        chaseTarget = hittable;
    }
    public bool NotInAttackRange
    {
        get
        {
            return Vector2.Distance(transform.position, chaseTarget.GetFixedPosition()) > startAttackDistance;
        }
    }
    public void StartMove()
    {
        isMoving = true;
    }
    public virtual void Move()
    {
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, -100), speed * Time.fixedDeltaTime);
    }
    public virtual void Attack()
    {
        // 計算朝向目標的方向
        Vector2 moveDir = (chaseTarget.GetFixedPosition() - (Vector2)transform.position).normalized;
        // 直接向前衝刺固定距離，不管目標實際位置
        Vector2 attackPos = (Vector2)transform.position + moveDir * 2f;

        isMoving = false;
        transform.DOMove(attackPos, speed / 5).SetEase(Ease.Linear).OnComplete(() =>
        {
            if (chaseTarget != null)
            {
                chaseTarget.GetDamaged(1);
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
            isMoving = true;
        });
    }
    public virtual void Idle()
    {

    }
    public virtual void GetDamaged(int damage)
    {
        hp -= damage;
        Hit();
        if (hp <= 0)
        {
            Kill(() =>
            {
                deadCallBack?.Invoke(this);
                manager.ResetZombie(this);
            });
        }
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

    public Vector2 GetFixedPosition()
    {
        throw new NotImplementedException();
    }
}
