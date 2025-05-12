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
    /// <summary>
    /// <paramref name="data"/> 基礎樓層設定
    /// <paramref name="floorInfoData"/> Json儲存資料
    /// <paramref name="logOutTime"/> 登出時間，計算離線收益
    /// </summary>
    public void InitFloor(FloorDataSetting data, Dictionary<int, FloorJsonData> floorInfoData, double logOutTime)
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
        mainFloor.Init(facilityAnimationDataSetting, this);
        mediator.SetMainFloor(mainFloor);
        mediator.SetFloor(mainFloor);

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
            floor.Init(facilityAnimationDataSetting, this);
            mediator.SetFloor(floor);
        }
        //設定儲存資料
        foreach (KeyValuePair<int, FloorJsonData> kvp in floorInfoData)
        {
            int floorId = kvp.Key;
            FloorJsonData floorInfo = kvp.Value;
            foreach (FloorBase floor in floorManager.floors)
            {
                if ((int)floor.floorType == floorId)
                {
                    Debug.Log("InitFloor:" + floorId + "產品數量：" + floorInfo.productAmount + " 等級：" + floorInfo.level);
                    floor.onSaveProduct += SaveFloorProduct;
                    floor.onSaveLevel += SaveFloorLevel;
                    floor.SetData(floorInfo, logOutTime);
                    break;
                }
            }
        }
        mediator.OnInitCompelet();
    }
    public void SetAllCollider(bool enabled)
    {
        floorManager.SetAllCollider(enabled);
    }
    public void AddProduct(FloorType floorType, int amount)
    {
        floorManager.AddProduct(floorType, amount);
    }
    public void AddLevel(FloorType floorType, int level)
    {
        floorManager.AddLevel(floorType, level);
    }
    public void SaveFloorProduct(FloorType floorType, int amount)
    {
        mediator.SaveFloorProduct(floorType, amount);
    }
    public void SaveFloorLevel(FloorType floorType, int level)
    {
        mediator.SaveFloorLevel(floorType, level);
    }
    public void OnClickSkyWatcher()
    {
        //MainFloor 使用
        mediator.OnClickSkyWatcher();
    }
}
