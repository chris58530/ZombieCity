using System.Collections.Generic;
using UnityEngine;

public class GameCameraProxy : IProxy
{
    public Dictionary<CameraType, GameCamera> gameCameras = new();
    public GameCamera mainCamera;
    public bool canSwipe;
    public void SetCamera(CameraType type, GameCamera camera)
    {
        mainCamera = camera;
        gameCameras[type] = camera;
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
