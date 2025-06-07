using UnityEngine;

[ExecuteAlways]
public class CameraScaler : MonoBehaviour
{
    public float targetWidth = 10f; // 你想要的寬度（World 單位）
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
        AdjustCameraSize();
    }

    void AdjustCameraSize()
    {
        float screenAspect = (float)Screen.width / (float)Screen.height;
        cam.orthographicSize = targetWidth / screenAspect / 2f;

        Debug.Log($"螢幕寬度: {Screen.width}, 高度: {Screen.height}, 計算後Size: {cam.orthographicSize}");
    }

#if UNITY_EDITOR
    void Update()
    {
        if (!Application.isPlaying)
        {
            AdjustCameraSize(); // 編輯模式下也自動調整
        }
    }
#endif
}
