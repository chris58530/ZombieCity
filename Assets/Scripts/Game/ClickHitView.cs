using UnityEngine;
using Zenject;
public class ClickHitView : MonoBehaviour, IView
{
    [Inject] private ClickHitViewMediator mediator;
    private ClickHitController clickController;

    public void Awake()
    {
        InjectService.Instance.Inject(this);
    }
    private void OnEnable()
    {
        mediator.Register(this);
        clickController = new GameObject("ClickHitController").AddComponent<ClickHitController>();
        clickController.transform.SetParent(transform);
        clickController.onClickZombie += OnClickZombie;
    }
    private void OnDisable()
    {
        mediator.DeRegister(this);
    }
    public void OnEnableClickController()
    {

    }
    public void OnClickZombie(ZombieBase zombie)
    {
        Debug.Log("ClickHitView: OnClickZombie" + zombie.name);
        mediator.OnClickZombie(zombie);
    }
}

