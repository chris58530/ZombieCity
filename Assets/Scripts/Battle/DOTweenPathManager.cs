using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

/// <summary>
/// DOTween 路徑管理器 - 管理和編輯 DOTweenPath
/// </summary>
public class DOTweenPathManager : MonoBehaviour
{
    [Header("路徑設定")]
    [SerializeField] private List<DOTweenPath> availablePaths = new List<DOTweenPath>();

    [Header("編輯工具")]
    [SerializeField] private bool showPathGizmos = true;
    [SerializeField] private Color pathColor = Color.green;
    [SerializeField] private Color waypointColor = Color.red;
    [SerializeField] private float waypointSize = 0.3f;

    [Header("預覽設定")]
    [SerializeField] private GameObject previewObject;
    [SerializeField] private bool autoPreview = false;
    [SerializeField] private float previewDuration = 5f;

    private void Awake()
    {
        // 自動搜尋場景中的 DOTweenPath
        RefreshAvailablePaths();
    }

    /// <summary>
    /// 刷新可用路徑列表
    /// </summary>
    [ContextMenu("Refresh Available Paths")]
    public void RefreshAvailablePaths()
    {
        availablePaths.Clear();

        // 搜尋場景中所有的 DOTweenPath
        DOTweenPath[] paths = FindObjectsByType<DOTweenPath>(FindObjectsSortMode.None);
        availablePaths.AddRange(paths);

        Debug.Log($"找到 {availablePaths.Count} 個 DOTweenPath");
    }

    /// <summary>
    /// 創建新的路徑
    /// </summary>
    public DOTweenPath CreateNewPath(string pathName, Vector3[] waypoints)
    {
        GameObject pathObject = new GameObject($"Path_{pathName}");
        pathObject.transform.SetParent(transform);

        DOTweenPath path = pathObject.AddComponent<DOTweenPath>();

        // 設置路徑點
        if (waypoints != null && waypoints.Length > 0)
        {
            path.wps = new List<Vector3>(waypoints);
        }
        else
        {
            // 預設路徑點
            path.wps = new List<Vector3>
            {
                Vector3.zero,
                Vector3.right * 5,
                Vector3.right * 10
            };
        }

        // 基本設定
        path.duration = 5f;
        path.easeType = Ease.Linear;
        path.pathType = DG.Tweening.PathType.CatmullRom;
        path.pathResolution = 10;
        path.isClosedPath = false;

        availablePaths.Add(path);

        Debug.Log($"創建新路徑：{pathName}");
        return path;
    }

    /// <summary>
    /// 複製路徑
    /// </summary>
    public DOTweenPath DuplicatePath(DOTweenPath originalPath, string newName)
    {
        if (originalPath == null)
        {
            Debug.LogError("原始路徑為空");
            return null;
        }

        GameObject pathObject = new GameObject($"Path_{newName}");
        pathObject.transform.SetParent(transform);

        DOTweenPath newPath = pathObject.AddComponent<DOTweenPath>();

        // 複製設定
        newPath.wps = new List<Vector3>(originalPath.wps);
        newPath.duration = originalPath.duration;
        newPath.easeType = originalPath.easeType;
        newPath.pathType = originalPath.pathType;
        newPath.pathResolution = originalPath.pathResolution;
        newPath.isClosedPath = originalPath.isClosedPath;
        newPath.lookAhead = originalPath.lookAhead;

        availablePaths.Add(newPath);

        Debug.Log($"複製路徑：{originalPath.name} -> {newName}");
        return newPath;
    }

    /// <summary>
    /// 刪除路徑
    /// </summary>
    public void DeletePath(DOTweenPath path)
    {
        if (path == null)
            return;

        availablePaths.Remove(path);

        if (Application.isPlaying)
        {
            Destroy(path.gameObject);
        }
        else
        {
            DestroyImmediate(path.gameObject);
        }

        Debug.Log($"刪除路徑：{path.name}");
    }

    /// <summary>
    /// 根據名稱獲取路徑
    /// </summary>
    public DOTweenPath GetPathByName(string pathName)
    {
        return availablePaths.Find(p => p.name.Contains(pathName));
    }

    /// <summary>
    /// 預覽路徑
    /// </summary>
    public void PreviewPath(DOTweenPath path)
    {
        if (path == null || previewObject == null)
        {
            Debug.LogWarning("路徑或預覽物件為空");
            return;
        }

        // 停止之前的預覽
        DOTween.Kill(previewObject.transform);

        // 設置起始位置
        if (path.wps.Count > 0)
        {
            previewObject.transform.position = path.wps[0];
        }

        // 開始預覽動畫
        previewObject.transform.DOPath(path.wps.ToArray(), previewDuration, path.pathType)
            .SetEase(path.easeType)
            .SetLoops(-1, LoopType.Restart)
            .SetId("PathPreview");

        Debug.Log($"開始預覽路徑：{path.name}");
    }

