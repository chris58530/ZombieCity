using System;
using System.Runtime.CompilerServices;
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

    private void OnEnable()
    {
        mediator.Register(this);
        mediator.RegisterHittableTarget(battleCampCarController);

        OnGunStateChanged += rotateController.SetRotate;
        OnGunStateChanged += gunView.SetGunState;
    }
    private void OnDisable()
    {
        mediator.DeRegister(this);
    }

    public void ResetView()
    {
        gunView.ResetView();
        battleCampCarController.ResetView();
        root.SetActive(false);
    }
    private void Update()
    {
        if (gunView.isLock)
            return;
        if (Input.GetMouseButton(0))
        {
            OnGunStateChanged?.Invoke(GunState.Pressing);
        }
        else
        {
            OnGunStateChanged?.Invoke(GunState.Releasing);
        }
    }



    public void ShowBattleCampCar()
    {
        root.SetActive(true);
        battleCampCarController.MoveToBottom(moveSpeed, () =>
        {
            Debug.Log("Camp Car Arrive");
            RequestStartShoot();
            mediator.NotifyCampCarArrive();
        });
    }

    private void RequestStartShoot()
    {
        //通知GunView開始射擊
        gunView.SetUpGun(gunDataSetting);

        //啟動旋轉
        rotateController.enabledRotate = true;
    }

    public void SetEnableShooting(bool isEnable)
    {
        OnGunStateChanged?.Invoke(GunState.Releasing);
        //to do  1023
        gunView.isLock = !isEnable;
        rotateController.enabledRotate = isEnable;
    }
}

