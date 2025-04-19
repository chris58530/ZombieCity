public class PlayerDataProxy : IProxy
{
    public PlayerData playerData;
    public void SetData(PlayerData data)
    {
        playerData = data;
    }
    public PlayerData GetData()
    {
        return playerData;
    }
    public void SetFloorData(FloorProductData floorProductData)
    {
        playerData.floorProductData = floorProductData;
    }
}
