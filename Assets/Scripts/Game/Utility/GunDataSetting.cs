using System;
using UnityEngine;

public class GunDataSetting : ScriptableObject
{
    public GunData campCarGunData;
    public GunData[] gunDatas;
}
[Serializable]
public class GunData
{
    public int level;
    public AnimationCurve attackCurve;
    public PathMode pathMode;
    public BulletTarget target;
    public BulletType bulletType;
    public Sprite sprite;
    public Transform shootPoint;
    public int ID;

}