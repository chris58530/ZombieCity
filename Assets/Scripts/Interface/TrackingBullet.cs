using UnityEngine;
using DG.Tweening;
using System.Linq;
using System.Collections.Generic;

public class TrackingBullet : BulletBase
{
    [SerializeField] private float trackingSpeed = 5f;
    [SerializeField] private float maxTrackingDistance = 10f;
    [SerializeField] private float trackingUpdateInterval = 0.1f;

    private Transform targetTransform;
    private Tween moveTween;
    private Tween updateTargetTween;

    public override void DoPathMove(PathMode mode = PathMode.Straight)
    {
        // 初始移動，使用直線路徑
        moveTween = transform.DOMove(transform.position + transform.up * 10, 3f)
            .SetEase(Ease.Linear);

        // 啟動目標追蹤更新
        StartTracking();
    }

    private void StartTracking()
    {
        // 定期更新目標位置
        updateTargetTween = DOVirtual.DelayedCall(trackingUpdateInterval, () =>
        {
            // 尋找最近的目標
            FindClosestTarget();

            if (targetTransform != null)
            {
                // 取消原來的移動
                moveTween.Kill();

                // 計算方向
                Vector3 direction = (targetTransform.position - transform.position).normalized;

                // 更新子彈旋轉，使其朝向目標
                transform.up = direction;

                // 重新設置移動
                moveTween = transform.DOMove(transform.position + direction * 10, 3f)
                    .SetEase(Ease.Linear);
            }

            // 繼續追蹤
            StartTracking();
        }).SetId(GetInstanceID());
    }

    private void FindClosestTarget()
    {
        // 根據子彈目標類型查找目標
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, maxTrackingDistance);

        if (colliders.Length == 0)
            return;

        // 篩選出有效目標
        List<Transform> validTargets = new List<Transform>();

        foreach (var collider in colliders)
        {
            if (collider.TryGetComponent(out IHittable hittable) && HitTarget(hittable))
            {
                validTargets.Add(collider.transform);
            }
        }

        if (validTargets.Count == 0)
            return;

        // 找出最近的目標
        targetTransform = validTargets
            .OrderBy(t => Vector3.Distance(transform.position, t.position))
            .FirstOrDefault();
    }

    public override void OnDespawned()
    {
        base.OnDespawned();
        targetTransform = null;
        moveTween?.Kill();
        updateTargetTween?.Kill();
    }

    private void OnDestroy()
    {
        moveTween?.Kill();
        updateTargetTween?.Kill();
    }
}
