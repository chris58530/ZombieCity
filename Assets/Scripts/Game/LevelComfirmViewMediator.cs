using UnityEngine;
using Zenject;

public class LevelComfirmViewMediator : IMediator
{
    [Inject]private GameStateProxy gameStateProxy;
    private LevelComfirmView view;

    public override void Register(IView view)
    {
        base.Register(view);
        this.view = view as LevelComfirmView;
    }

    public override void DeRegister(IView view)
    {
        base.DeRegister(view);
    }
    [Listener(SelectLevelEvent.ON_SELECT_LEVEL_CLICKED)]
    public void ShowLevelConfirm()
    {
        view.ShowLevelConfirm();
    }
    public void ConfirmLevel()
    {
        gameStateProxy.RequestChangeState(GameState.Battle);
    }
}
