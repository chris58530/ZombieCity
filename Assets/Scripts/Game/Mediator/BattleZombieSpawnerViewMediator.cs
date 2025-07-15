public class BattleZombieSpawnerViewMediator : IMediator
{
    private BattleZombieSpawnerView view;

    public override void Register(IView view)
    {
        this.view = view as BattleZombieSpawnerView;
    }

    [Listener(GameEvent.ON_BATTLE_STATE_START)]
    public void StartSpawningZombies()
    {
        view.StartSpawning();
    }

    [Listener(GameEvent.ON_BATTLE_STATE_END)]
    public void StopSpawningZombies()
    {
        view.ResetView();
    }
}