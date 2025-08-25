using System;
using UnityEngine;


public class SurvivorDataSetting : ScriptableObject
{
    public SurvivorData[] survivorData;
    public SurvivorData GetSurvivorData(int id)
    {
        if (id < 0 || id >= survivorData.Length) return null;
        return survivorData[id];
    }

}
[Serializable]
public class SurvivorData //存檔設定
{
    public SurvivorInfo survivorInfo;
    public int level;
}
[Serializable]
public class SurvivorInfo //基本設定
{
    public string name; //UI
    public UnityEngine.UI.Image icon;
    public SurvivorBase survivorBasePrefab;
    public UnityEngine.UIElements.TextField description;
}
