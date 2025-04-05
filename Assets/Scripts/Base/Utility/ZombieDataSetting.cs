using System;
using UnityEngine;
using UnityEngine.UIElements;

public class ZombieDataSetting : ScriptableObject
{
    public ZombieData[] zombieData;
}
[Serializable]
public class ZombieData
{
    public ZombieInfo zombieInfo;
    public int level;
    public int isLock;
    public int poolCount;
}
[Serializable]
public class ZombieInfo
{
    public string name;
    public Sprite icon;
    public ZombieBase zombieBasePrefab;
    public TextField description;
}
