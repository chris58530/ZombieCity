using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class SurvivorDataSetting : ScriptableObject
{
    public SurvivorData[] survivorData;

}
[Serializable]
public class SurvivorData
{
    public SurvivorInfo survivorInfo;
    public int level;
    public int isLock;
}
[Serializable]
public class SurvivorInfo
{
    public string name;
    public Sprite icon;
    public SurvivorBase survivorBasePrefab;
    public TextField description;
}
