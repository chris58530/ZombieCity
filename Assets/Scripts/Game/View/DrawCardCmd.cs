using UnityEngine;

public class DrawCardCmd : ICommand
{
    private DrawCardProxy proxy;
    public override void Execute(MonoBehaviour mono)
    {
        isLazy = true;
    }
    public void OnDrawCardShow()
    {
        proxy.InitData();
    }
}
