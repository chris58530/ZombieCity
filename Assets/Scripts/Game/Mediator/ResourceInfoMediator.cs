using Zenject;

public class ResourceInfoMediator : IMediator
{
    [Inject] private ResourceInfoProxy proxy;
    private ResourceInfoView view;

    public override void Register(IView view)
    {
        base.Register(view);
        this.view = view as ResourceInfoView;
    }

    public override void DeRegister(IView view)
    {
        base.DeRegister(view);
    }

    [Listener(ResourceInfoEvent.ON_UPDATE_RESOURCE)]
    public void OnUpdateResource()
    {
        view.OnUpdateResource(proxy.moneyAmount, proxy.satisfactionAmount);
    }
    [Listener(ResourceInfoEvent.ON_ADD_MONEY)]
    public void OnAddMoney()
    {
        view.OnAddMoney(proxy.moneyAmount);
        listener.BroadCast(PlayerDataEvent.ON_UPDATE_PLAYER_DATA);

    }

    [Listener(ResourceInfoEvent.ON_ADD_SATISFACTION)]
    public void OnAddSatisfaction()
    {
        view.OnAddSatisfaction(proxy.satisfactionAmount);
        listener.BroadCast(PlayerDataEvent.ON_UPDATE_PLAYER_DATA);

    }
}
