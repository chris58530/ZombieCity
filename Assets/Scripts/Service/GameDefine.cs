using UnityEngine;

public class GameDefine
{
    public static bool IsLock(int num)
    {
        if (num == 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    public static bool IsFlip(float direction)
    {
        if (direction > 0)
            return true;
        else
            return false;
    }
}
