using System;
using DG.Tweening;
using UnityEngine;

public class DropItemObject : MonoBehaviour, IPoolable
{
    public DropItemType dropItemType;
    public DropItemBounceType dropItemBounceType;
    public CollectPerformanceType collectPerformanceType;
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

        float angle;
        do
        {
            switch (dropItemBounceType)
            {
                case DropItemBounceType.Small:
                    angle = UnityEngine.Random.Range(85f, 95f);
                    break;
                case DropItemBounceType.Big:
                    angle = UnityEngine.Random.Range(30f, 150f);
                    break;
                default:
                    angle = UnityEngine.Random.Range(85f, 95f);
                    break;
            }
        } while (angle == 90);
        Vector3 direction = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), 0, Mathf.Sin(angle * Mathf.Deg2Rad)).normalized;

        float dropHeight = .5f;
        float groundY = startPosition.y - dropHeight;
        Vector3 targetPosition = startPosition + direction * 2f;
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
        if (collectPerformanceType == CollectPerformanceType.Move)
        {
            Vector3 target = new Vector3(-2f, transform.position.y + 2.8f, 0f); // 向左上

            transform.DOMove(target, 1f).SetEase(Ease.InOutQuad).OnComplete(() =>
            {
                onCollectCallback?.Invoke(this);
                ResetView();
            });
        }
        else if (collectPerformanceType == CollectPerformanceType.Coin)
        {
            Vector3 target = new Vector3(0f, transform.position.y + 20.8f, 0f); // 向上

            transform.DOMove(target, 1f).SetEase(Ease.InOutQuad).OnComplete(() =>
            {
                onCollectCallback?.Invoke(this);
                ResetView();
            });
        }
        else if (collectPerformanceType == CollectPerformanceType.ZombieCore)
        {
            Vector3 target = new Vector3(3f, transform.position.y + 20.8f, 0f); // 向上

            transform.DOMove(target, 1f).SetEase(Ease.InOutQuad).OnComplete(() =>
            {
                onCollectCallback?.Invoke(this);
                ResetView();
            });
        }
        else
        {
            onCollectCallback?.Invoke(this);
            ResetView();
        }
    }
    public void ResetView()
    {
        animationView.StopAnimation();
        onCollectCallback = null;
        DOTween.Kill(GetHashCode());
    }
    public void OnDisable()
    {
        onCollectCallback?.Invoke(this);
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
public enum DropItemBounceType
{
    None,
    Small, // 小跳
    Big,
}
public enum CollectPerformanceType
{
    None,
    Move,
    Coin,
    ZombieCore
}