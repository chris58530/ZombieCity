using UnityEngine;
using Zenject;
public class ClickHitView : MonoBehaviour, IView
{
    [Inject] private ClickHitViewMediator mediator;
    private ClickHitController clickController;
    [SerializeField] private GameObject clickMask;

    public void Awake()
    {
        InjectService.Instance.Inject(this);
    }
    private void OnEnable()
    {
        mediator.Register(this);
        clickMask.SetActive(false);


        clickController = new GameObject("ClickHitController").AddComponent<ClickHitController>();
        clickController.transform.SetParent(transform);
        clickController.onClickZombie += OnClickZombie;
        clickController.onClickSurvivor += OnClickSurvivor;
        clickController.onClickSurvivorUp += OnClickUp;
    }
    private void OnDisable()
    {
        mediator.DeRegister(this);
    }
    public void OnEnableClickController()
    {

    }
    public void OnClickZombie(SafeZombieBase zombie)
    {
        mediator.OnClickZombie(zombie);
    }
    public void OnClickSurvivor(SurvivorBase survivor, Vector3 pickPos)
    {
        mediator.OnClickSurvivor(survivor, pickPos);
        clickMask.SetActive(true);
    }
    public void OnClickUp(FloorBase floor)
    {
        mediator.OnClickUp(floor);
        clickMask.SetActive(false);

    }
}

