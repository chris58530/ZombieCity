using System;
using UnityEngine;
public class SingleGunView : MonoBehaviour
{
    [SerializeField] private GunAnimationEventHandler gunAnimationEventHandler;
    [SerializeField] private AnimationView animationView;//包含所有角色的動畫
    [SerializeField] private GunData gunData;
    private PoolManager gunManager;
    [SerializeField] private Transform shootPoint;

    // 讓外部可以獲取這把槍的資料
    public GunData GunData => gunData;
    public BulletType BulletType => gunData?.bulletType ?? BulletType.Normal;
    public Action<int> onShootAnimationEvent;
    private void OnEnable()
    {
        gunAnimationEventHandler.onShootAnimationEvent += ShootAnimationEvent;
    }

    public void ResetView()
    {
        gunData = null;
    }

    public void SetGunData(GunData data, PoolManager manager)
    {
        gunManager = manager;
        gunData = data;

        // 如果有槍數據，立即顯示槍
        if (data != null)
        {
            Show();
        }
    }
    public void Show()
    {
        if (gunData == null)
        {
            Debug.LogWarning("嘗試顯示沒有設置數據的槍");
            return;
        }

        animationView.PlayAnimation("Idle_" + gunData.ID);
        StartShoot();

    }
    public void StartShoot()
    {
        animationView.PlayAnimation("Shoot_" + gunData.ID);
    }

    public void ShootAnimationEvent(int todo)
    {
        if (gunData == null)
        {
            Debug.LogError("嘗試使用未設置數據的槍進行射擊");
            return;
        }

        if (gunManager == null)
        {
            Debug.LogError("嘗試使用未設置管理器的槍進行射擊");
            return;
        }

        var bullet = gunManager.Spawn<BulletBase>(gunManager.transform);
        if (bullet == null)
        {
            Debug.LogError("無法生成子彈");
            return;
        }

        // 設置子彈位置和旋轉
        bullet.transform.position = shootPoint.position;
        bullet.transform.rotation = shootPoint.rotation;

        // 設置子彈回調
        Action<BulletBase> onHitCallBack = bulletBase =>
        {
            bulletBase.gameObject.SetActive(false);
            gunManager.Despawn(bulletBase);
        };

        // 根據槍的數據計算傷害
        float damage = gunData.attackCurve != null
            ? gunData.attackCurve.Evaluate(gunData.level)
            : gunData.level;

        // 啟用子彈
        bullet.gameObject.SetActive(true);
        bullet.SetUp(gunData.target, gunData.pathMode, damage, onHitCallBack);
        bullet.SetLayer("Battle");
        bullet.DoPathMove();

        // 播放動畫（如果有）

    }
}
