using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SurvivorManager : MonoBehaviour
{
    public SurvivorBase[] survivors;
    private Dictionary<int, SurvivorBase> survivorDict = new Dictionary<int, SurvivorBase>();
    public void AddSurvivor(SurvivorBase survivor)
    {
        if (!survivorDict.ContainsKey(survivor.id))
        {
            survivorDict.Add(survivor.id, survivor);
        }
    }
    public void OnClickSurvivor(SurvivorBase survivor, Vector3 pickPos)
    {
        Debug.Log("SurvivorManager: OnClickSurvivor" + survivor.name);
        survivor.OnPick(pickPos);

    }
    public void OnClickSurvivorComplete(SurvivorBase survivor, Vector3 floorPos)
    {
        Debug.Log($"{survivor.name} in :{floorPos}");
        survivor.OnDrop(floorPos);

    }
    public void MoveAuto()
    {

    }
    public void MoveSurvivor(int id, Transform enter, Transform facility, Action callBack)
    {
        if (survivorDict.ContainsKey(id))
        {
            SurvivorBase survivor = survivorDict[id];
            if (survivor == null)
            {
                Debug.LogError($"Survivor with ID {id} not found.");
                return;
            }
            Transform self = survivor.transform;
            survivor.SetFlip(GameDefine.IsFlipByLocal(self, enter));
            survivor.isBusy = true;
            float moveX = enter.position.x;
            float currentX = survivor.transform.position.x;
            float distance = Mathf.Abs(moveX - currentX);
            float speed = 2f;
            float duration = distance / speed;
            survivor.SetBusy(true);
            survivor.transform.DOMoveX(moveX, duration).SetEase(Ease.Linear).OnComplete(() =>
            {
                survivor.transform.DOMove(enter.position, 0.1f).OnComplete(() =>
                {
                    survivor.SetFlip(GameDefine.IsFlipByLocal(self, facility));
                    survivor.transform.DOMoveX(facility.position.x, duration).OnComplete(() =>
                    {
                        survivor.SetBusy(false);
                        callBack?.Invoke();
                    });
                });
            });
        }
    }
}
