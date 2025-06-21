using UnityEngine;

public class SelectLevelView : MonoBehaviour, IView
{
    [Zenject.Inject] private SelectLevelViewMediator mediator;
    [SerializeField] private GameObject root;
    private void Awake()
    {
       //by許杯 InjectService.Instance.Inject(this);
        //by許杯 root.SetActive(false);
    }

    private void OnEnable()
    {
        //by許杯 mediator.Register(this);
    }

    private void OnDisable()
    {
         //by許杯 mediator.DeRegister(this);
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
