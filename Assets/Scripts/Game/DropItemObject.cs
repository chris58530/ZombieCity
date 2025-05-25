using System;
using UnityEngine;

public class DropItemObject : MonoBehaviour, IPoolable
{
    public DropItemType dropItemType;
    public AnimationView animationView;
    public Action<DropItemObject> onCollectCallback;
    public void OnDespawned()
    {
    }

    public void OnSpawned()
    {
    }

    public void Show(Vector3 position, Action<DropItemObject> onCollectCallback)
    {
        transform.position = position;
        this.onCollectCallback = onCollectCallback;

    }
    public void OnCollect() //Button CLick
    {
        //TODO add resource to data
        onCollectCallback?.Invoke(this);
        onCollectCallback = null;
    }
}
public enum DropItemType
{
    None,
    Money,
    ZombieCore,

}