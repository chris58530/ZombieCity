using UnityEngine;
using Zenject;

public class GameCamera : MonoBehaviour
{
   [Inject]private GameCameraProxy gameCameraProxy;
    private void OnEnable()
    {
        InjectService.Instance.Inject(this);
       gameCameraProxy.SetCamera(this);
    }
}
