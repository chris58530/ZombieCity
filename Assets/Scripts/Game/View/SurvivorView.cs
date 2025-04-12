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
    public void InitSurvivor(SurvivorDataSetting data, FloorBase startFloor)
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
            survivor.transform.position = new Vector2(randomX, -10);
            survivorManager.AddSurvivor(survivor, startFloor);
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
}

