using UnityEngine;
using DG.Tweening;

public class BattleCampCarController : MonoBehaviour
{
    [Header("移動設定")]
    [Tooltip("車子自動前進的速度 (單位/秒)")]
    public float forwardSpeed = 5f;

    [Tooltip("車子左右移動的速度 (單位/秒)")]
    public float sideSpeed = 3f;

    [Tooltip("車子在 X 軸 (左右) 移動的範圍限制 (最小X值, 最大X值)")]
    public Vector2 xBounds;
    private float xPosition;

    private Tween sideMoveTween;
    private Tween forwardMoveTween;

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector2 touchPosition = Input.mousePosition;

            if (touchPosition.x < Screen.width / 2)
            {
                xPosition += Time.deltaTime * sideSpeed;

                if (xPosition > xBounds.x)
                {
                    xPosition = xBounds.x;
                }
            }
            else
            {
                xPosition -= Time.deltaTime * sideSpeed;
                if (xPosition < xBounds.y)
                {
                    xPosition = xBounds.y;
                }
            }
            sideMoveTween?.Kill();
            sideMoveTween = transform.DOMoveX(xPosition, sideSpeed)
                                   .SetEase(Ease.OutQuad);
        }
    }

    public void StartForwardMovement()
    {
        forwardMoveTween?.Kill();
        forwardMoveTween = transform.DOMoveY(float.MaxValue, forwardSpeed)
                                   .SetSpeedBased(true)
                                   .SetEase(Ease.Linear)
                                   .OnComplete(() => OnArriveAtDestination())
                                   .SetLoops(-1, LoopType.Restart);
    }
    private void OnArriveAtDestination()
    {

    }
    public void SetFollowCamera(GameObject camera)
    {
        if (camera != null)
        {
            camera.transform.SetParent(transform);
            camera.transform.localPosition = new Vector3(0, 2, -5);
            camera.transform.localRotation = Quaternion.Euler(20, 0, 0);
        }
    }
    public void ResetView()
    {
        xPosition = 0;
        sideMoveTween?.Kill();
        forwardMoveTween?.Kill();
    }
}