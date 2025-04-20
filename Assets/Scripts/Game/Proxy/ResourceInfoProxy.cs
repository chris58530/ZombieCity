public class ResourceInfoProxy : IProxy
{
    public ResourceInfoData resourceInfoData;
    public int moneyAmount;
    public int satisfactionAmount;
    public int gemAmount;
    public void SetResource(ResourceInfoData resourceInfoData)
    {
        this.resourceInfoData = resourceInfoData;
        moneyAmount = resourceInfoData.moneyAmount;
        satisfactionAmount = resourceInfoData.satisfactionAmount;
        gemAmount = resourceInfoData.gemAmount;
        listener.BroadCast(JsonDataEvent.ON_UPDATE_PLAYER_DATA);

        listener.BroadCast(ResourceInfoEvent.ON_UPDATE_RESOURCE);
    }
    public void AddMoney(int money)
    {
        moneyAmount += money;
        resourceInfoData.moneyAmount = moneyAmount;
        listener.BroadCast(JsonDataEvent.ON_UPDATE_PLAYER_DATA);

        listener.BroadCast(ResourceInfoEvent.ON_ADD_MONEY);
    }
    public void AddSatisfaction(int satisfaction)
    {
        satisfactionAmount += satisfaction;
        resourceInfoData.satisfactionAmount = satisfactionAmount;
        listener.BroadCast(JsonDataEvent.ON_UPDATE_PLAYER_DATA);
        listener.BroadCast(ResourceInfoEvent.ON_ADD_SATISFACTION);
    }
    public void AddGem(int gem)
    {
        gemAmount += gem;
        resourceInfoData.gemAmount = gemAmount;
        listener.BroadCast(JsonDataEvent.ON_UPDATE_PLAYER_DATA);
    }
    // public void GetResourceInfo(out int money, out int satisfaction) // save to Json
    // {
    //     money = moneyAmount;
    //     satisfaction = satisfactionAmount;
    // }
}
