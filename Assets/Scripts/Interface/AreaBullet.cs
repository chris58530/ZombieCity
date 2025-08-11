using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public class AreaBullet : BulletBase
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float explosionRadius = 2f;
    [SerializeField] private GameObject explosionEffectPrefab; // 爆炸效果預製體，可選

    private Vector3 targetPosition;
    private bool hasTarget = false;
    private Tween moveTween;

    public void SetTargetPosition(Vector3 position)
    {
        targetPosition = position;
        hasTarget = true;
    }

    public void SetTargetTransform(Transform target)
    {
        if (target != null)
        {
            targetPosition = target.position;
            hasTarget = true;
        }
    }

    public override void DoPathMove(PathMode mode = PathMode.Straight)
    {
        if (!hasTarget)
        {
            // 如果沒有目標，使用默認的直線移動
            base.DoPathMove(mode);
            return;
        }

        // 計算飛行時間
        float distance = Vector3.Distance(transform.position, targetPosition);
        float duration = distance / moveSpeed;

        // 移動到目標位置
        moveTween = transform.DOMove(targetPosition, duration)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                // 到達目標位置時，觸發範圍爆炸
                ExplodeAtTarget();
            });
    }

    private void ExplodeAtTarget()
    {
        // 播放爆炸效果
        if (explosionEffectPrefab != null)
        {
            Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        }

        // 尋找範圍內的所有目標
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        List<IHittable> hitTargets = new List<IHittable>();

        foreach (var collider in colliders)
        {
            if (collider.TryGetComponent(out IHittable hittable) && HitTarget(hittable))
            {
                // 確保每個目標只被計算一次
                if (!hitTargets.Contains(hittable))
                {
                    hitTargets.Add(hittable);
                    OnHitTarget(hittable);
                }
            }
        }

        // 通知回收子彈
        onHitCallBack?.Invoke(this);
    }

    public override void OnHitTarget(IHittable hittable)
    {
        // 對目標造成傷害
        if (hittable is ZombieBase zombie)
        {
            zombie.GetDamaged(2); // 範圍傷害可能更高
        }
    }

    // 覆蓋碰撞處理，子彈飛行途中不會被碰撞物阻擋
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 不執行任何操作，讓子彈繼續飛行到目標位置
    }

    public override void OnDespawned()
    {
        base.OnDespawned();
        hasTarget = false;
        targetPosition = Vector3.zero;
        moveTween?.Kill();
    }

    private void OnDestroy()
    {
        moveTween?.Kill();
    }

#if UNITY_EDITOR
    // 在編輯器中繪製爆炸範圍
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
#endif
}
