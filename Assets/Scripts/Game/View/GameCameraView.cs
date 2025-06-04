using UnityEngine;
using DG.Tweening;
using Zenject;

public class GameCameraView : MonoBehaviour, IView
{
    [Inject] private GameCameraViewMediator mediator;
    [SerializeField] private CameraSwipeController cameraSwipeController;
    [SerializeField] private GameCamera gameCamera;
    public bool isFrist = true;
    private Tween firstMoveTween;

    public void Awake()
    {
        InjectService.Instance.Inject(this);

    }
    private void OnEnable()
    {
        mediator.Register(this);
    }
    private void OnDisable()
    {
        mediator.DeRegister(this);
    }
    public void MoveToGameView()
    {
        if (!isFrist) return;
        isFrist = false;
        Vector3 targetPosition = new Vector3(0, -5, -10);
        firstMoveTween = gameCamera.transform.DOMove(targetPosition, 1f).SetEase(Ease.InOutSine).OnComplete(() =>
        {
            cameraSwipeController.openSwipe = true;
            firstMoveTween?.Kill();

        });
    }
    public void InitSwipe(GameCamera gameCamera)
    {
        this.gameCamera = gameCamera;

        if (this.gameCamera == null)
        {
            LogService.Instance.Log("GameCamera is null");
            return;
        }

        cameraSwipeController.Init(gameCamera);

    }
    public void StartSwipe()
    {
        cameraSwipeController.openSwipe = true;
    }
    public void StopSwipe()
    {
        cameraSwipeController.openSwipe = false;
    }
}
