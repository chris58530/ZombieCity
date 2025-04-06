using UnityEngine;

public class ClickHitCmd : ICommand
{
    public override void Execute(MonoBehaviour mono)
    {
        isLazy = true;
    }

}
