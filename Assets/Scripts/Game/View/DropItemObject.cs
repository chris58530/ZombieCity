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
        float angle = UnityEngine.Random.Range(70f, 110f);
        Vector3 direction = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), 0, Mathf.Sin(angle * Mathf.Deg2Rad)).normalized;
        Vector3 offset = direction * 1f + Vector3.up * 0.6f;
        Vector3 startPosition = position ;
        transform.position = startPosition+ new Vector3(0, .5f, 0);

        Vector3 targetPosition = position + direction * 1f;
        transform.DOJump(targetPosition, 0.3f, 3, .8f).SetEase(Ease.InQuad);
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