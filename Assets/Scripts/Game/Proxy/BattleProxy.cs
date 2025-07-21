public class BattleProxy : IProxy
{
    public BattleZombieSpawnData battleZombieSpawnData;
    public IHittable campCar;
    public void SetData(BattleZombieSpawnData data)
    {
        battleZombieSpawnData = (BattleZombieSpawnData)data;
    }

}
