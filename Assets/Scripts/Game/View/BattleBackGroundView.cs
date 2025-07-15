using UnityEngine;

public class BattleBackGroundView : MonoBehaviour, IView
{
    [SerializeField] private GameObject root;
    [Zenject.Inject] private BattleBackGroundViewMediator mediator;

    private void Awake()
    {
        InjectService.Instance.Inject(this);
        ResetView();
    }
    public void ResetView()
    {
        root.SetActive(false);
    }
    private void OnEnable()
    {
        mediator.Register(this);
    }
    private void OnDisable()
    {
        mediator.DeRegister(this);
    }
    public void Show()
    {
        root.SetActive(true);
    }
}
