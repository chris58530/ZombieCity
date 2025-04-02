using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 0.1f; // 控制相機移動速度
    private Vector2 lastTouchPosition;
    private bool isDragging = false;

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                lastTouchPosition = touch.position;
                isDragging = true;
            }
            else if (touch.phase == TouchPhase.Moved && isDragging)
            {
                float deltaY = (touch.position.y - lastTouchPosition.y) * moveSpeed * Time.deltaTime;
                transform.position += new Vector3(0, -deltaY, 0); // 負號表示手指往上滑時相機往下
                lastTouchPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                isDragging = false;
            }
        }
    }
}