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
    protected FacilityAnimationDataSetting animationDataSetting;
    private FloorInfoData floorInfoData;
    public Action<FloorType, int, FacilityData> onSaveFacility;
    public Action<FloorType, int> onSaveProduct;
    public Action<int, FloorBase, FacilityBase> onShowSurvivor;
    public FloorView floorView;

    public virtual void  Init(FacilityAnimationDataSetting animationDataSetting,FloorView floorView)
    {
        this.animationDataSetting = animationDataSetting;
        this.floorView = floorView;
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
    public virtual void SetWorking(int survivorId, FacilityBase facility)
    {
        string animation = animationDataSetting.GetUseString(floorType, survivorId);
        facility.SetAnimation(animation);
        facility.isUsing = true;
        facility.usingSurvivor = survivorId;
        facility.onSurvivorEndWork += SetNotWorking;

        FacilityData fdata = facility.GetData();
        onSaveFacility?.Invoke(floorType, facility.order, fdata);
    }
    public virtual void SetNotWorking(int survivorId, FacilityBase facility)
    {
        facility.onSurvivorEndWork = null;
        onShowSurvivor?.Invoke(survivorId, this, facility);
        facility.isUsing = false;
        facility.usingSurvivor = 0;
        FacilityData fdata = facility.GetData();
        onSaveFacility?.Invoke(floorType, facility.order, fdata);
    }
    public virtual void SetCollider(bool enabled)
    {
        Collider2D collider = GetComponent<Collider2D>();
        collider.enabled = enabled;
    }
    public virtual void SetMask(bool active)
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
    public virtual void SetProductAmount(double logOutTime)
    {
        if (floorProductText != null)
        {
            floorProductText.text = "logOut Time:" + ((int)logOutTime).ToString() + " sec";
        }
    }
    public virtual void SetFacilityData(FloorInfoData data, double logOutTime)
    {
        floorInfoData = data;
        //處理登出總共秒數 增加物資
        SetProductAmount(logOutTime);
        //初始化設施
        foreach (var facility in data.facilityData)
        {
            int order = facility.Key;
            FacilityData fdata = facility.Value;

            foreach (var facilityBase in facilities)
            {
                if (facilityBase.order == order)
                {
                    facilityBase.Init(fdata);
                    facilityBase.onSurvivorEndWork += SetNotWorking;

                }
            }
        }
    }

}
public enum FloorType
{
    Main = 900,
    Floor_901 = 901,
    Floor_902 = 902,
    Floor_903 = 903,
    Floor_904 = 904,
}
