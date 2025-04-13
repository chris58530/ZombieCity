using System;
using UnityEngine;
using UnityEngine.UIElements;

public class SurvivorDataSetting : ScriptableObject
{
    public SurvivorData[] survivorData;

}
[Serializable]
public class SurvivorData //存檔設定
{
    public SurvivorInfo survivorInfo;
    public int level;
    public bool isLock;
}
[Serializable]
public class SurvivorInfo //基本設定
{
    public string name; //UI
    public Sprite icon;
    public SurvivorBase survivorBasePrefab;
    public TextField description;
}
