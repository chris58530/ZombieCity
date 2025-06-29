using System;
using UnityEngine;

public class BattleCampCarView : MonoBehaviour, IView
{
    // [Zenject.Inject] private BattleCampCarViewMediator mediator;
    [SerializeField] private GameObject root;
    [SerializeField] private BattleCampCarController battleCampCarController;
    [SerializeField] private GameObject followCamera;
    private BattleZombieSpawnData battleSetting;
    [Header("Test")]
    public bool isTest = false;
    public GameObject testRoot;
    private void Awake()
    {
        if (!isTest)
        {
            Destroy(testRoot);
        }
        // InjectService.Instance.Inject(this);
        root.SetActive(false);
        ShowBattleCampCar();
    }

    private void OnEnable()
    {
        // mediator.Register(this);
    }

    private void OnDisable()
    {
        // mediator.DeRegister(this);
    }
    public void SetBattleSetting(BattleZombieSpawnData setting)
    {
        battleSetting = setting;
    }
    public void ShowBattleCampCar()
    {
        root.SetActive(true);
    }

    public void HideBattleCampCar()
    {
        root.SetActive(false);
    }
}
[Serializable]
public class WaveSetting
{
    public float triggerSecond;
    public ZombieSpwnSetting[] zombieSpwnSettings;

}
[Serializable]
public class ZombieSpwnSetting
{
    public ZombieSpawnData zombieType;

    public int zombieCount;
}
[Serializable]
public class ZombieSpawnData
{
    public ZombieType zombieType;
    public int level;

}

public enum ZombieType
{
    Normal,
    Boss,
    Special
}
