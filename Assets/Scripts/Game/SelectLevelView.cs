using UnityEngine;

public class SelectLevelView : MonoBehaviour, IView
{
    [Zenject.Inject] private SelectLevelViewMediator mediator;
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
    public void SelectLevelClicked(int levelId)
    {
        mediator.SelectLevelClicked(levelId);
    }
    public void ResetView()
    {
        root.SetActive(false);
    }
}
