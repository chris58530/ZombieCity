using UnityEngine;
using Zenject;
public class LevelComfirmView : MonoBehaviour,IView
{
    [Inject] private LevelComfirmViewMediator mediator;

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

    public void ShowLevelConfirm()
    {
        root.SetActive(true);
    }

    public void HideLevelConfirm()
    {
        root.SetActive(false);
    }
    public void ConfirmLevel()
    {
        mediator.ConfirmLevel();
    }
    public void CancelLevel()
    {
        HideLevelConfirm();
    }
    public void ResetView()
    {
        root.SetActive(false);
    }
}
