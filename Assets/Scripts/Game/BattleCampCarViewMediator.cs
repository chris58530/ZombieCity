public class BattleCampCarViewMediator : IMediator
{
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

    [Listener(CampCarEvent.ON_BATTLE_CAR_SHOW)]
    public void ShowBattleCampCar()
    {
        view.ShowBattleCampCar();
    }
}
