using UnityEngine;
using Zenject;

public class ResourceInfoCmd : ICommand
{
    [Inject] private ResourceInfoProxy proxy;
    [Inject] private PlayerDataProxy playerDataProxy;

    public override void Execute(MonoBehaviour mono)
    {
        isLazy = true;
        // TODO: implement resource info execution logic
        ResourceInfoData resourceInfoData;
        proxy.SetResource();
    }

}
