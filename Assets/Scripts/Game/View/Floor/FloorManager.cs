using UnityEngine;

public class FloorManager : MonoBehaviour
{
    public FloorBase[] floors;
    public void SetCollider(bool enabled)
    {
        for (int i = 0; i < floors.Length; i++)
        {
            floors[i].SetCollider(enabled);
        }
    }
    public void SetAnimation()
    {
        
    }
}
