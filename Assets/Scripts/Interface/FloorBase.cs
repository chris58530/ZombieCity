using System;
using System.Collections.Generic;
using NUnit.Framework;
using TMPro;
using UnityEngine;

public class FloorBase : MonoBehaviour
{
    [SerializeField] private TMP_Text floorProductText;
    [SerializeField] private GameObject mask;
    [SerializeField] private Transform enterPosition; //get y
    private Vector2 limitPositionX;
    [SerializeField] private FacilityBase mainFacilitie;
    [SerializeField] private FacilityBase[] facilities;
    public FloorType floorType;
    public string playingAnimation;
    private FacilityAnimationDataSetting animationDataSetting;
    private List<FacilityWorkData> facilityWorkData;
    public Action<int, List<FacilityWorkData>> onSetWorking;
    public Action<int, int> onSetProduct;

    public void Init(FacilityAnimationDataSetting animationDataSetting)
    {
        this.animationDataSetting = animationDataSetting;
    }
    /// <summary>
    /// 登入後會使用到 照登出時正在播哪個動畫SET
    /// </summary>
    public void SetFacilityState()
    {

    }
    public Vector3 GetEnterPosition()
    {
        return enterPosition.position;
    }
    public Vector2 GetLimitPositionX()
    {
        limitPositionX = new Vector2(-2.3f, 2.3f);
        return new Vector2(limitPositionX.x, limitPositionX.y);
    }
    public FacilityBase GetEmptyFacilities()
    {
        for (int i = 0; i < facilities.Length; i++)
        {
            if (!facilities[i].isUsing)
            {
                facilities[i].isUsing = true;
                return facilities[i];
            }
        }
        return null;
    }
    public void SetWorking(int survivorId, FacilityBase facility)
    {
        string animation = animationDataSetting.GetUseString(floorType, survivorId);
        facility.SetAnimation(animation);

        for (int i = 0; i < facilities.Length && i < facilities.Length; i++)
        {
            facilityWorkData[i].animationString = facilities[i].animationName;
            facilityWorkData[i].isUsing = facilities[i].isUsing;
        }
        onSetWorking?.Invoke((int)floorType, facilityWorkData);

    }
    public void SetNotWorking(FacilityBase facility)
    {
        string animation = animationDataSetting.GetIdleString(floorType);
        onSetWorking?.Invoke((int)floorType, facilityWorkData);
        facility.SetAnimation(animation);
    }
    public void SetCollider(bool enabled)
    {
        Collider2D collider = GetComponent<Collider2D>();
        collider.enabled = enabled;
    }

    public void SetMask(bool active)
    {
        mask.SetActive(active);
    }

    public FacilityBase CheckEmptyFacility()
    {
        for (int i = 0; i < facilities.Length; i++)
        {
            if (facilities[i].isUsing)
            {
                return facilities[i];
            }
        }
        return null;
    }
    public void SetProductAmount(double logOutTime)
    {
        if (floorProductText != null)
        {
            floorProductText.text = "logOut Time:" + ((int)logOutTime).ToString() + " sec";
        }
    }
    public void SetFacilityData(List<FacilityWorkData> facilityWorkDatas, double logOutTime)
    {
        this.facilityWorkData = facilityWorkDatas;
        SetProductAmount(logOutTime);
        //處理登出總共秒數 增加物資
        for (int i = 0; i < facilityWorkDatas.Count && i < facilities.Length; i++)
        {
            FacilityWorkData data = facilityWorkDatas[i];
            FacilityBase facility = facilities[i];
            facility.isUsing = data.isUsing;
            if (data.isUsing)
            {
                facility.SetAnimation(data.animationString);
            }
            facility.SetStartTime(data.startTime);
            facility.SetEfficientTime(data.efficientTime);
        }
    }
    public void SaveFacilityData()
    {

    }
}
public enum FloorType
{
    Main,
    Floor_901,
    Floor_902,
    Floor_903,
    Floor_904,
}
