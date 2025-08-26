using DG.Tweening;
using UnityEngine;
using UnityEngine.Video;

public class SelectLevelView : MonoBehaviour, IView
{
    [Zenject.Inject] private SelectLevelViewMediator mediator;
    [SerializeField] private GameObject root;
    [Header("點擊後縮放設定")]
    [SerializeField] private float scaleSize;
    [SerializeField] private float scaleDuration;
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
        mediator.OnLeaveClick();
        ResetView();
    }
    public void SelectLevelClicked(BattleZombieSpawnData battleZombieSpawnData)
    {
        root.transform.DOScale(scaleSize, scaleDuration).SetEase(Ease.InBack).OnComplete(() =>
        {
            mediator.SelectLevelClicked(battleZombieSpawnData);
        });
    }
    public void CancelSelectLevel()
    {
        root.transform.DOScale(1f, scaleDuration).SetEase(Ease.OutBack).OnComplete(() =>
        {
            mediator.OnLeaveClick();
        });
    }
    public void ResetView()
    {
        root.transform.localScale = Vector3.one;
        root.SetActive(false);
    }
    public void ClickCampCar() //打開選角畫面
    {
        mediator.OnClickCampCar();
    }
}
