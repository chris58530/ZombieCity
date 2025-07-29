using UnityEngine;
using Zenject;
using System;

public class GunView : MonoBehaviour, IView
{
    [Inject] private GunViewMediator mediator;
    [Header("Base Gun")]
    [SerializeField] private Transform shootPoint;
    [SerializeField]private BulletBase baseBulletPrefabs;
    [SerializeField] private AnimationView animationView_Base;
    private PoolManager baseGunManager;
    [Header("Test")]
    [SerializeField] private bool isTest;
    [SerializeField] private GunData gunData;
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

    }

    public void StartShoot()
    {

    }
}
[Serializable]
public class GunData
{
    public int level;
    public AnimationCurve attackCurve;// 攻擊曲線對應level
}