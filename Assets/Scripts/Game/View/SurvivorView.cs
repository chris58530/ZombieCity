using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class SurvivorView : MonoBehaviour, IView
{
    [Inject] private SurvivorViewMediator medaitor;
    private SurvivorManager survivorManager;

    private void Awake()
    {
        InjectService.Instance.Inject(this);
    }
    private void OnEnable()
    {
        medaitor.Register(this);
    }
    private void OnDisable()
    {
        medaitor.DeRegister(this);
    }
    public void InitSurvivor(SurvivorDataSetting data, Dictionary<int, bool> workingSurvivor, FloorBase startFloor)
    {
        GameObject survivorManagerObj = new GameObject("SurvivorManager");
        survivorManagerObj.transform.SetParent(transform);
        survivorManager = survivorManagerObj.AddComponent<SurvivorManager>();
        survivorManager.onSaveWorkingSurvivor += SaveWorkingSurvivor;
        survivorManager.survivors = new SurvivorBase[data.survivorData.Length];
        for (int i = 0; i < data.survivorData.Length; i++)
        {

            SurvivorBase survivor = Instantiate(data.survivorData[i].survivorInfo.survivorBasePrefab);
            survivorManager.survivors[i] = survivor;
            survivor.name = "Survivor_" + survivor.id;
            survivor.transform.parent = survivorManagerObj.transform;
            float randomX = Random.Range(-2f, 2f);
            survivor.transform.position = new Vector2(randomX, -10);
            if (workingSurvivor.ContainsKey(survivor.id))
            {
                if (workingSurvivor[survivor.id])
                {
                    survivor.SetWorking();
                }
            }
            survivorManager.AddSurvivor(survivor, startFloor);
            medaitor.SetSurvivorDic(survivor.id, survivor);
        }
        for (int i = 0; i < data.survivorData.Length; i++)
        {
            SurvivorBase survivor = survivorManager.survivors[i];

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
    public void OnLeaveFacility(LeaingFacilitySurvivor leavingFacilitySurvivor)
    {
        survivorManager.OnLeaveFacility(leavingFacilitySurvivor);
    }
    public void SaveWorkingSurvivor(int id, bool isWorking)
    {
        medaitor.SaveWorkingSurvivor(id, isWorking);
    }

}

