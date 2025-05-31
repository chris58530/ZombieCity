using UnityEngine;

public class DropItemProxy : IProxy
{
    public DropItemRequest dropItemRequest;
    public void RequestDropItem(DropItemRequest dropItemRequest)
    {
        this.dropItemRequest = dropItemRequest;
        listener.BroadCast(DropItemEvent.REQUEST_DROP_ITEM);
    }
}
public class DropItemRequest
{
    public DropItemType dropItemType;
    public Vector3 position;
    public int dropAmount;
    public DropItemRequest(DropItemType dropItemType, Vector3 position, int dropAmount = 1)
    {
        this.dropItemType = dropItemType;
        this.position = position;
        this.dropAmount = dropAmount;
    }
}