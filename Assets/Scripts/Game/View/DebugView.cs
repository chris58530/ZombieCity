using UnityEngine;
using Zenject;

public class DebugView : MonoBehaviour, IView
{
    [Inject] private Listener listener;
    private void OnEnable()
    {
        listener.RegisterListener(this);
        InvokeRepeating("SpawnZombie", 0, 1f);

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
    public void SpawnZombie()
    {
        listener.BroadCast(DebugEvent.ON_ZOMBIE_SPAWN);
    }
}
