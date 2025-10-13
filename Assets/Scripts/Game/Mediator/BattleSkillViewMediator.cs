using Zenject;

public class BattleSkillViewMediator : IMediator
{
    [Inject] private BattleSkillProxy proxy;
    private BattleSkillView view;

    public override void Register(IView view)
    {
        this.view = view as BattleSkillView;
    }

    [Listener(GameEvent.ON_BATTLE_STATE_START)]
    public void ShowBattleSkills()
    {
    }

    [Listener(GameEvent.ON_BATTLE_STATE_END)]
    public void HideBattleSkills()
    {
        // view.ResetView();
    }

    public void OnSelectSkill(SkillType skillType)
    {
        proxy.RequestSkillUpgrade(skillType);
    }
}
