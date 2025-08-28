using DG.Tweening;
using UnityEngine;
using Zenject;
public class LevelComfirmView : MonoBehaviour, IView
{
    [Inject] private LevelComfirmViewMediator mediator;
    [SerializeField] private GameObject root;
    [SerializeField] private GameObject movingGroup;
    [Header("開啟時移動")]
    [SerializeField] private Transform startPos;
    [SerializeField] private Transform endPos;
    [SerializeField] private float fadeInTime;

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
        movingGroup.SetActive(true);
        movingGroup.transform.position = startPos.position;
        movingGroup.transform.DOMove(endPos.position, fadeInTime).SetEase(Ease.OutBack);
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
        mediator.CancelLevel();
        HideLevelConfirm();
    }
    public void ResetView()
    {
        root.SetActive(false);
    }
}
