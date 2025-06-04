using UnityEngine;

public class CameraSwipeController : MonoBehaviour
{
    public GameCamera gameCamera;
    public float moveSpeed = 2f;
    public float scrollSpeed = 50f;
    public float minY = -30f;
    public float maxY = -5f;
    public bool openSwipe;
    private Vector2 lastMousePosition;
    private float velocity = 0f;


    public void Init(GameCamera camera)
    {
        gameCamera = camera;
        camera.transform.parent = this.transform;
    }



    void Update()
    {
        if (!openSwipe) return;
        SwipeCamera();

#if UNITY_EDITOR
        Debug.Log("openSwipe: " + openSwipe);
#endif
    }
    private void SwipeCamera()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lastMousePosition = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            Vector2 currentMousePosition = Input.mousePosition;
            Vector2 delta = currentMousePosition - lastMousePosition;

            if (Mathf.Abs(delta.y) > Mathf.Abs(delta.x)) // 僅垂直滑動
            {
                float moveAmount = -delta.y * moveSpeed * 0.01f; // 移除了 Time.deltaTime 並做比例調整
                MoveCamera(moveAmount);
                velocity = moveAmount;
            }

            lastMousePosition = currentMousePosition;
        }

        // 滾輪僅在電腦上有效
        if (!Application.isMobilePlatform)
        {
            float scrollInput = Input.GetAxis("Mouse ScrollWheel");
            Debug.Log("Mouse ScrollWheel: " + Input.GetAxis("Mouse ScrollWheel"));
            if (Mathf.Abs(scrollInput) > 0)
            {
                float moveAmount = scrollInput * scrollSpeed;
                MoveCamera(moveAmount);
            }
        }

        // 慣性滑動
        if (!Input.GetMouseButton(0))
        {
            if (Mathf.Abs(velocity) > 0.01f)
            {
                MoveCamera(velocity);
                velocity *= 0.9f; // 摩擦力
            }
            else
            {
                velocity = 0f;
            }
        }
    }

    private void MoveCamera(float moveAmount)
    {
        if (gameCamera == null)
        {
            Debug.LogError("GameCamera is not assigned.");
            return;
        }

        Vector3 newPosition = gameCamera.transform.position + new Vector3(0, moveAmount, 0);
        // 限制 Y 軸範圍
        float clampedY = Mathf.Clamp(newPosition.y, minY, maxY);
        gameCamera.transform.position = new Vector3(newPosition.x, clampedY, newPosition.z);
    }
}