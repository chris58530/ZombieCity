using System;
using UnityEngine;

public class GunAnimationEventHandler : MonoBehaviour
{
    public int gunSeat;
    public Action<int> onShootAnimationEvent;
    public void OnShootAnimationEvent()
    {
        onShootAnimationEvent?.Invoke(gunSeat);
    }
}
