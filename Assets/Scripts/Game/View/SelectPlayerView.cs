using UnityEngine;
using Zenject;

public class SelectPlayerView : MonoBehaviour, IView
{
    [Inject] private SelectPlayerViewMediator mediator;

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

    public void ResetView()
    {

    }
}
