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
    [Header("Test")]
    public bool isTest = false;
    public GameObject testRoot;
    private void Awake()
    {
        InjectService.Instance.Inject(this);
        ResetView();
    }

    private void OnEnable()
    {
        mediator.Register(this);
        mediator.RegisterHittableTarget(battleCampCarController);
    }

    private void OnDisable()
    {
        mediator.DeRegister(this);
    }

    public void ShowBattleCampCar()
    {
        root.SetActive(true);
        battleCampCarController.MoveToMiddle(moveSpeed);
    }


    public void ResetView()
    {
        Debug.Log("ResetView called in BattleCampCarView");

        gunView.ResetView();
        battleCampCarController.ResetView();
        root.SetActive(false);
    }
}

