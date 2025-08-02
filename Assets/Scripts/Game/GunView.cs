using UnityEngine;
using Zenject;
using System;
using DG.Tweening;

public class GunView : MonoBehaviour, IView
{
    [Inject] private GunViewMediator mediator;
    [Header("Base Gun")]
    [SerializeField] private Transform shootPoint;
    [SerializeField] private BulletBase baseBulletPrefabs;
    [SerializeField] private AnimationView animationView_Base;
    private float shootRate = 0.3f;
    private PoolManager baseGunManager;
    private Tween shootTween;
    private void Awake()
    {
        InjectService.Instance.Inject(this);
    }

    private void OnEnable()
    {
        mediator.Register(this);
        Initialize();
    }

    private void OnDisable()
    {
        mediator.DeRegister(this);
    }

    private void Initialize()
    {
        if (baseGunManager) return;
        baseGunManager = new GameObject("BaseGunManager").AddComponent<PoolManager>();
        baseGunManager.RegisterPool(baseBulletPrefabs, 20, baseGunManager.transform);
        StartShoot();
    }
    private void RecycleBullet(BulletBase bullet)
    {
        baseGunManager.Despawn(bullet);
    }
    private void SpawnBullet()
    {
        BulletBase bullet = baseGunManager.Spawn<BulletBase>(baseGunManager.transform);
        //防呆 自動回收
        Tween recycleTween = DOVirtual.DelayedCall(3, () =>
        {
            RecycleBullet(bullet);
        });
        bullet.transform.position = shootPoint.position;
        bullet.transform.rotation = shootPoint.rotation;
        Action<BulletBase> onHitCallBack = (BulletBase bulletBase) =>
        {
            recycleTween?.Kill();
            RecycleBullet(bulletBase);
        };
        bullet.gameObject.SetActive(true);
        bullet.SetUp(BulletTarget.Zombie, PathMode.Straight, onHitCallBack);
        bullet.SetLayer("Battle");
        bullet.DoPathMove();


    }
    public void StartShoot()
    {
        shootTween = DOVirtual.DelayedCall(0f, () =>
         {
             SpawnBullet();
         }).SetLoops(-1, LoopType.Restart)
           .SetDelay(shootRate)
           .SetId("Shooting");
    }

    public void StopShoot()
    {
        if (shootTween != null)
        {
            shootTween.Kill();
            shootTween = null;
        }
    }
}
[Serializable]
public class GunData
{
    public int level;
    public AnimationCurve attackCurve;// 攻擊曲線對應level
}