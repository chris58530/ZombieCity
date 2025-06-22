using System;
using UnityEngine;

public class BattleCampCarView : MonoBehaviour, IView
{
    // [Zenject.Inject] private BattleCampCarViewMediator mediator;
    [SerializeField] private GameObject root;
    [SerializeField] private BattleCampCarController battleCampCarController;
    [SerializeField] private GameObject followCamera;
    private BattleSetting battleSetting;
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
        battleCampCarController.StartForwardMovement();
    }

    private void OnDisable()
    {
        // mediator.DeRegister(this);
        battleCampCarController.ResetView();
    }
    public void SetBattleSetting(BattleSetting setting)
    {
        battleSetting = setting;
    }
    public void ShowBattleCampCar()
    {
        root.SetActive(true);
        battleCampCarController.SetFollowCamera(followCamera);
    }

    public void HideBattleCampCar()
    {
        root.SetActive(false);
    }
}
[Serializable]
public class TriggerZombieSetting
{
    public float triggerMeter;
    public float spawnYRange;
    public BattleSpaceSetting[] zombieSettings;

}
[Serializable]
public class BattleSpaceSetting
{
    public SpaceType spaceType;
    public ZombieType zombieType;
    public int zombieCount;
}
public enum SpaceType
{
    Right,
    Middle,
    Left
}
public enum ZombieType
{
    Normal,
    Boss,
    Special
}
