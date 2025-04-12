public class PlayerDataProxy : IProxy
{
    // TODO: Add player-related data and logic here
    private PlayerData playerData;
    public void SetData(PlayerData data)
    {
        playerData = data;
    }
    public PlayerData GetData()
    {
        return playerData;
    }
}
