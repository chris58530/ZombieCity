using Zenject;

public class BattleZombieSpawnerViewMediator : IMediator
{
    [Inject] private BattleProxy battleProxy;
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
    public IHittable GetCampCar()
    {
        return battleProxy.campCar;
    }

    public void RequestUpdateZombieCount(int count)
    {
        battleProxy.RequestZombieCountUpdate(count);
    }

    [Listener(BattleSkillEvent.ON_SELECT_START)]
    public void NotifyAllZombiesFreeze()
    {
        view.StopSpawning();
        view.OnFreezeAllZombie(true);
    }
    [Listener(BattleSkillEvent.ON_SELECT_END)]
    public void NotifyAllZombiesUnfreeze()
    {
        view.ReStartSpawning();
        view.OnFreezeAllZombie(false);
    }
}