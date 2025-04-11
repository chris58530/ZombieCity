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
    //邏輯資料
    public ZombieInfo zombieInfo;
    public int hp;
    public int money;
    public int isLock;
    
}
[Serializable]
public class ZombieInfo
{
    //外觀資料
    public string name;
    public Sprite icon;
    public ZombieBase zombieBasePrefab;
    public TextField description;
}
