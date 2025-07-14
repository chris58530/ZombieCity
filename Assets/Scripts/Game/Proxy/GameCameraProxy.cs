using System.Collections.Generic;
using UnityEngine;

public class GameCameraProxy : IProxy
{
    public Dictionary<CameraType, GameCamera> gameCameras = new();
    public GameCamera mainCamera;
    public bool canSwipe;
    public void SetCamera(CameraType type, GameCamera camera)
    {
        if (type == CameraType.Game)
        {
            mainCamera = camera;
        }
        Debug.Log($"Setting camera of type {type}");
        gameCameras[type] = camera;
    }
    public void UseCamera(CameraType type)
    {
        if (gameCameras.TryGetValue(type, out var camera))
        {
            mainCamera = camera;
            camera.gameObject.SetActive(true);
            foreach (var cam in gameCameras.Values)
            {
                if (cam != camera)
                {
                    cam.gameObject.SetActive(false);
                }
            }
        }
        else
        {
            Debug.LogWarning($"Camera of type {type} not found.");
        }
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
