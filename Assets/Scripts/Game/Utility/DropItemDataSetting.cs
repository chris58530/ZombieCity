using UnityEngine;

public class DropItemDataSetting : ScriptableObject
{
    public DropItemData[] dropItemDatas;

}
[System.Serializable]
public class DropItemData
{
    public DropItemObject dropItemObject;
}

