using UnityEngine;
using Zenject;

// Command
public class TrasitionBackGroundCmd : ICommand
{
    [Inject] private TrasitionBackGroundProxy proxy;

    public override void Execute(MonoBehaviour mono)
    {
        // 執行背景轉換的指令
        proxy.InitData();
    }
}
