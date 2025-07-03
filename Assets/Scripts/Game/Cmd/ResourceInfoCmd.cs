using UnityEngine;
using Zenject;

public class ResourceInfoCmd : ICommand
{
    [Inject] private ResourceInfoProxy proxy;
    [Inject] private JsonDataProxy playerDataProxy;

    public override void Execute(MonoBehaviour mono)
    {
        isLazy = true;
    }
    [Listener(GameEvent.ON_GAME_STATE_START)]
    public void SetData()
    {
        ResourceJsonData resourceInfoData = playerDataProxy.GetData().resourceInfoData;
       
        proxy.SetResource(resourceInfoData);
    }

}
