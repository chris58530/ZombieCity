using System;
using UnityEngine;
using Zenject;

public class SurvivorAnimationHandler : MonoBehaviour
{
    [Inject] private Listener listener;
    [Inject] private DropItemProxy dropItemProxy;
    [SerializeField] private int survivorId;
    public void Awake()
    {
        InjectService.Instance.Inject(this);
    }

    public virtual void AddProduct(FloorType floorType)
    {
        dropItemProxy.RequestDropItem(new DropItemRequest(DropItemType.ZombieCore, Vector3.zero, 1));
    }
    public virtual void AddProductSP(FloorType floorType)
    {
    }
}
