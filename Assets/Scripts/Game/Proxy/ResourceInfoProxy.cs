public class ResourceInfoProxy : IProxy
{
    public int moneyAmount;
    public int satisfactionAmount;
    public void SetResource(int money = 0, int satisfaction = 0)
    {
        moneyAmount = money;
        satisfactionAmount = satisfaction;
        listener.BroadCast(ResourceInfoEvent.ON_UPDATE_RESOURCE);
    }
    public void AddMoney(int money)
    {
        moneyAmount += money;
        listener.BroadCast(ResourceInfoEvent.ON_ADD_MONEY);
    }
    public void AddSatisfaction(int satisfaction)
    {
        satisfactionAmount += satisfaction;
        listener.BroadCast(ResourceInfoEvent.ON_ADD_SATISFACTION);
    }
    public void GetResourceInfo(out int money, out int satisfaction) // save to Json
    {
        money = moneyAmount;
        satisfaction = satisfactionAmount;
    }
}
