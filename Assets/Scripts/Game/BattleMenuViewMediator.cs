using UnityEngine;
using Zenject;

public class BattleMenuViewMediator : IMediator
{
    [Inject] private GameStateProxy gameStateProxy;
    private BattleMenuView view;

    public override void Register(IView view)
    {
        base.Register(view);
        this.view = view as BattleMenuView;
    }

    public override void DeRegister(IView view)
    {
        base.DeRegister(view);
    }
    [Listener(GameEvent.ON_BATTLE_STATE_START)]
    public void OnShowMenu()
    {
        view.Show();
    }
    public void OnLeaveClick()
    {
        gameStateProxy.RequestChangeState(GameState.Game);
    }

}
