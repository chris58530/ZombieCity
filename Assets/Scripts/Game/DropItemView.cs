using UnityEngine;
using Zenject;

public class DropItemView : MonoBehaviour, IView
{
    [Inject] private DropItemViewMediator mediator;
    private PoolManager poolManager;

    private void Awake()
    {
        InjectService.Instance.Inject(this);
    }
    private void OnEnable()
    {
        mediator.Register(this);
    }
    private void OnDisable()
    {
        mediator.DeRegister(this);
    }
    public void Initialize()
    {

    }
}
