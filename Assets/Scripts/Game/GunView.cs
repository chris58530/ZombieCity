using UnityEngine;
using Zenject;
using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;

public class GunView : MonoBehaviour, IView
{
    [Inject] private GunViewMediator mediator;

    [Header("露營車砲台")]
    [SerializeField] private SingleGunView carGunView;

    [Header("倖存者炮台")]
    [SerializeField] private SingleGunView[] survivorGunViews;

    [Header("子彈")]
    [SerializeField] private BulletBase[] bulletPrefabs;
    private GunDataSetting currentGunDataSetting;

    private Dictionary<BulletType, PoolManager> bulletManagers = new Dictionary<BulletType, PoolManager>();
    private Tween shootTween;
    public bool isSetUp = false;
    public bool isLock = false;

    //技能系統
    public bool skill_Add = false;
    public bool skill_Penetrate = false;
    public bool skill_FireRate = false;
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
        //檢查是否有初始化過
        if (bulletManagers.Count > 0)
        {
            ActivateExistingManagers();
            return;
        }
        CreateBulletManagers();
    }

    private void ActivateExistingManagers()
    {
        foreach (var manager in bulletManagers.Values)
        {
            if (manager != null && !manager.gameObject.activeInHierarchy)
                manager.gameObject.SetActive(true);
        }
    }

    private void CreateBulletManagers()
    {
        bulletManagers.Clear();

        foreach (var bulletPrefab in bulletPrefabs)
        {
            if (bulletPrefab == null) continue;

            BulletType bulletType = GetValidBulletType(bulletPrefab);

            if (bulletManagers.ContainsKey(bulletType))
                continue;

            PoolManager manager = GetOrCreatePoolManager(bulletType);
            bulletManagers[bulletType] = manager;
            manager.RegisterPool(bulletPrefab, 30, manager.transform);
        }
    }

    private BulletType GetValidBulletType(BulletBase bulletPrefab)
    {
        return bulletPrefab.bulletType == BulletType.None ? BulletType.Normal : bulletPrefab.bulletType;
    }

    private PoolManager GetOrCreatePoolManager(BulletType bulletType)
    {
        string managerName = $"{bulletType}BulletManager";
        var existingManager = FindObjectsByType<PoolManager>(FindObjectsSortMode.None)
                             .FirstOrDefault(m => m.name == managerName);

        if (existingManager != null)
            return existingManager;

        var manager = new GameObject(managerName).AddComponent<PoolManager>();
        manager.transform.SetParent(transform);
        return manager;
    }

    public void ResetView()
    {
        shootTween?.Kill();
        isSetUp = false;
        foreach (var manager in bulletManagers.Values)
        {
            manager?.DespawnAll<BulletBase>();
        }
        carGunView.ResetView();
        foreach (var singleGun in survivorGunViews)
        {
            singleGun?.ResetView();
        }
    }

    private BulletType DetermineBulletType(BulletBase bullet)
    {
        if (bullet is TrackingBullet) return BulletType.Tracking;
        if (bullet is AreaBullet) return BulletType.Area;
        if (bullet is PiercingBullet) return BulletType.Piercing;
        return BulletType.Normal;
    }
    public void SetUpGun(GunDataSetting gunDataSetting)
    {
        currentGunDataSetting = gunDataSetting;
        SetupBaseGun(gunDataSetting);
        SetupSingleGuns(gunDataSetting);

        isSetUp = true;
    }

    private void SetupBaseGun(GunDataSetting gunDataSetting)
    {
        var gunData = gunDataSetting.campCarGunData;
        var manager = bulletManagers[gunData.bulletType];
        carGunView.SetGunData(this, gunData, manager);
    }

    private void SetupSingleGuns(GunDataSetting gunDataSetting)
    {
        for (int i = 0; i < survivorGunViews.Length && i < gunDataSetting.gunDatas.Length; i++)
        {
            SetSingleGun(i, gunDataSetting.gunDatas[i]);
        }
    }

    public void SetSingleGun(int singleGunIndex, GunData gunData)
    {
        if (!IsValidGunIndex(singleGunIndex) || gunData == null)
            return;

        SingleGunView singleGun = survivorGunViews[singleGunIndex];
        if (singleGun == null)
            return;

        PoolManager manager = GetBulletManager(gunData.bulletType);
        if (manager == null)
            return;

        singleGun.SetGunData(this, gunData, manager);
    }

    public void SetGunState(GunState gunState)
    {
        if (!isSetUp)
            return;

        // if (isLock)
        //     return;

        if (gunState == GunState.Pressing)
            StartShooting();
        else if (gunState == GunState.Releasing)
            StopShooting();
    }
    public void StartShooting()
    {
        carGunView.StartShoot();
        foreach (var singleGun in survivorGunViews)
        {
            singleGun?.StartShoot();
        }
    }

    public void StopShooting()
    {
        carGunView.StopShooting();
        foreach (var singleGun in survivorGunViews)
        {
            singleGun?.StopShooting();
        }
    }

    private bool IsValidGunIndex(int index)
    {
        return index >= 0 && index < survivorGunViews.Length;
    }

    private PoolManager GetBulletManager(BulletType bulletType)
    {
        if (bulletManagers.TryGetValue(bulletType, out var manager))
            return manager;

        return bulletManagers.Count > 0 ? bulletManagers.Values.First() : null;
    }
}
public enum GunState
{
    None,
    Pressing,
    Releasing,
    Stopping
}
