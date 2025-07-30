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
    [SerializeField] private float shootRate = 0.3f;
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
        baseGunManager = new GameObject("BaseGunManager").AddComponent<PoolManager>();
        baseGunManager.transform.SetParent(transform);
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
        bullet.transform.position = shootPoint.position;
        bullet.transform.rotation = shootPoint.rotation;
        Action<BulletBase> onHitCallBack = (BulletBase bulletBase) =>
        {
            RecycleBullet(bulletBase);
        };
        bullet.SetUp(BulletTarget.Zombie, onHitCallBack);
        bullet.gameObject.SetActive(true);
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