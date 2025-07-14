using Zenject;

public class SelectLevelViewMediator : IMediator
{
    [Inject] private BattleProxy battleProxy;
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
    public void SelectLevelClicked(BattleZombieSpawnData battleZombieSpawnData)
    {
        battleProxy.SetData(battleZombieSpawnData);
        listener.BroadCast(SelectLevelEvent.ON_SELECT_LEVEL_CLICKED);
    }
    [Listener(GameEvent.ON_GAME_STATE_END)]
    public void HideSelectLevel()
    {
        view.ResetView();
    }

}
