using Zenject;

public class BattleCampCarViewMediator : IMediator
{
    [Inject] private BattleProxy battleProxy;
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
    public void RegisterHittableTarget(IHittable hittable)
    {
        battleProxy.campCar = hittable;
    }
}
