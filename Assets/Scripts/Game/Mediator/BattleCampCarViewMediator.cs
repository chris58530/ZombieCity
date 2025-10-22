using Zenject;

public class BattleCampCarViewMediator : IMediator
{
    [Inject] private BattleProxy battleProxy;
    [Inject] private GunProxy gunProxy;
    private BattleCampCarView view;

    public override void Register(IView view)
    {
        base.Register(view);
        this.view = view as BattleCampCarView;
    }

    public override void DeRegister(IView view)
    {
        base.DeRegister(view);
    }

    // [Listener(CampCarEvent.ON_BATTLE_CAR_SHOW)]
    [Listener(GameEvent.ON_BATTLE_STATE_START)]
    public void ShowBattleCampCar()
    {
        view.ShowBattleCampCar();
    }
    [Listener(GameEvent.ON_BATTLE_STATE_END)]
    public void HideBattleCampCar()
    {
        view.ResetView();
    }

    public void NotifyCampCarArrive()
    {
        // 廣播前導動畫結束 開始遊戲了
        listener.BroadCast(BattleEvent.ON_BATTLE_START);
    }
    public void RegisterHittableTarget(IHittable hittable)
    {
        battleProxy.campCar = hittable;
    }
    [Listener(BattleSkillEvent.ON_SELECT_START)]
    public void OnStopShoot()
    {
        view.SetEnableShooting(false);

    }
    [Listener(BattleSkillEvent.ON_SELECT_END)]
    public void OnReStartShoot()
    {
        view.SetEnableShooting(true);
    }

}
