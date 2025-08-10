using UnityEngine;
using Zenject;

public class BattleMenuView : MonoBehaviour, IView
{
    [Inject] private BattleMenuViewMediator mediator;
    [SerializeField] private GameObject root;
    private void Awake()
    {
        InjectService.Instance.Inject(this);
        ResetView();
    }

    private void OnEnable()
    {
        mediator.Register(this);
    }

    // This method is called when the view is disabled
    private void OnDisable()
    {
        mediator.DeRegister(this);
    }
    private void ResetView()
    {
        root.SetActive(false);
    }
    public void Show()
    {
        root.SetActive(true);
    }
    public void OnLeaveClick()
    {
        mediator.OnLeaveClick();
        ResetView();
    }
}
