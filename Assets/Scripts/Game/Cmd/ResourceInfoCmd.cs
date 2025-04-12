using UnityEngine;
using Zenject;

public class ResourceInfoCmd : ICommand
{
    [Inject] private ResourceInfoProxy proxy;
    [Inject] private PlayerDataProxy playerDataProxy;

    public override void Execute(MonoBehaviour mono)
    {
        isLazy = true;
    }
    [Listener(GameEvent.ON_INIT_GAME)]
    public void SetData()
    {
        ResourceInfoData resourceInfoData = playerDataProxy.GetData().resourceInfoData;
       
        proxy.SetResource(resourceInfoData);
    }

}
