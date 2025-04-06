using UnityEngine;

public class GameCameraProxy : IProxy
{
    public GameCamera gameCamera;
    public bool canSwipe;
    public float minY;
    public void SetCamera(GameCamera camera)
    {
        gameCamera = camera;
    }
    public void EnabelSwipe()
    {
        canSwipe = true;
        listener.BroadCast(CameraEvent.INIT_CAMERA_SWIPE);
    }
    public void DisableSwipe()
    {
        canSwipe = false;
        listener.BroadCast(CameraEvent.CLOSE_CAMERA_SWIPE);
    }
}
