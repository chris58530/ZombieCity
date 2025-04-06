using UnityEngine;
using Zenject;

public class DebugView : MonoBehaviour, IView
{
    [Inject] private Listener listener;
    private void OnEnable()
    {
        listener.RegisterListener(this);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            listener.BroadCast(DebugEvent.ON_DEBUG_EVENT);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            listener.BroadCast(DebugEvent.ON_ZOMBIE_SPAWN);
        }
    }
}
