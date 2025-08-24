using Zenject;

public class SelectPlayerViewMediator : IMediator
{
    [Inject] private SelectPlayerProxy selectPlayerProxy;
    private SelectPlayerView view;

    public override void Register(IView view)
    {
        this.view = view as SelectPlayerView;
    }

    [Listener(SelectPlayerEvent.ON_SHOW_SELECT_PLAYER)]
    public void ShowSelectPlayer()
    {

    }

    [Listener(SelectPlayerEvent.ON_HIDE_SELECT_PLAYER)]
    public void HideSelectPlayer()
    {
        view.ResetView();
    }
}
