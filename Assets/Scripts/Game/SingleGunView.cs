using System;
using DG.Tweening;
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

    private GunView gunView;

    public bool canShoot = true;

    private void OnEnable()
    {
        gunAnimationEventHandler.onShootAnimationEvent += ShootAnimationEvent;
    }

    public void ResetView()
    {
        gunData = null;
        DOTween.Kill(GetHashCode());
    }

    public void SetGunData(GunView gunView, GunData data, PoolManager manager)
    {
        gunManager = manager;
        gunData = data;
        this.gunView = gunView;

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
    }
    public void StartShoot()
    {
        string animationName = "Shoot_" + gunData.ID;
        float speed = gunView.skill_FireRate ? 1.5f : 0.75f;
        animationView.PlayAnimation(animationName);
        animationView.SetAnimationSpeed(speed);
    }

    public void StopShooting()
    {
        animationView.PlayAnimation("Idle_" + gunData.ID);
    }

    public void ShootAnimationEvent(int todo)
    {
        if (!canShoot) return;

        int bulletCount = gunView.skill_Add ? 2 : 1;

        if (bulletCount == 1)
        {
            CreateBullet(Vector3.zero);
        }
        else
        {
            float spacing = 0.5f; // 子彈間距
            CreateBullet(Vector3.left * spacing / 2f);   // 左邊
            CreateBullet(Vector3.right * spacing / 2f);  // 右邊
        }
    }

    private void CreateBullet(Vector3 positionOffset)
    {
        var bullet = gunManager.Spawn<BulletBase>(gunManager.transform);
        if (bullet == null)
        {
            Debug.LogError("Bullet 無法生成子彈");
            return;
        }

        // 應用位置偏移（相對於 shootPoint 的局部偏移）
        Vector3 offsetPosition = shootPoint.TransformPoint(positionOffset);
        bullet.transform.position = offsetPosition;

        // 保持相同的旋轉角度
        bullet.transform.rotation = shootPoint.rotation;

        Action<BulletBase> onHitCallBack = bulletBase =>
        {
            if (!gunView.skill_Penetrate)
                gunManager.Despawn(bulletBase);
        };

        float damage = gunData.attackCurve != null
            ? gunData.attackCurve.Evaluate(gunData.level)
            : gunData.level;

        bullet.gameObject.SetActive(true);
        bullet.SetUp(BulletTarget.Zombie, gunData.pathMode, damage, onHitCallBack);
        bullet.SetLayer("Battle");
        bullet.DoPathMove();

        //計時自動消失
        float autoResetTime = 3;
        bullet.AutoReset(autoResetTime, () =>
        {
            gunManager.Despawn(bullet);
        });
    }
}
