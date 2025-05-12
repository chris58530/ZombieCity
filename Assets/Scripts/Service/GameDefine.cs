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
    public static bool IsFlipByWorld(float direction)
    {
        if (direction > 0)
            return true;
        else
            return false;
    }
    public static bool IsFlipByLocal(Transform self, Transform destination)
    {
        if (self.position.x > destination.position.x)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public static float GetSurvivorZ()
    {
        return -0.01f;
    }
}
