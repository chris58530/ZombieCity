public class BattleProxy : IProxy
{
    public BattleZombieSpawnData battleZombieSpawnData;
    public void SetData(BattleZombieSpawnData data)
    {
        battleZombieSpawnData = (BattleZombieSpawnData)data;
    }

}
