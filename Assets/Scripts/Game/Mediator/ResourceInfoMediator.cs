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
        view.OnUpdateResource(proxy.moneyAmount, proxy.satisfactionAmount, proxy.zombieCoreAmount);
    }
    [Listener(ResourceInfoEvent.ON_ADD_MONEY)]
    public void OnAddMoney()
    {
        view.OnAddMoney(proxy.moneyAmount);

    }

    [Listener(ResourceInfoEvent.ON_ADD_SATISFACTION)]
    public void OnAddSatisfaction()
    {
        view.OnAddSatisfaction(proxy.satisfactionAmount);

    }
    [Listener(ResourceInfoEvent.ON_ADD_ZOMBIECORE)]
    public void OnAddZombieCore()
    {
        view.OnAddZombieCore(proxy.zombieCoreAmount);
    }
    [Listener(ResourceInfoEvent.HIDE)]
    public void Hide()
    {
        view.Hide();
    }
    [Listener(GameEvent.ON_GAME_STATE_START)]
    [Listener(ResourceInfoEvent.SHOW)]

    public void OnGameStateStart()
    {
        view.Show();
        OnUpdateResource();
    }
}
