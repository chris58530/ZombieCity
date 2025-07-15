public class BattleSkillViewMediator : IMediator
{
    private BattleSkillView view;

    public override void Register(IView view)
    {
        this.view = view as BattleSkillView;
    }

    [Listener(GameEvent.ON_BATTLE_STATE_START)]
    public void ShowBattleSkills()
    {
        view.ShowBattleSkills();
    }

    [Listener(GameEvent.ON_BATTLE_STATE_END)]
    public void HideBattleSkills()
    {
        view.ResetView();
    }
}
