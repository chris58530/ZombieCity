using System;
using UnityEngine;
using Zenject;

public class SurvivorAnimationHandler : MonoBehaviour
{
    [Inject] private Listener listener;
    [Inject] private FloorProxy floorProxy;
    [SerializeField] private int survivorId;
    public void Awake()
    {
        InjectService.Instance.Inject(this);
    }

    public virtual void AddProduct(FloorType floorType)
    {
        floorProxy.AddProduct(floorType, survivorId);
    }
    public virtual void AddProductSP(FloorType floorType)
    {
        floorProxy.AddProductSP(floorType, survivorId);
    }
}
