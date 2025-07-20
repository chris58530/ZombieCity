using Zenject;

public class BattleZombieSpawnerViewMediator : IMediator
{
    [Inject]private BattleProxy battleProxy;
    private BattleZombieSpawnerView view;

    public override void Register(IView view)
    {
        this.view = view as BattleZombieSpawnerView;
    }

    [Listener(GameEvent.ON_BATTLE_STATE_START)]
    public void StartSpawningZombies()
    {
        BattleZombieSpawnData battleZombieSpawnData = battleProxy.battleZombieSpawnData;
        view.StartSpawning(battleZombieSpawnData);
    }

    [Listener(GameEvent.ON_BATTLE_STATE_END)]
    public void StopSpawningZombies()
    {
        view.ResetView();
    }
}