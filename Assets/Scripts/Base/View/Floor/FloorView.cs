using UnityEngine;
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
        floorManager.floors = new Floor[data.floorData.Length];

        for (int i = 0; i < data.floorData.Length; i++)
        {
            Floor floor = Instantiate(data.floorData[i].floorPrefab);
            floorManager.floors[i] = floor;
            floor.SetMask(data.floorData[i].isLocked);
            floor.name = "Floor_" + i;
            float nextY = data.floorHeight * i;
            floor.transform.position = data.startPosition + new Vector2(0, -nextY);
            floor.transform.parent = floorManagerObj.transform;
        }
    }
}
public class FloorManager : MonoBehaviour
{
    public Floor[] floors;

}
