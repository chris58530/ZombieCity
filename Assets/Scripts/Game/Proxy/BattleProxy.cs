public class BattleProxy : IProxy
{
    public BattleZombieSpawnData battleZombieSpawnData;
    public IHittable campCar;

    //BattleZombie
    public int battleZombieCount;

    public void SetData(BattleZombieSpawnData data)
    {
        battleZombieSpawnData = (BattleZombieSpawnData)data;
    }

    public void RequestZombieCountUpdate(int count)
    {
        battleZombieCount = count;
        listener.BroadCast(BattleEvent.REQUEST_UPDATE_ZOMBIE_COUNT);
    }

}
