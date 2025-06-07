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

    public void Show(Vector3 startPosition, float keepTime, Action<DropItemObject> onCollectCallback)
    {
        this.onCollectCallback = onCollectCallback;

        float angle = UnityEngine.Random.Range(50f, 130f);
        Vector3 direction = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), 0, Mathf.Sin(angle * Mathf.Deg2Rad)).normalized;

        float dropHeight = 1f;
        float groundY = startPosition.y - dropHeight;
        Vector3 targetPosition = startPosition + direction * 1.5f;
        targetPosition.y = groundY;

        transform.position = startPosition;

        transform.DOJump(targetPosition, 1f, 2, 1f).SetEase(Ease.Linear).OnComplete(() =>
        {
            transform.position = new Vector3(transform.position.x, groundY, transform.position.z);
            DOVirtual.DelayedCall(keepTime, () =>
            {
                onCollectCallback?.Invoke(this);
            }).SetId(GetHashCode());
        });
    }
    public void OnCollect() // Button Click
    {
        Vector3 target = transform.position + new Vector3(-10f, 10f, 0f); // 向左上
        transform.DOMove(target, 2f).SetEase(Ease.InOutQuad).OnComplete(() =>
        {
            onCollectCallback?.Invoke(this);
            ResetView();
        });
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