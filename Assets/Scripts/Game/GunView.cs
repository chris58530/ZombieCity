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
    [SerializeField] private SingleGunView baseGunView;

    [Header("倖存者炮台")]
    [SerializeField] private SingleGunView[] singleGunViews;

    [Header("子彈")]
    [SerializeField] private BulletBase[] bulletPrefabs;
    private GunDataSetting currentGunDataSetting;

    private Dictionary<BulletType, PoolManager> bulletManagers = new Dictionary<BulletType, PoolManager>();
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
        // 檢查是否已經初始化
        if (bulletManagers.Count > 0)
        {
            // 確保所有管理器都是活動的
            foreach (var manager in bulletManagers.Values)
            {
                if (!manager.gameObject.activeInHierarchy)
                    manager.gameObject.SetActive(true);
            }
            return;
        }

        // 檢查子彈預製體陣列
        if (bulletPrefabs == null || bulletPrefabs.Length == 0)
        {
            Debug.LogWarning("沒有設置子彈預製體，請在 Inspector 中設置 bulletPrefabs");
            return;
        }

        // 清空字典
        bulletManagers.Clear();

        // 為每種子彈類型創建一個 PoolManager
        foreach (var bulletPrefab in bulletPrefabs)
        {
            if (bulletPrefab == null) continue;

            // 從子彈預製體獲取子彈類型
            BulletType bulletType = bulletPrefab.bulletType;

            // 檢查是否是有效的類型
            if (bulletType == BulletType.None)
            {
                Debug.LogWarning($"子彈預製體 {bulletPrefab.name} 沒有設置有效的子彈類型");
                bulletType = BulletType.Normal; // 設置為默認類型
            }

            // 如果該類型已經有 PoolManager，則跳過
            if (bulletManagers.ContainsKey(bulletType))
            {
                Debug.Log($"已經存在 {bulletType} 類型的子彈池管理器，跳過 {bulletPrefab.name}");
                continue;
            }

            // 創建或查找對應類型的 PoolManager
            string managerName = $"{bulletType}BulletManager";
            var existingManagers = FindObjectsByType<PoolManager>(FindObjectsSortMode.None);
            var existingManager = existingManagers.FirstOrDefault(m => m.name == managerName);

            PoolManager manager;
            if (existingManager != null)
            {
                // 使用現有管理器
                manager = existingManager;
                Debug.Log($"使用現有的 {bulletType} 子彈池管理器");
            }
            else
            {
                // 創建新管理器
                manager = new GameObject(managerName).AddComponent<PoolManager>();
                manager.transform.SetParent(transform); // 設置為 GunView 的子物體
                Debug.Log($"為 {bulletType} 類型創建了新的子彈池管理器");
            }

            // 註冊到字典
            bulletManagers[bulletType] = manager;

            // 註冊子彈到對應的池
            manager.RegisterPool(bulletPrefab, 20, manager.transform);

            Debug.Log($"已註冊 {bulletPrefab.name} 到 {bulletType} 類型的子彈池管理器，預創建數量：20");
        }

        // 檢查是否成功創建了至少一個管理器
        if (bulletManagers.Count == 0)
        {
            Debug.LogError("沒有成功創建任何子彈池管理器");
        }
        else
        {
            Debug.Log($"共創建了 {bulletManagers.Count} 個子彈池管理器：" +
                     string.Join(", ", bulletManagers.Keys.Select(k => k.ToString())));
        }
    }

    public void ResetView()
    {
        shootTween?.Kill();

        // 清空所有類型的子彈池
        foreach (var manager in bulletManagers.Values)
        {
            manager?.DespawnAll<BulletBase>();
        }
    }

    private void RecycleBullet(BulletBase bullet)
    {
        if (bullet == null) return;

        // 嘗試確定子彈類型
        BulletType bulletType = DetermineBulletType(bullet);

        // 使用對應類型的管理器回收子彈
        if (bulletManagers.TryGetValue(bulletType, out var manager))
        {
            manager.Despawn(bullet);
        }
        else
        {
            Debug.LogWarning($"找不到類型 {bulletType} 的子彈池管理器");
            Destroy(bullet.gameObject);
        }
    }

    // 根據子彈對象確定其類型
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

        baseGunView.SetGunData(gunDataSetting.campCarGunData, bulletManagers[gunDataSetting.campCarGunData.bulletType]);

        for (int i = 0; i < singleGunViews.Length; i++)
        {
            if (i < gunDataSetting.gunDatas.Length)
            {
                GunData gunData = gunDataSetting.gunDatas[i];
                SetSingleGun(i, gunData);
            }
            else
            {
                Debug.LogWarning($"索引 {i} 處沒有對應的槍數據，請確保 gunDatas 的長度足夠");
            }
        }
    }

    // 設置單個槍的數據和對應的子彈管理器
    public void SetSingleGun(int singleGunIndex, GunData gunData)
    {
        // 檢查索引是否有效
        if (singleGunIndex < 0 || singleGunIndex >= singleGunViews.Length)
        {
            Debug.LogError($"無效的槍索引: {singleGunIndex}");
            return;
        }

        // 獲取對應的 SingleGunView
        SingleGunView singleGun = singleGunViews[singleGunIndex];
        if (singleGun == null)
        {
            Debug.LogError($"索引 {singleGunIndex} 處的槍視圖為空");
            return;
        }

        // 檢查槍數據
        if (gunData == null)
        {
            Debug.LogError("槍數據為空");
            return;
        }

        // 獲取對應子彈類型的管理器
        BulletType bulletType = gunData.bulletType;
        if (!bulletManagers.TryGetValue(bulletType, out var manager))
        {
            Debug.LogWarning($"找不到類型 {bulletType} 的子彈池管理器，嘗試使用默認管理器");

            // 嘗試使用第一個可用的管理器作為備選
            if (bulletManagers.Count > 0)
            {
                manager = bulletManagers.Values.First();
            }
            else
            {
                Debug.LogError("沒有可用的子彈池管理器");
                return;
            }
        }

        // 設置槍的數據和管理器
        singleGun.SetGunData(gunData, manager);

        Debug.Log($"已為索引 {singleGunIndex} 的槍設置了類型為 {bulletType} 的子彈管理器");
    }
}