    /// <summary>
    /// 停止預覽
    /// </summary>
    public void StopPreview()
    {
        if (previewObject != null)
        {
            DOTween.Kill(previewObject.transform, "PathPreview");
        }
    }

    /// <summary>
    /// 獲取路徑總長度（近似值）
    /// </summary>
    public float GetPathLength(DOTweenPath path)
    {
        if (path == null || path.wps.Count < 2)
            return 0f;

        float totalLength = 0f;
        for (int i = 1; i < path.wps.Count; i++)
        {
            totalLength += Vector3.Distance(path.wps[i - 1], path.wps[i]);
        }

        return totalLength;
    }

    /// <summary>
    /// 反轉路徑
    /// </summary>
    public void ReversePath(DOTweenPath path)
    {
        if (path == null || path.wps.Count < 2)
            return;

        path.wps.Reverse();
        Debug.Log($"反轉路徑：{path.name}");
    }

    /// <summary>
    /// 優化路徑點（移除重複和過近的點）
    /// </summary>
    public void OptimizePath(DOTweenPath path, float minDistance = 0.5f)
    {
        if (path == null || path.wps.Count < 2)
            return;

        List<Vector3> optimizedPoints = new List<Vector3>();
        optimizedPoints.Add(path.wps[0]); // 保留第一個點

        for (int i = 1; i < path.wps.Count; i++)
        {
            Vector3 currentPoint = path.wps[i];
            Vector3 lastOptimizedPoint = optimizedPoints[optimizedPoints.Count - 1];

            if (Vector3.Distance(currentPoint, lastOptimizedPoint) >= minDistance)
            {
                optimizedPoints.Add(currentPoint);
            }
        }

        // 確保保留最後一個點
        if (optimizedPoints.Count > 1 &&
            Vector3.Distance(optimizedPoints[optimizedPoints.Count - 1], path.wps[path.wps.Count - 1]) > 0.01f)
        {
            optimizedPoints.Add(path.wps[path.wps.Count - 1]);
        }

        path.wps = optimizedPoints;
        Debug.Log($"優化路徑：{path.name}，從 {path.wps.Count} 個點優化為 {optimizedPoints.Count} 個點");
    }

    /// <summary>
    /// 將路徑轉換為世界座標
    /// </summary>
    public Vector3[] GetWorldPositions(DOTweenPath path)
    {
        if (path == null)
            return new Vector3[0];

        Vector3[] worldPositions = new Vector3[path.wps.Count];
        for (int i = 0; i < path.wps.Count; i++)
        {
            worldPositions[i] = path.transform.TransformPoint(path.wps[i]);
        }

        return worldPositions;
    }

    private void OnDrawGizmos()
    {
        if (!showPathGizmos)
            return;

        foreach (var path in availablePaths)
        {
            if (path == null || path.wps.Count < 2)
                continue;

            DrawPathGizmos(path);
        }
    }

    private void DrawPathGizmos(DOTweenPath path)
    {
        Gizmos.color = pathColor;

        // 繪製路徑線
        for (int i = 1; i < path.wps.Count; i++)
        {
            Vector3 from = path.transform.TransformPoint(path.wps[i - 1]);
            Vector3 to = path.transform.TransformPoint(path.wps[i]);
            Gizmos.DrawLine(from, to);
        }

        // 繪製路徑點
        Gizmos.color = waypointColor;
        for (int i = 0; i < path.wps.Count; i++)
        {
            Vector3 worldPos = path.transform.TransformPoint(path.wps[i]);
            Gizmos.DrawWireSphere(worldPos, waypointSize);

            // 繪製路徑點編號
#if UNITY_EDITOR
            UnityEditor.Handles.Label(worldPos + Vector3.up * 0.5f, i.ToString());
#endif
        }
    }

    // 編輯器專用方法
#if UNITY_EDITOR
    [ContextMenu("Create Test Path")]
    private void CreateTestPath()
    {
        Vector3[] testPoints = new Vector3[]
        {
            new Vector3(0, 0, 0),
            new Vector3(5, 0, 0),
            new Vector3(10, 2, 0),
            new Vector3(15, 0, 0),
            new Vector3(20, 0, 0)
        };

        CreateNewPath("TestPath", testPoints);
    }
#endif
}