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
        switch (floorType)
        {
            case FloorType.Floor_901:
                dropItemProxy.RequestDropFloorItem(new DropRequest(DropItemType.Carrot, transform.position, 1));
                break;
            case FloorType.Floor_902:
                dropItemProxy.RequestDropFloorItem(new DropRequest(DropItemType.Power, transform.position, 1));
                break;
            case FloorType.Floor_903:
                dropItemProxy.RequestDropFloorItem(new DropRequest(DropItemType.dumbbel, transform.position, 1));
                break;
            case FloorType.Floor_904:
                dropItemProxy.RequestDropFloorItem(new DropRequest(DropItemType.fish, transform.position, 1));
                break;
            case FloorType.Floor_905:
                dropItemProxy.RequestDropFloorItem(new DropRequest(DropItemType.crystal, transform.position, 1));
                break;
            default:
                Debug.LogWarning("Unknown floor type: " + floorType);
                break;
        }
    }
    public virtual void AddProductSP(FloorType floorType)
    {
    }
}
