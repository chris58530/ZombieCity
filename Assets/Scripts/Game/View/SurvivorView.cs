using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class SurvivorView : MonoBehaviour, IView
{
    [Inject] private SurvivorViewMediator mediator;
    private SurvivorManager survivorManager;

    private void Awake()
    {
        InjectService.Instance.Inject(this);
    }
    private void OnEnable()
    {
        mediator.Register(this);
    }
    private void OnDisable()
    {
        mediator.DeRegister(this);
    }
    public void InitSurvivor(SurvivorDataSetting data, Dictionary<int, SurvivorJsonData> infoData, FloorBase startFloor, Dictionary<int, FloorBase> floorBaseDic)
    {
        Debug.Log("InitSurvivor");
        GameObject survivorManagerObj = new GameObject("SurvivorManager");
        survivorManagerObj.transform.SetParent(transform);
        survivorManager = survivorManagerObj.AddComponent<SurvivorManager>();
        survivorManager.survivors = new SurvivorBase[data.survivorData.Length];
        for (int i = 0; i < data.survivorData.Length; i++)
        {

            SurvivorBase survivor = Instantiate(data.survivorData[i].survivorInfo.survivorBasePrefab);
            survivorManager.survivors[i] = survivor;
            survivor.name = "Survivor_" + survivor.id;
            survivor.transform.parent = survivorManagerObj.transform;
            float randomX = Random.Range(-2f, 2f);
            survivor.transform.position = new Vector2(randomX, -10);
            survivorManager.AddSurvivor(survivor, startFloor);
            mediator.SetSurvivorDic(survivor.id, survivor);
        }
        foreach (KeyValuePair<int, SurvivorJsonData> kvp in infoData)
        {
            int id = kvp.Key;
            SurvivorJsonData survivorInfo = kvp.Value;
            foreach (SurvivorBase survivor in survivorManager.survivors)
            {
                if (survivor.id == id)
                {
                    survivor.onSaveStayingFloor = SaveStayingFloor;
                    survivor.onSaveLevel = SaveLevel;
                    survivor.SetData(survivorInfo);
                    survivor.SetStayingFloor(floorBaseDic[survivorInfo.stayingFloor] == null ? startFloor : floorBaseDic[survivorInfo.stayingFloor]);
                    break;
                }
            }
        }
    }

    public void OnClickSurvivor(SurvivorBase survivor, Vector3 pickPos)
    {
        survivorManager.OnClickSurvivor(survivor, pickPos);
    }
    public void OnClickSurvivorComplete(SurvivorBase survivor, FloorBase floor)
    {
        survivorManager.OnClickSurvivorComplete(survivor, floor);
    }
    public void AddLevel(int id, int amount)
    {
        survivorManager.AddLevel(id, amount);
    }
    public void SetStayingFloor(int id, FloorType floorType)
    {
        survivorManager.SetStayingFloor(id, floorType);
    }
    public void SaveStayingFloor(int id, FloorType floorType)
    {
        mediator.SaveSurvivorStayingFloor(id, floorType);
    }
    public void SaveLevel(int id, int level)
    {
        mediator.SaveSurvivorLevel(id, level);
    }
}

