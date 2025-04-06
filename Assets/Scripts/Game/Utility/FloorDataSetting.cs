using System;
using UnityEngine;

public class FloorDataSetting : ScriptableObject
{
    public Vector2 startPosition;
    public float floorHeight;
    public FloorData[] floorData;

}
[Serializable]
public class FloorData
{
    public Floor floorPrefab;
    public int unlockPrice;
    public bool isLocked;
}

