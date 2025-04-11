using UnityEngine;
using Zenject;

public class ResourceInfoCmd : ICommand
{
    [Inject] private ResourceInfoProxy proxy;

    
    public override void Execute(MonoBehaviour mono)
    {
        isLazy = true;
        // TODO: implement resource info execution logic
        proxy.SetResource();
    }
 
}
