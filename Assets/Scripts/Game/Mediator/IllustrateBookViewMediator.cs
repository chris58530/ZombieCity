using UnityEngine;

public class IllustrateBookViewMediator : IMediator
{
    private IllustrateBookView view;
    public override void Register(IView view)
    {
        base.Register(view);
        this.view = view as IllustrateBookView;
    }
    public void RequestStopSwipe()
    {
        listener.BroadCast(CameraEvent.CLOSE_CAMERA_SWIPE);
    }
    public void RequestOpenSwipe()
    {
        listener.BroadCast(CameraEvent.OPEN_CAMERA_SWIPE);
    }
}
