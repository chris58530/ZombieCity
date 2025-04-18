using System;
using System.Collections.Generic;
using UnityEngine;

public class FacilityAnimationDataSetting : ScriptableObject
{
    public List<FacilityAnimationData> facilityAnimationDatas;
    public string GetUseString(FloorType floorType, int survivorId)
    {
        foreach (var data in facilityAnimationDatas)
        {
            if (data.floorType != floorType) continue;

            foreach (var anim in data.facilityUseAnimations)
            {
                if (anim.survivorId == survivorId)
                {
                    return anim.workAnimation;
                }
            }
        }
        return null;
    }
    public string GetIdleString(FloorType floorType)
    {
        foreach (var data in facilityAnimationDatas)
        {
            if (data.floorType != floorType) continue;
            return data.IdleAnimation;
        }
        return null;
    }
}
[Serializable]
public class FacilityAnimationData
{
    public List<FacilityUseAnimation> facilityUseAnimations;
    public string IdleAnimation;

    public FloorType floorType;
}
[Serializable]
public class FacilityUseAnimation
{
    public int survivorId;
    public string workAnimation;
    // public UserAnimation userAnimation;
    //保留 如果將來需要 不只一個動畫
}
public class UserAnimation
{
    public string workAnimation;
    public string tiredAnimation;

}
