using UnityEngine;
using Zenject;

public class GameCamera : MonoBehaviour
{
    [Inject] private GameCameraProxy gameCameraProxy;
    [SerializeField]private CameraType cameraType;
    private void OnEnable()
    {
        InjectService.Instance.Inject(this);
        gameCameraProxy.SetCamera(cameraType,this);
    }
}
public enum CameraType
{
    MainCamera,
    DrawCardCamra
}