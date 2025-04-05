using UnityEngine;
using Zenject;

public class GameCameraView : MonoBehaviour, IView
{
    [Inject] private GameCameraViewMediator mediator;
    [SerializeField] private CameraSwipeController cameraSwipeController;
    [SerializeField]private GameCamera gameCamera;
    public void Awake()
    {
        InjectService.Instance.Inject(this);
        cameraSwipeController.enabled = false;

    }
    private void OnEnable()
    {
        mediator.Register(this);
    }
    private void OnDisable()
    {
        mediator.DeRegister(this);
    }
    public void OpenSwipe(GameCamera gameCamera,float minY)
    {
        this.gameCamera = gameCamera;
        LogService.Instance.Log($"gameCamera: {gameCamera}");

        if(this.gameCamera == null)
        {
            LogService.Instance.Log("GameCamera is null");
            return;
        }
        cameraSwipeController.enabled = true;

        cameraSwipeController.Init(gameCamera,minY);

    }
    public void CloseSwipe()
    {
    }
}
