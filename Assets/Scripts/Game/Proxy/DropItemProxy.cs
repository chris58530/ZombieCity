using UnityEngine;

public class DropItemProxy : IProxy
{
    public DropRequest dropRequest;
    public void RequestDropFloorItem(DropRequest dropRequest)
    {
        this.dropRequest = dropRequest;
        listener.BroadCast(DropItemEvent.REQUEST_DROP_FLOOR_ITEM);
    }
    public void RequestDropResourceItem(DropRequest dropRequest)
    {
        this.dropRequest = dropRequest;
        listener.BroadCast(DropItemEvent.REQUEST_DROP_RESOURCE_ITEM);

    }
}
public class DropRequest
{
    public DropItemType dropItemType;
    public Vector3 position;
    public int dropAmount;
    public DropRequest(DropItemType dropItemType, Vector3 position, int dropAmount = 1)
    {
        this.dropItemType = dropItemType;
        this.position = position;
        this.dropAmount = dropAmount;
    }
}