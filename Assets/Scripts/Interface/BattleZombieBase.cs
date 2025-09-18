using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
public class BattleZombieBase : MonoBehaviour, IPoolable, IHittable
{
    [Header("Settings")]
    public int level; //生成時由ZombieSpawnData設定
    public float attack;
    public AnimationCurve attackCurve;
    public float hp;
    public AnimationCurve hpCurve;


    [SerializeField] private Image healthBar;
    [Header("Basic Properties")]
    public int id;
    public AnimationView animationView;
    public SpriteRenderer sprite;
    public bool isFresh;
    public BattleZombieManager manager;
    public bool IsDead { get; private set; } = false;
    public new Collider2D collider2D;

    [Header("Battle Properties")]
    public float speed = 1.0f;
    public float moveDuration = 5f;
    public float startAttackDistance = 3.0f;
    public bool isMoving = false;
    public IHittable chaseTarget;
    public Action<BattleZombieBase> deadCallBack;
    public int maxHp;
    private int currentHp;

    public BattleZombieBase GetZombie()
    {
        return this;
    }


    public void Setup(BattleZombieManager manager, int hp, float attackValue, Vector2 spawnPoint, IHittable target, Action<BattleZombieBase> deadCallback = null)
    {
        this.manager = manager;
        maxHp = hp;
        currentHp = hp;
        attack = attackValue;
        transform.position = spawnPoint;
        deadCallBack = deadCallback ?? ((zombie) => { Debug.Log("Zombie is dead."); });
        chaseTarget = target;
        SetLayer("Battle");
        IsDead = false;
        isMoving = false;
        isFresh = true;

        if (sprite != null)
            sprite.color = Color.white;

        // 確保 healthBar 存在且有效後再更新
        if (healthBar != null && healthBar.rectTransform != null)
        {
            UpdateHealthBar();
        }
        else
        {
            Debug.LogWarning($"HealthBar is null or invalid for zombie {id}");
        }

        gameObject.SetActive(true);
    }

    public void Setup()
    {
        IsDead = false;
        isMoving = false;
        isFresh = true;

        if (sprite != null)
            sprite.color = Color.white;

        gameObject.SetActive(true);
    }

    public void Hit()
    {
        sprite.color = Color.red;
        DOVirtual.DelayedCall(0.1f, () =>
        {
            sprite.color = Color.white;
        }).SetId(GetHashCode());
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
        isMoving = false;
        chaseTarget = null;
        deadCallBack = null;
        maxHp = 0;
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

    public virtual void FixedUpdate()
    {
        // if (!isMoving) return;
        // if (NotInAttackRange)
        // {
        //     Move();
        // }
        // else
        // {
        //     Attack();
        //     return;
        // }
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
        Vector2 moveDir = (chaseTarget.GetFixedPosition() - (Vector2)transform.position).normalized;
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
        currentHp -= damage;

        // 安全地更新血條
        if (healthBar != null && healthBar.rectTransform != null)
        {
            UpdateHealthBar();
        }

        Hit();
        if (currentHp <= 0)
        {
            Kill(() =>
            {
                deadCallBack?.Invoke(this);
                Debug.Log("Zombie is dead.");
                if (manager != null)
                {
                    manager.ResetBattleZombie(this);
                }
            });
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

    }

    private void OnTriggerExit2D(Collider2D collision)
    {

    }

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
        return transform.position;
    }
    public void UpdateHealthBar()
    {
        if (healthBar != null && maxHp > 0)
        {
            float fillAmount = (float)currentHp / maxHp;
            // 確保 fillAmount 在有效範圍內 [0, 1]
            fillAmount = Mathf.Clamp01(fillAmount);
            healthBar.fillAmount = fillAmount;

            // 確保 healthBar 的 RectTransform 有效
            if (healthBar.rectTransform != null)
            {
                var rect = healthBar.rectTransform.rect;
                if (rect.width <= 0 || rect.height <= 0 ||
                    float.IsNaN(rect.width) || float.IsNaN(rect.height) ||
                    float.IsInfinity(rect.width) || float.IsInfinity(rect.height))
                {
                    Debug.LogWarning($"Invalid healthBar rect detected for zombie {id}: {rect}");
                    return;
                }
            }
        }
    }
}
