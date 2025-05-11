using System;
using System.Collections.Generic;
using NUnit.Framework;
using TMPro;
using UnityEngine;

public class FloorBase : MonoBehaviour
{
    public FloorType floorType;
    [Header("場地限制")]
    [SerializeField] private Transform enterPosition; //get y
    [SerializeField] private Vector2 limitPositionX;

    [Header("顯示數值")]
    [SerializeField] private TMP_Text productText;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private GameObject mask;
    protected FacilityAnimationDataSetting animationDataSetting;
    protected FloorJsonData floorInfoData;
    protected FloorView floorView;
    public Action<FloorType, int> onSaveProduct;
    public Action<FloorType, int> onSaveLevel;

    public virtual void Init(FacilityAnimationDataSetting animationDataSetting, FloorView floorView)
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
        if (limitPositionX == null)
            limitPositionX = new Vector2(-2.3f, 2.3f);
        return limitPositionX;
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
    public virtual void SetData(FloorJsonData data, double logOutTime)
    {
        floorInfoData = data;
        SetProductAmount(data.productAmount);
        SetLevel(data.level);
    }
    public virtual void SetProductAmount(int amount)
    {
        productText.text = "Product:" + amount.ToString();
        onSaveProduct?.Invoke(floorType, floorInfoData.productAmount);

    }
    public void OnAddProduct(int amount)
    {
        floorInfoData.productAmount += amount;
        SetProductAmount(floorInfoData.productAmount);
    }
    public virtual void SetLevel(int level)
    {
        levelText.text = "Level:" + level.ToString();
        onSaveLevel?.Invoke(floorType, floorInfoData.level);
    }
    public virtual void OnAddLevel(int amount)
    {
        floorInfoData.level++;
        SetLevel(floorInfoData.level);
        onSaveProduct?.Invoke(floorType, floorInfoData.productAmount);
    }
}
public enum FloorType
{
    Main = 900,
    Floor_901 = 901,
    Floor_902 = 902,
    Floor_903 = 903,
    Floor_904 = 904,
    Floor_905 = 905,
}
