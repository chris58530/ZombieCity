using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class FloorView : MonoBehaviour, IView
{
    [Inject] private FloorViewMediator mediator;
    private FloorManager floorManager;
    [SerializeField] private FacilityAnimationDataSetting facilityAnimationDataSetting;
    private void OnEnable()
    {
        InjectService.Instance.Inject(this);

        mediator.Register(this);
    }
    private void OnDisable()
    {
        mediator.DeRegister(this);
    }
    public void InitFloor(FloorDataSetting data, FloorProductData productData, double logOutTime)
    {
        //Init InitFloorManager
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
            floor.SetMask(data.floorData[i].isLock);
            floor.name = "Floor_" + i;
            float nextY = data.floorHeight * i;
            floor.transform.position = data.startPosition + new Vector2(0, -nextY);
            floor.transform.parent = floorManagerObj.transform;
            floor.SetCollider(false);
            floor.Init(facilityAnimationDataSetting);
        }

        //Init Facility
        foreach (KeyValuePair<int, List<FacilityWorkData>> kvp in productData.FloorFacility)
        {
            int floorId = kvp.Key;
            List<FacilityWorkData> facilities = kvp.Value;
            foreach (FloorBase floor in floorManager.floors)
            {
                if ((int)floor.floorType == floorId)
                {
                    floor.SetFacilityData(facilities, logOutTime);
                    floor.onSetWorking += SaveFacilities;
                    floor.onSetProduct += SetFloorProduct;
                    break;
                }
            }
        }
        mediator.OnInitCompelet();
    }
    public void SetCollider(bool enabled)
    {
        floorManager.SetCollider(enabled);
    }
    public void SetAnimation()
    {

    }
    public void SaveFacilities(int floorID, List<FacilityWorkData> facilityWorkData)
    {
        mediator.SaveFacilities(floorID, facilityWorkData);
    }
    public void SetFloorProduct(int floorID, int amount)
    {
        mediator.SetFloorProduct(floorID, amount);

    }
}
