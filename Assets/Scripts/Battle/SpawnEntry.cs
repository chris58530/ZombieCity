using UnityEngine;
using DG.Tweening;

/// <summary>
/// 單個出怪條目 - 定義敵人、路徑、時間等屬性
/// </summary>
[System.Serializable]
public class SpawnEntry
{
    [Header("基本設定")]
    [Tooltip("要生成的敵人預製體")]
    public GameObject enemyPrefab;

    [Tooltip("生成數量")]
    public int spawnCount = 1;

    [Tooltip("在波次開始後的第幾秒觸發")]
    public float spawnTime = 0f;

    [Tooltip("多個敵人之間的間隔時間（秒）")]
    public float spawnInterval = 0.1f;

    [Header("路徑設定")]
    [Tooltip("移動路徑類型")]
    public PathType pathType = PathType.DOTweenPath;

    [Tooltip("DOTweenPath 組件引用（推薦方式）")]
    public DOTweenPath doTweenPath;

    [Tooltip("自定義路徑點（備用方式）")]
    public Vector3[] customPathPoints;

    [Tooltip("移動持續時間")]
    public float moveDuration = 5f;

    [Tooltip("路徑動畫類型")]
    public Ease pathEase = Ease.Linear;

    [Header("敵人屬性覆寫")]
    [Tooltip("是否覆寫HP")]
    public bool overrideHP = false;

    [Tooltip("自定義HP值")]
    public int customHP = 100;

    [Tooltip("是否覆寫速度")]
    public bool overrideSpeed = false;

    [Tooltip("自定義移動速度倍率")]
    public float customSpeedMultiplier = 1f;

    [Header("進階設定")]
    [Tooltip("生成位置偏移（相對於路徑起點）")]
    public Vector3 spawnOffset = Vector3.zero;

    [Tooltip("敵人標籤（用於特殊邏輯識別）")]
    public string enemyTag = "";

    /// <summary>
    /// 獲取有效的路徑點
    /// </summary>
    public Vector3[] GetPathPoints()
    {
        switch (pathType)
        {
            case PathType.DOTweenPath:
                if (doTweenPath != null)
                {
                    return doTweenPath.wps.ToArray();
                }
                break;

            case PathType.Custom:
                if (customPathPoints != null && customPathPoints.Length > 0)
                {
                    return customPathPoints;
                }
                break;
        }

        // 備用：返回原點
        return new Vector3[] { Vector3.zero };
    }

    /// <summary>
    /// 獲取生成位置
    /// </summary>
    public Vector3 GetSpawnPosition()
    {
        Vector3[] pathPoints = GetPathPoints();
        Vector3 startPoint = pathPoints.Length > 0 ? pathPoints[0] : Vector3.zero;
        return startPoint + spawnOffset;
    }

    /// <summary>
    /// 創建 DOTween 路徑動畫
    /// </summary>
    public Tween CreatePathTween(Transform target)
    {
        Vector3[] pathPoints = GetPathPoints();

        if (pathPoints.Length <= 1)
        {
            Debug.LogWarning($"路徑點不足，無法創建動畫：{enemyPrefab?.name}");
            return null;
        }

        // 使用 DOTween 創建路徑動畫
        return target.DOPath(pathPoints, moveDuration, DG.Tweening.PathType.CatmullRom)
                    .SetEase(pathEase)
                    .SetAutoKill(true);
    }
}

/// <summary>
/// 路徑類型枚舉
/// </summary>
public enum PathType
{
    DOTweenPath,    // 使用 DOTweenPath 組件
    Custom          // 使用自定義路徑點
}