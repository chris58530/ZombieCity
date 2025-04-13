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
    public void SetResourceInfoData(ResourceInfoData data)
    {
        playerData.resourceInfoData = data;
    }
}
