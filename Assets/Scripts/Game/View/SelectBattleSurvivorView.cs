using DG.Tweening;
using UnityEngine;
using Zenject;

public class SelectBattleSurvivorView : MonoBehaviour, IView
{
    [Inject] private SelectBattleSurvivorViewMediator mediator;
    [SerializeField] private GameObject root;

    [Header("角色Icon")]
    [SerializeField] private SelectBattleSurvivorIconView selectBattleSurvivorIconView;



    [Header("角色按鈕")]
    [SerializeField] private SelectBattleSurvivorButton[] selectPlayerButtons;

    public bool isShowing;
    private void Awake()
    {
        InjectService.Instance.Inject(this);
    }

    private void OnEnable()
    {
        mediator.Register(this);
        ResetView();
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

    public void Show()
    {
        isShowing = true;
        root.SetActive(true);
    }

    //UI Event
    public void Exit()
    {
        mediator.HideSelectPlayer();
        ResetView();
    }

    //UI Event
    public void SelectPlayer(SelectBattleSurvivorButton playerButton)
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

        //更新Icon
        selectBattleSurvivorIconView.UpdateIcon(mediator.GetSelectedPlayers());
    }

    //UI Event
    public void RemoveAllSelectPlayers()
    {
        mediator.RemoveAllSelectPlayers();
    }
}
