using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class FloorView : MonoBehaviour, IView
{
    [Inject] private FloorViewMedaitor medaitor;
    private FloorManager floorManager;
    private void OnEnable()
    {
        InjectService.Instance.Inject(this);

        medaitor.Register(this);
    }
    private void OnDisable()
    {
        medaitor.DeRegister(this);
    }
    public void InitFloor(FloorDataSetting data)
    {
        GameObject floorManagerObj = new GameObject("FloorManager");
        floorManagerObj.transform.SetParent(transform);
        floorManager = floorManagerObj.AddComponent<FloorManager>();
        floorManager.floors = new FloorBase[data.floorData.Length + 1];//main floor

        FloorBase mainFloor = Instantiate(data.mainFloorPrefab);
        mainFloor.transform.position = Vector3.zero;
        mainFloor.transform.parent = floorManagerObj.transform;
        floorManager.floors[0] = mainFloor;
        mainFloor.name = "MainFloor";
        mainFloor.SetCollider(false);
        for (int i = 0; i < data.floorData.Length; i++)
        {
            FloorBase floor = Instantiate(data.floorData[i].floorPrefab);
            floorManager.floors[i + 1] = floor;//i + 1 因main floor佔位
            floor.SetMask(data.floorData[i].isLocked);
            floor.name = "Floor_" + i;
            float nextY = data.floorHeight * i;
            floor.transform.position = data.startPosition + new Vector2(0, -nextY);
            floor.transform.parent = floorManagerObj.transform;
            floor.SetCollider(false);
        }
    }
    public void SetCollider(bool enabled)
    {
        floorManager.SetCollider(enabled);
    }


}
