using UnityEngine;
using Zenject;

public class GameCamera : MonoBehaviour
{
    [Inject] private GameCameraProxy gameCameraProxy;
    [SerializeField] private CameraType cameraType;
    private void OnEnable()
    {
        gameCameraProxy.SetCamera(cameraType, this);
    }
}
public enum CameraType
{
    Game,
    DrawCardCamra,
    Battle
}