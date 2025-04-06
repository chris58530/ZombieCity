using UnityEngine;
using Zenject;

public class GameCameraCmd : ICommand
{
    [Inject] private GameCameraProxy proxy;
    [Inject]private FloorProxy floorProxy;
    public override void Execute(MonoBehaviour mono)
    {
        isLazy = true;
        proxy.minY = -18;
        proxy.EnabelSwipe();
    }




}
