using DG.Tweening;
using UnityEngine;
using Zenject;

public class SelectPlayerView : MonoBehaviour, IView
{
    [Inject] private SelectPlayerViewMediator mediator;
    [SerializeField] private GameObject root;

    [Header("開啟時移動")]
    [SerializeField] private Transform startPos;
    [SerializeField] private Transform endPos;
    [SerializeField] private float fadeInTime;

    [Header("角色按鈕")]
    [SerializeField] private SelectPlayerButton[] selectPlayerButtons;

    public bool isShowing;
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
        isShowing = false;
        root.SetActive(false);
        DOTween.Kill(GetHashCode());
    }

    public void ShowAndFadeIn()
    {
        isShowing = true;
        root.SetActive(true);
        root.transform.position = startPos.position;
        root.transform.DOMove(endPos.position, fadeInTime).SetEase(Ease.OutCubic).SetId(GetHashCode());
    }

    //UI Event
    public void Exit()
    {
        ResetView();
    }

    //UI Event
    public void SelectPlayer(SelectPlayerButton playerButton)
    {
        if (mediator.HasSelectedPlayers(playerButton.GetPlayerId()))
        {
            // Player is already selected
            playerButton.SetSelected(false);
            mediator.RemoveSelectPlayer(playerButton.GetPlayerId());
            return;
        }
        playerButton.SetSelected(true);
        mediator.SelectPlayer(playerButton.GetPlayerId());
    }

    //UI Event
    public void RemoveAllSelectPlayers()
    {
        mediator.RemoveAllSelectPlayers();
    }
}
