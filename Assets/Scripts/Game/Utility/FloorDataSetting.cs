using System;
using UnityEngine;

public class FloorDataSetting : ScriptableObject
{
    public FloorBase mainFloorPrefab;
    public Vector2 startPosition;
    public float floorHeight;
    public FloorData[] floorData;

}
[Serializable]
public class FloorData
{
    public FloorBase floorPrefab;
    public int unlockPrice;
    public bool isLocked;
}

