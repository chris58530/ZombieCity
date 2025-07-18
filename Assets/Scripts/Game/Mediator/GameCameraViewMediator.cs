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
    [Listener(CameraEvent.MOVE_TO_GAME_VIEW)]
    public void MoveToGameView()
    {
        view.MoveToGameView();
    }
    [Listener(CameraEvent.INIT_CAMERA_SWIPE)]
    public void OpenSwipe()
    {
        GameCamera camera = proxy.mainCamera;
        if (camera == null)
        {
            return;
        }

        view.InitSwipe(camera);
    }
    [Listener(CameraEvent.OPEN_CAMERA_SWIPE)]
    public void StartSwipe()
    {
        view.StartSwipe();
    }
    [Listener(CameraEvent.CLOSE_CAMERA_SWIPE)]
    public void StopSwipe()
    {
        view.StopSwipe();
    }

}
