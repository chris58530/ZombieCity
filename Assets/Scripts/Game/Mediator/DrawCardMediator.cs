public class DrawCardMediator : IMediator
{
    private DrawCardView view;

    public override void Register(IView view)
    {
        base.Register(view);
        this.view = view as DrawCardView;
    }
    [Listener(DrawCardEvent.ON_DRAW_CARD_SHOW)]
    public void OnDrawCardShow()
    {
        listener.BroadCast(CameraEvent.ON_USE_FEATURE_CAMERA);
        view.Open();
    }
    public void OnDrawCardClose()
    {
        listener.BroadCast(TrasitionBackGroundEvent.ON_TRASITION_BACKGROUND);
        listener.BroadCast(CameraEvent.ON_USE_FEATURE_CAMERA_COMPLETE);
    }
}
