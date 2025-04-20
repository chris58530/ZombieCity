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
    public void InitFloor(FloorDataSetting data, Dictionary<int, FloorInfoData> floorInfoData, double logOutTime)
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
            floor.name = "Floor_" + (int)floor.floorType;
            float nextY = data.floorHeight * i;
            floor.transform.position = data.startPosition + new Vector2(0, -nextY);
            floor.transform.parent = floorManagerObj.transform;
            floor.SetCollider(false);
            floor.Init(facilityAnimationDataSetting);
        }

        //Init Facility
        foreach (KeyValuePair<int, FloorInfoData> kvp in floorInfoData)
        {
            int floorId = kvp.Key;
            FloorInfoData floorInfo = kvp.Value;
            foreach (FloorBase floor in floorManager.floors)
            {
                Debug.Log("floorId: " + floorId + " floorType: " + (int)floor.floorType);
                if ((int)floor.floorType == floorId)
                {
                    Debug.Log("floorId: " + floorId);
                    floor.SetFacilityData(floorInfo, logOutTime);
                    floor.onSaveFacility += SaveFacilities;
                    floor.onSaveProduct += SaveFloorProduct;
                    floor.onShowSurvivor += RequestShowSurvivor;
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

    public void RequestShowSurvivor(int survivorID, FloorBase floor, FacilityBase facility)
    {
        mediator.RequestShowSurvivor(survivorID,floor, facility);
    }
    public void SaveFacilities(FloorType floorType, int order, FacilityData fdata)
    {
        mediator.SaveFacilities(floorType, order, fdata);
    }
    public void SaveFloorProduct(FloorType floorType, int amount)
    {
        mediator.SaveFloorProduct(floorType, amount);

    }
}
