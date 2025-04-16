using System;
using System.Collections.Generic;
using UnityEngine;

public class FacilityAnimationDataSetting : ScriptableObject
{
    public List<FacilityAnimationData> facilityAnimationDatas;

}
[Serializable]
public class FacilityAnimationData
{
    public List<FacilityAnimation> facilityAnimations;
    public int floorId;
}
[Serializable]
public class FacilityAnimation
{
    public int survivorId;
    public string animation;
}

