using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class DropItemView : MonoBehaviour, IView
{
    [Inject] private DropItemViewMediator mediator;
    [SerializeField] private DropItemDataSetting dropItemDataSetting;
    private Dictionary<DropItemType, DropItemManager> dropItemManagers = new Dictionary<DropItemType, DropItemManager>();

    private void Awake()
    {
        InjectService.Instance.Inject(this);
    }
    private void OnEnable()
    {
        mediator.Register(this);
    }
    private void OnDisable()
    {
        mediator.DeRegister(this);
    }
    public void Initialize()
    {
        foreach (var dropItemData in dropItemDataSetting.dropItemDatas)
        {
            DropItemObject dropItemObject = dropItemData.dropItemObject;
            DropItemManager itemManager = new GameObject("DropItemManager_" + dropItemData.dropItemObject.dropItemType).AddComponent<DropItemManager>();
            itemManager.InitItem(dropItemObject);
            itemManager.transform.SetParent(transform);
            dropItemManagers.Add(dropItemData.dropItemObject.dropItemType, itemManager);
        }
    }

    /// <param name="position"></param> 顯示在觸發物件的位置
    /// <param name="collectCallBack"></param> 加進去玩家資料裡面 數量有多少
    public void OnSpawnItem(DropItemType dropItemType, Vector3 position, Action collectCallBack)
    {
        if (dropItemManagers.TryGetValue(dropItemType, out DropItemManager itemManager))
        {
            itemManager.SpawnItem(position, collectCallBack);
        }
    }
}

public class DropItemManager : MonoBehaviour
{
    private int poolCount = 15;
    private PoolManager poolManager;

    public void InitItem(DropItemObject itemObject)
    {
        poolManager = new GameObject("ItemPool_").AddComponent<PoolManager>();
        poolManager.transform.SetParent(transform);
        poolManager.RegisterPool(itemObject, poolCount, poolManager.transform);
    }

    public void SpawnItem(Vector3 position, Action collectCallBack)
    {
        DropItemObject dropItemObject = poolManager.Spawn<DropItemObject>();
        dropItemObject.Show(position, item =>
        {
            collectCallBack?.Invoke();
            ResetItem(item);
        });
    }
    public void ResetItem(DropItemObject itemObject)
    {
        poolManager.Despawn(itemObject);
    }
}
