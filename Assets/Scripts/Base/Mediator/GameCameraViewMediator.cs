using UnityEngine;
using Zenject;

public class GameCameraViewMediator : IMediator
{
    [Inject] private GameCameraProxy proxy;
    private GameCameraView view;
    public override void Register(IView view)
    {
        base.Register(view);
        this.view = view as GameCameraView;
    }
    [Listener(CameraEvent.OPEN_CAMERA_SWIPE)]
    public void OpenSwipe()
    {
        GameCamera camera = proxy.gameCamera;
        float minY = proxy.minY;
        if(camera == null)
        {
            LogService.Instance.Log("camera is null");
            return;
        }
        LogService.Instance.Log($"GameCameraProxy: {camera}");

        view.OpenSwipe(camera,minY);
    }
    [Listener(CameraEvent.CLOSE_CAMERA_SWIPE)]
    public void CloseSwipe()
    {
        view.CloseSwipe();
    }
}
