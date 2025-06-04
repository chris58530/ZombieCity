using UnityEngine;
using Zenject;

public class GameCameraCmd : ICommand
{
    [Inject] private GameCameraProxy proxy;
    [Inject] private FloorProxy floorProxy;
    public override void Execute(MonoBehaviour mono)
    {
        isLazy = true;
        proxy.EnabelSwipe();
    }
    // [Listener(CameraEvent.ON_USE_FEATURE_CAMERA)]
    // public void OnUseFeatureCamera()
    // {
    //     proxy.DisableSwipe();

    // }
    // [Listener(CameraEvent.ON_USE_FEATURE_CAMERA_COMPLETE)]
    // public void OnUseFeatureCameraComplete()
    // {
    //     proxy.EnabelSwipe();
    // }


}
