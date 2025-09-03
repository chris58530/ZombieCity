using Zenject;

public class BattleZombieCounterViewMediator : IMediator
{
    [Inject] private BattleProxy battleProxy;
    private BattleZombieCounterView view;


    public override void Register(IView view)
    {
        this.view = view as BattleZombieCounterView;

    }
    [Listener(BattleEvent.REQUEST_UPDATE_ZOMBIE_COUNT)]
    private void OnZombieCountUpdated()
    {
        int remaining = battleProxy.battleZombieCount;
        view.UpdateDisplay(remaining);
    }
}
