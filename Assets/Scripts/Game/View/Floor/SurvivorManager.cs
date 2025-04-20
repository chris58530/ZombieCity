using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SurvivorManager : MonoBehaviour
{
    public SurvivorBase[] survivors;
    private Dictionary<int, SurvivorBase> survivorDict = new();
    private Dictionary<SurvivorBase, FloorBase> survivorFloorDic = new();
    private Dictionary<SurvivorBase, FacilityBase> survivorFacilityDic = new();
    private Dictionary<SurvivorBase, Tween> tweenDic = new();
    public Action<int, bool> onSaveWorkingSurvivor;

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
        if (!tweenDic.ContainsKey(survivor))
        {
            tweenDic.Add(survivor, null);
        }
        SetSurvivorIdle(survivor);
    }
    public void OnClickSurvivor(SurvivorBase survivor, Vector3 pickPos)
    {
        if (tweenDic.ContainsKey(survivor))
        {
            tweenDic[survivor].Kill();
            tweenDic.Remove(survivor);

        }
        if (survivorFacilityDic.ContainsKey(survivor))
        {
            survivorFacilityDic[survivor].isUsing = false;
            survivorFacilityDic.Remove(survivor);
        }
        survivor.sprite.color = Color.white;

        survivor.OnPick(pickPos);

    }
    public void OnClickSurvivorComplete(SurvivorBase survivor, FloorBase floor)
    {
        survivorFloorDic[survivor] = floor;
        Vector2 limitX = floor.GetLimitPositionX();
        float clampedX = Mathf.Clamp(survivor.transform.position.x, limitX.x, limitX.y);
        Vector3 dropPos = new Vector3(clampedX, floor.GetEnterPosition().y, survivor.transform.position.z);
        survivor.OnDrop(dropPos);
        SetSurvivorIdle(survivor);
    }
    public void OnLeaveFacility(LeaingFacilitySurvivor leavingFacilitySurvivor)
    {
        SurvivorBase survivor = leavingFacilitySurvivor.survivor;
        FloorBase floor = leavingFacilitySurvivor.floor;
        FacilityBase facility = leavingFacilitySurvivor.facility;
        survivorFloorDic[survivor] = floor;
        float facilityX = facility.transform.position.x;
        survivor.transform.position = new Vector3(facilityX, 0);
        Vector3 dropPos = new Vector3(facilityX, floor.GetEnterPosition().y, survivor.transform.position.z);
        survivor.OnDrop(dropPos);
        SetSurvivorIdle(survivor, true);
    }
    public void SetSurvivorIdle(SurvivorBase survivor, bool skipFacility = false)
    {
        if (survivor.isWorking) return;
        float idleTime = UnityEngine.Random.Range(3f, 4f);
        FloorBase floor = survivorFloorDic[survivor];
        if (floor == null)
        {
            Debug.LogError($"Floor not found for survivor {survivor.name}");
            return;
        }
        //檢查當下的樓層是否有空的設施
        FacilityBase targetFacility = floor.GetEmptyFacilities();
        if (targetFacility != null && !skipFacility)
        {
            survivorFacilityDic[survivor] = targetFacility;
            survivor.sprite.color = Color.green;
            SetSurvivorMove(survivor, targetFacility.transform.position, () =>
            {
                //tiredTime 之後建立新的設定檔 設施/倖存者編號、秒數
                //StartWork之後倖存者會關閉 直到拖拽點擊設施又重新出現
                survivor.SetWorking();
                floor.SetWorking(survivor.id, targetFacility);
                onSaveWorkingSurvivor?.Invoke(survivor.id, true);
                tweenDic[survivor]?.Kill();
            });
            return;
        }
        Vector2 limitX = floor.GetLimitPositionX();
        float randomX = UnityEngine.Random.Range(limitX.x, limitX.y);
        Vector2 destination = new Vector2(randomX, floor.GetEnterPosition().y);
        tweenDic[survivor] = DOVirtual.DelayedCall(idleTime, () =>
        {
            SetSurvivorMove(survivor, destination, () =>
            {
                SetSurvivorIdle(survivor);
            });
        });
    }
    public void SetSurvivorMove(SurvivorBase survivor, Vector2 destination, Action callBack)
    {
        float distance = Vector2.Distance(survivor.transform.position, destination);
        float speed = .3f;
        float duration = distance / speed;
        tweenDic[survivor] = survivor.transform.DOMove(destination, duration).SetEase(Ease.Linear).OnComplete(() =>
        {
            callBack?.Invoke();
        });
    }

}
