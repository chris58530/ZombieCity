using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class FloorView : MonoBehaviour, IView
{
    [Inject] private FloorViewMedaitor medaitor;
    private FloorManager floorManager;
    private SurvivorManager survivorManager;
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
    public void InitSurvivor(SurvivorDataSetting data)
    {
        GameObject survivorManagerObj = new GameObject("SurvivorManager");
        survivorManagerObj.transform.SetParent(transform);
        survivorManager = survivorManagerObj.AddComponent<SurvivorManager>();
        survivorManager.survivors = new SurvivorBase[data.survivorData.Length];
        for (int i = 0; i < data.survivorData.Length; i++)
        {
            if (GameDefine.IsLock(data.survivorData[i].isLock))
            {
                continue;
            }

            SurvivorBase survivor = Instantiate(data.survivorData[i].survivorInfo.survivorBasePrefab);
            survivorManager.survivors[i] = survivor;
            survivor.name = "Survivor_" + survivor.id;
            survivor.transform.parent = survivorManagerObj.transform;
            float randomX = Random.Range(-2f, 2f);
            survivor.transform.position = new Vector2(randomX,-10);
            survivorManager.AddSurvivor(survivor);
        }

    }
}
public class FloorManager : MonoBehaviour
{
    public Floor[] floors;

}
public class SurvivorManager : MonoBehaviour
{
    public SurvivorBase[] survivors;
    public Dictionary<int, SurvivorBase> survivorDict = new Dictionary<int, SurvivorBase>();
    public void AddSurvivor(SurvivorBase survivor)
    {
        if (!survivorDict.ContainsKey(survivor.id))
        {
            survivorDict.Add(survivor.id, survivor);
        }
    }
}
