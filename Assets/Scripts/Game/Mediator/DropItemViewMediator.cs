using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class DropItemViewMediator : IMediator
{
    [Inject] private DropItemProxy dropItemProxy;
    [Inject] private FloorProxy floorProxy;
    [Inject] private ResourceInfoProxy resourceInfoProxy;

    private List<Action> logoutActions = new List<Action>();
    private DropItemView view;
    public override void Register(IView view)
    {
        base.Register(view);
        this.view = view as DropItemView;
    }
    [Listener(DropItemEvent.REQUEST_DROP_FLOOR_ITEM)]
    public void OnSpanwFloorItem()
    {
        DropRequest dropItemRequest = dropItemProxy.dropRequest;
        DropItemType type = dropItemRequest.dropItemType;
        Vector3 position = dropItemRequest.position;
        int dropAmount = dropItemRequest.dropAmount;
        Action collectAction = () =>
        {
            switch (type)
            {
                case DropItemType.Carrot:
                    floorProxy.AddProduct(FloorType.Floor_901, dropAmount);
                    break;
                case DropItemType.Power:
                    floorProxy.AddProduct(FloorType.Floor_902, dropAmount);
                    break;
                case DropItemType.dumbbel:
                    floorProxy.AddProduct(FloorType.Floor_903, dropAmount);
                    break;
                case DropItemType.fish:
                    floorProxy.AddProduct(FloorType.Floor_904, dropAmount);
                    break;
                case DropItemType.crystal:
                    floorProxy.AddProduct(FloorType.Floor_905, dropAmount);
                    break;
                default:
                    Debug.LogWarning($"Unknown drop item type: {type}");
                    return;
            }
        };
        logoutActions.Add(collectAction);
        view.OnSpawnItem(type, position, () =>
        {
            collectAction?.Invoke();
            Debug.Log($"Collecting {dropAmount} of {type} at position {position}");
            logoutActions.Remove(collectAction);

        });
    }
    [Listener(DropItemEvent.REQUEST_DROP_RESOURCE_ITEM)]
    public void OnSpanwRescourceItem()
    {
        DropRequest dropItemRequest = dropItemProxy.dropRequest;
        DropItemType type = dropItemRequest.dropItemType;
        Vector3 position = dropItemRequest.position;
        int dropAmount = dropItemRequest.dropAmount;

        Action collectAction = () =>
        {
            switch (type)
            {
                case DropItemType.Coin:
                    Debug.Log($"Collecting {dropAmount} coins.");
                    resourceInfoProxy.AddMoney(dropAmount);
                    break;
                case DropItemType.Satisfaction:
                    resourceInfoProxy.AddSatisfaction(dropAmount);
                    break;
                case DropItemType.ZombieCore:
                    resourceInfoProxy.AddZombieCoreAmount(dropAmount);
                    break;
                default:
                    Debug.LogWarning($"Unknown drop item type: {type}");
                    resourceInfoProxy.AddMoney(dropAmount);
                    return;
            }

        };
        logoutActions.Add(collectAction);
        view.OnSpawnItem(type, position, () =>
        {
            collectAction?.Invoke();
            logoutActions.Remove(collectAction);
        });
    }
}
