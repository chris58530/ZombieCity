using UnityEngine;
using Zenject;
using UnityEngine.UIElements;

public class SurvivorCmd : ICommand
{
    [Inject] private SurvivorProxy proxy;
    public SurvivorDataSetting survivorDataSetting;
    public override void Execute(MonoBehaviour mono)
    {
        isLazy = true;
    }
    [Listener(GameEvent.ON_INIT_GAME)]
    public void InitSurvivor()
    {
        proxy.SetData(survivorDataSetting);
    }
}
