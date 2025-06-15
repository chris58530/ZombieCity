using UnityEngine;

public class SelectLevelView : MonoBehaviour, IView
{
    [Zenject.Inject] private SelectLevelViewMediator mediator;
    [SerializeField] private GameObject root;
    private void Awake()
    {
        InjectService.Instance.Inject(this);
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

    public void ShowSelectLevel()
    {
        root.SetActive(true);
    }
    public void HideSelectLevel()
    {
        root.SetActive(false);
    }
}
public class SelectLevelViewMediator : IMediator
{
    private SelectLevelView view;
    public override void Register(IView view)
    {
        base.Register(view);
        this.view = view as SelectLevelView;
    }
    public override void DeRegister(IView view)
    {
        base.DeRegister(view);
    }
    [Listener(CampCarEvent.ON_SELECT_LEVEL_SHOW)]
    public void ShowSelectLevel()
    {
        view.ShowSelectLevel();
    }
}
