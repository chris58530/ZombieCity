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
    private Dictionary<SurvivorBase, Sequence> animationSeqDic = new();
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
        SetSurvivorIdle(survivor, startFloor);
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
        if (animationSeqDic.ContainsKey(survivor))
        {
            Debug.Log($"Killing animation sequence for survivor {survivor.id}");
            animationSeqDic[survivor].Kill();
            animationSeqDic.Remove(survivor);
        }
        survivor.OnPick(pickPos);

    }
    public void OnClickSurvivorComplete(SurvivorBase survivor, FloorBase floor)
    {
        survivorFloorDic[survivor] = floor;
        Vector2 limitX = floor.GetLimitPositionX();
        float clampedX = Mathf.Clamp(survivor.transform.position.x, limitX.x, limitX.y);
        Vector3 dropPos = new Vector3(clampedX, floor.GetEnterPosition().y, survivor.transform.position.z);
        survivor.OnDrop(dropPos, floor.floorType);
        SetSurvivorIdle(survivor, floor);
    }
    public void SetSurvivorIdle(SurvivorBase survivor, FloorBase floor)
    {
        survivor.PlayAnimation("Idle", () =>
        {
            int repeatCount = UnityEngine.Random.Range(5, 10);

            animationSeqDic[survivor] = DOTween.Sequence();
            Action finishCallback = () =>
            {
                SetSurvivorMove(survivor, floor.GetLimitPositionX(), () =>
                 {
                     SetSurvivorIdle(survivor, floor);
                 });
            };
            string clipName = "Survivior" + survivor.id + "_" + floor.survivorAnimation.ToString(); ;
            //TODO clipName wrong
            // if (floor.survivorAnimation == string.Empty || survivor.GetAnimationView().GetAnimationLengthByClipName(clipName) <= 0)
            // {
            //     finishCallback?.Invoke();
            //     Debug.LogWarning($"Floor {floor.floorType} has no survivor animation set.");
            //     return;
            // }
            // 播放 floor.survivorAnimation 動畫

            for (int i = 0; i < repeatCount; i++)
            {
                float animationTime = survivor.GetAnimationView().GetAnimationLength(floor.survivorAnimation);
                animationSeqDic[survivor].AppendCallback(() =>
              {
                  survivor.PlayAnimation(floor.survivorAnimation);
                  Debug.Log($"Survivor playing animation {survivor.id} / {animationTime} /{floor.survivorAnimation} on floor {floor.floorType}");
              });
                // 等待動畫播完（
                animationSeqDic[survivor].AppendInterval(0.84f); //TODO 操 這個方法是壞的 先哈扣
            }
            animationSeqDic[survivor].OnComplete(() =>
          {
              finishCallback?.Invoke();
          });
        });
    }
    public void SetSurvivorMove(SurvivorBase survivor, Vector2 limitPositionX, Action callBack)
    {
        survivor.PlayAnimation("Move");
        float moveX = UnityEngine.Random.Range(limitPositionX.x, limitPositionX.y);
        Vector2 destination = new Vector3(moveX, survivor.transform.position.y, survivor.transform.position.z);
        float distance = Vector2.Distance(survivor.transform.position, destination);
        float speed = .3f;
        float duration = distance / speed;
        tweenDic[survivor] = survivor.transform.DOMove(destination, duration).SetEase(Ease.Linear).OnComplete(() =>
        {
            callBack?.Invoke();
        });
    }
    public void AddLevel(int id, int amount)
    {
        if (survivorDict.ContainsKey(id))
        {
            survivorDict[id].OnAddLevel(amount);
        }
    }
    public void SetStayingFloor(int id, FloorType floorType)
    {
        if (survivorDict.ContainsKey(id))
        {
            survivorDict[id].OnSetStayingFloor(floorType);
        }
    }

}
