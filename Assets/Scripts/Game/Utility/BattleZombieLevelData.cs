using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BattleZombieLevelData : ScriptableObject
{
    public int zombieID;
    public List<ZombieLevelData> zombieLevelDatas;
    public int GetHp(int level)
    {
        foreach (var data in zombieLevelDatas)
        {
            if (zombieID == level)
            {
                return Mathf.RoundToInt(data.hpCurve.Evaluate(level));
            }
        }
        return 0; // Default value if not found
    }
    public int GetAttack(int level)
    {
        foreach (var data in zombieLevelDatas)
        {
            if (zombieID== level)
            {
                return Mathf.RoundToInt(data.attkCurve.Evaluate(level));
            }
        }
        return 0; // Default value if not found
    }
}
[Serializable]
public class ZombieLevelData
{
    public AnimationCurve hpCurve;
    public AnimationCurve attkCurve;
}

