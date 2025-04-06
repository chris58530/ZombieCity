using UnityEngine;

public class TestCmd : ICommand
{
    public override void Execute(MonoBehaviour mono)
    {
        isLazy = true;
    }

    [Listener(DebugEvent.ON_DEBUG_EVENT)]
    public void InitFloor()
    {
        LogService.Instance.Log("TestCmd InitFloor");
    }
}
