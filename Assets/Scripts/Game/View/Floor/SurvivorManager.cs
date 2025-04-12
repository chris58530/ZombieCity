using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SurvivorManager : MonoBehaviour
{
    public SurvivorBase[] survivors;
    private Dictionary<int, SurvivorBase> survivorDict = new();
    private Dictionary<SurvivorBase, FloorBase> survivorFloorDic = new();
    private Dictionary<SurvivorBase, Tween> moveDic = new();

    public void AddSurvivor(SurvivorBase survivor, FloorBase startFloor)
    {
        if (!survivorDict.ContainsKey(survivor.id))
        {
            survivorDict.Add(survivor.id, survivor);
        }
        if (!survivorFloorDic.ContainsKey(survivor))
        {
            survivorFloorDic.Add(survivor, startFloor);
        }
        if (!moveDic.ContainsKey(survivor))
        {
            moveDic.Add(survivor, null);
        }
        SetIdle(survivor);
    }
    public void OnClickSurvivor(SurvivorBase survivor, Vector3 pickPos)
    {
        if (moveDic.ContainsKey(survivor))
        {
            moveDic[survivor].Kill();
            moveDic.Remove(survivor);
        }
        survivor.OnPick(pickPos);

    }
    public void OnClickSurvivorComplete(SurvivorBase survivor, FloorBase floor)
    {
        survivorFloorDic[survivor] = floor;
        Vector2 limitX = floor.GetLimitPositionX();
        float clampedX = Mathf.Clamp(survivor.transform.position.x, limitX.x, limitX.y);
        Vector3 dropPos = new Vector3(clampedX, floor.GetEnterPosition().y, survivor.transform.position.z);
        survivor.OnDrop(dropPos);
        SetIdle(survivor);
    }
    public void SetIdle(SurvivorBase survivor)
    {
        float idleTime = UnityEngine.Random.Range(3f, 4f);
        FloorBase floor = survivorFloorDic[survivor];
        if (floor == null)
        {
            Debug.LogError($"Floor not found for survivor {survivor.name}");
            return;
        }
        Vector2 limitX = floor.GetLimitPositionX();
        float randomX = UnityEngine.Random.Range(limitX.x, limitX.y);
        Vector2 destination = new Vector2(randomX, floor.GetEnterPosition().y);
        moveDic[survivor] = DOVirtual.DelayedCall(idleTime, () =>
        {
            SetMove(survivor, destination, () =>
            {
                SetIdle(survivor);
            });
        });
    }
    public void SetMove(SurvivorBase survivor, Vector2 destination, Action callBack)
    {
        float distance = Vector2.Distance(survivor.transform.position, destination);
        float speed = .3f;
        float duration = distance / speed;
        moveDic[survivor]?.Kill();  
        moveDic[survivor] = survivor.transform.DOMove(destination, duration).OnComplete(() =>
        {
            callBack?.Invoke();
        });
    }
}
