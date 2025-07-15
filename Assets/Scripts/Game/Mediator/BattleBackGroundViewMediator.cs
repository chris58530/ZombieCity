public class BattleBackGroundViewMediator : IMediator
{
    private BattleBackGroundView view;
    public override void Register(IView view)
    {
        this.view = view as BattleBackGroundView;
    }

    [Listener(GameEvent.ON_BATTLE_STATE_START)]
    public void ShowBattleBackground()
    {
        view.Show();
    }
    [Listener(GameEvent.ON_BATTLE_STATE_END)]
    private void HideBattleBackground()
    {
        view.ResetView();
    }

}
