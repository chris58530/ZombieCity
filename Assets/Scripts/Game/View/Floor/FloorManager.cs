using System.Collections.Generic;
using UnityEngine;

public class FloorManager : MonoBehaviour
{
    public FloorBase[] floors;
    public void SetAllCollider(bool enabled)
    {
        for (int i = 0; i < floors.Length; i++)
        {
            floors[i].SetCollider(enabled);
        }
    }
    public void AddProduct(FloorType floorType, int amount)
    {
        for (int i = 0; i < floors.Length; i++)
        {
            if (floors[i].floorType == floorType)
            {
                floors[i].OnAddProduct(amount);
            }
        }
    }
    public void AddLevel(FloorType floorType, int level)
    {
        for (int i = 0; i < floors.Length; i++)
        {
            if (floors[i].floorType == floorType)
            {
                floors[i].OnAddLevel(level);
            }
        }
    }
}
