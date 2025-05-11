using UnityEngine;
using Zenject;

public class DebugView : MonoBehaviour, IView
{
    [Inject] private Listener listener;
    [Inject] private FloorProxy floorProxy;
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
        if (Input.GetKeyDown(KeyCode.Q))
        {
            OnAddProductFloor1();
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            OnAddLevelFloor1();
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            OnAddProductFloor2();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            OnAddLevelFloor2();
        }
    }
    public void SpawnZombie()
    {
        listener.BroadCast(DebugEvent.ON_ZOMBIE_SPAWN);
    }
    public void OnAddProductFloor1()
    {
        floorProxy.AddProduct(FloorType.Floor_901, 1);
    }
    public void OnAddLevelFloor1()
    {
        floorProxy.AddLevel(FloorType.Floor_901, 1);
    }
     public void OnAddProductFloor2()
    {
        floorProxy.AddProduct(FloorType.Floor_902, 1);
    }
    public void OnAddLevelFloor2()
    {
        floorProxy.AddLevel(FloorType.Floor_902, 1);
    }
    
}
