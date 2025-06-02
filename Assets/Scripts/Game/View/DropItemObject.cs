using System;
using DG.Tweening;
using UnityEngine;

public class DropItemObject : MonoBehaviour, IPoolable
{
    public DropItemType dropItemType;
    public AnimationView animationView;
    private Action<DropItemObject> onCollectCallback;
    public void OnDespawned()
    {

    }

    public void OnSpawned()
    {

    }

    public void Show(Vector3 position, float keepTime, Action<DropItemObject> onCollectCallback)
    {
        transform.position = position;
        this.onCollectCallback = onCollectCallback;

        DOVirtual.DelayedCall(keepTime, () =>
        {
            onCollectCallback?.Invoke(this);
        }).SetId(GetHashCode());
    }
    public void OnCollect() //Button CLick
    {
        //TODO add resource to data
        onCollectCallback?.Invoke(this);

        ResetView();
        // animationView.PlayAnimation("Collect", () =>
        // {

        // });

    }
    public void ResetView()
    {
        animationView.StopAnimation();
        onCollectCallback = null;
        DOTween.Kill(GetHashCode());
    }
}
public enum DropItemType
{
    None,
    //Resource
    Coin,
    Satisfaction,
    ZombieCore,

    //FloorItem
    Carrot, //901
    Power, //902
    dumbbel, //903
    fish, //904
    crystal //905

}   