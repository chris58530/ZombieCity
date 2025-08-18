using System;
using UnityEngine;
using UnityEngine.UIElements;

public class SafeZombieDataSetting : ScriptableObject
{
    public SafeZombieData[] zombieData;
}
[Serializable]
public class SafeZombieData
{
    //邏輯資料
    public SafeZombieInfo zombieInfo;
    public int hp;
    public int money;
    public bool isLock;

}
[Serializable]
public class SafeZombieInfo
{
    //外觀資料
    public string name;
    public Sprite icon;
    public SafeZombieBase zombieBasePrefab;
    public TextField description;
}
