using System;
using UnityEngine;

public class BattleCampCarView : MonoBehaviour, IView
{
    [Zenject.Inject] private BattleCampCarViewMediator mediator;
    [SerializeField] private GameObject root;
    [Header("CampCar")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private BattleCampCarController battleCampCarController;
    [SerializeField] private GameObject followCamera;
    [Header("Gun")]
    [SerializeField] private GunView gunView;
    private GunState gunState = GunState.None;

    [SerializeField] private RotateController rotateController;
    [Header("Test")]
    [SerializeField] private GunDataSetting gunDataSetting;
    public bool isTest = false;
    public GameObject testRoot;
    public Action<GunState> OnGunStateChanged;
    private void Awake()
    {
        InjectService.Instance.Inject(this);
        ResetView();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            OnGunStateChanged?.Invoke(GunState.Pressing);
        }
        else
        {
            OnGunStateChanged?.Invoke(GunState.Releasing);
        }
    }

    private void OnEnable()
    {
        mediator.Register(this);
        mediator.RegisterHittableTarget(battleCampCarController);

        OnGunStateChanged += rotateController.SetRotate;

    }

    private void OnDisable()
    {
        mediator.DeRegister(this);
    }

    public void ShowBattleCampCar()
    {
        root.SetActive(true);
        battleCampCarController.MoveToMiddle(moveSpeed, () =>
        {
            RequestStartShoot();
        });
    }


    public void ResetView()
    {
        gunView.ResetView();
        battleCampCarController.ResetView();
        root.SetActive(false);
    }
    private void RequestStartShoot()
    {
        //通知GunView開始射擊
        mediator.RequestStartShoot(gunDataSetting);

        //啟動旋轉
        rotateController.enabledRotate = true;
    }
}

