using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class DropItemViewMediator : IMediator
{
    [Inject] private DropItemProxy dropItemProxy;
    [Inject] private FloorProxy floorProxy;
    private List<Action> logoutActions = new List<Action>();
    private DropItemView view;
    public override void Register(IView view)
    {
        base.Register(view);
        this.view = view as DropItemView;
    }
    [Listener(DropItemEvent.REQUEST_DROP_ITEM)]
    public void OnSpanwItem()
    {
        DropItemRequest dropItemRequest = dropItemProxy.dropItemRequest;
        DropItemType type = dropItemRequest.dropItemType;
        Vector3 position = dropItemRequest.position;
        int dropAmount = dropItemRequest.dropAmount;
        Action spawnAction = () =>
        {
            floorProxy.AddProductSP(floorProxy.AddProductFloor, dropAmount);
        };
        logoutActions.Add(spawnAction);
        view.OnSpawnItem(type, position, () =>
        {
            logoutActions.Remove(spawnAction);

        });
    }
}
