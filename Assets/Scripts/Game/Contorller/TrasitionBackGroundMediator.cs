using Zenject;
// Mediator
public class TrasitionBackGroundMediator : IMediator
{
    [Inject] private TrasitionBackGroundProxy proxy;
    private TrasitionBackGroundView view;

    public override void Register(IView view)
    {
        base.Register(view);
        this.view = view as TrasitionBackGroundView;
    }

    public override void DeRegister(IView view)
    {
        base.DeRegister(view);
    }
    [Listener(TrasitionBackGroundEvent.ON_TRASITION_BACKGROUND)]
    public void RequestTrasitionBackGround()
    {
        view.RequestTrasitionBackGround();
    }
    public void OnTrasitionComplete()
    {
        listener.BroadCast(TrasitionBackGroundEvent.ON_TRASITION_COMPLETE);
    }
}
