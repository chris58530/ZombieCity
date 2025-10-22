using Zenject;

public class BattleSkillViewMediator : IMediator
{
    [Inject] private BattleSkillProxy proxy;
    private BattleSkillView view;

    public override void Register(IView view)
    {
        this.view = view as BattleSkillView;
    }

    [Listener(BattleEvent.ON_BATTLE_START)]
    public void OnBattleStart()
    {
        view.StartSkillCoolDown();
    }

    public void RequestFreezeTimeScale()
    {
        //選技能 要讓遊戲暫停
        //殭屍暫停移動
        //自動射擊的炮台要取消射擊
        listener.BroadCast(BattleSkillEvent.ON_SELECT_START);
    }

    public void OnSelectSkill(SkillType skillType)
    {
        proxy.RequestSkillUpgrade(skillType);
        listener.BroadCast(BattleSkillEvent.ON_SELECT_END);
    }
}
