using UnityEngine;
using Zenject;

public class BattleSelectView : MonoBehaviour, IView
{
    [Inject] private BattleSelectViewMedaitor mediator;

    

    private void Awake()
    {
        ResetView();
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
