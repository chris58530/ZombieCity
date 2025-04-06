using UnityEngine;
using Lean.Touch;

public class CameraSwipeController : MonoBehaviour
{
    private GameCamera gameCamera;
    public float moveSpeed = 10f;
    public float scrollSpeed = 5f;
    public float minY = -30f;
    public float maxY = -5f;
    public bool openSwipe;
    private Vector2 lastMousePosition;


    public void Init(GameCamera camera, float minY)
    {
        gameCamera = camera;
        camera.transform.parent = this.transform;
        this.minY = minY;
    }

    

    void Update()
    {
        SwipeCamera();
    }
    private void SwipeCamera(){
        if (!openSwipe) return;
        if (LeanTouch.Fingers.Count > 0)
        {
            LeanFinger finger = LeanTouch.Fingers[0];
            Vector2 currentPosition = finger.ScreenPosition;

            if (finger.Down)
            {
                lastMousePosition = currentPosition;
            }
            else if (finger.Set)
            {
                Vector2 delta = currentPosition - lastMousePosition;
                if (Mathf.Abs(delta.y) > Mathf.Abs(delta.x)) // 確保是垂直滑動
                {
                    float moveAmount = -delta.y * moveSpeed * Time.deltaTime;
                    MoveCamera(moveAmount);
                }
                lastMousePosition = currentPosition;
            }
        }
        //  滑鼠滾輪輸入（電腦測試）
        else
        {
            float scrollInput = Input.GetAxis("Mouse ScrollWheel");
            if (Mathf.Abs(scrollInput) > 0)
            {
                float moveAmount = scrollInput * scrollSpeed;
                MoveCamera(moveAmount);
            }

            // 3. 按下滑鼠模擬觸控（電腦測試）
            if (Input.GetMouseButtonDown(0)) // 左鍵按下
            {
                lastMousePosition = Input.mousePosition;
            }
            else if (Input.GetMouseButton(0)) // 左鍵按住並移動
            {
                Vector2 currentMousePosition = Input.mousePosition;
                Vector2 delta = currentMousePosition - lastMousePosition;
                if (Mathf.Abs(delta.y) > Mathf.Abs(delta.x)) // 確保是垂直移動
                {
                    float moveAmount = -delta.y * moveSpeed * Time.deltaTime;
                    MoveCamera(moveAmount);
                }
                lastMousePosition = currentMousePosition;
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