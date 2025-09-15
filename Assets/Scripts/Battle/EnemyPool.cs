using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

/// <summary>
/// 敵人物件池 - 管理敵人的創建和回收，避免 GC
/// </summary>
public class EnemyPool : MonoBehaviour
{
    [Header("物件池設定")]
    [SerializeField] private Transform poolParent;
    [SerializeField] private int defaultPoolSize = 10;
    [SerializeField] private int maxPoolSize = 50;
    [SerializeField] private bool autoExpand = true;

    [Header("調試")]
    [SerializeField] private bool enableDebugLog = false;
    [SerializeField] private bool showPoolStats = true;

    // 物件池數據結構
    private Dictionary<string, Queue<GameObject>> availablePools = new Dictionary<string, Queue<GameObject>>();
    private Dictionary<string, List<GameObject>> activePools = new Dictionary<string, List<GameObject>>();
    private Dictionary<string, GameObject> prefabRegistry = new Dictionary<string, GameObject>();

    // 統計資訊
    [System.Serializable]
    public class PoolStats
    {
        public string prefabName;
        public int available;
        public int active;
        public int total;
    }

    [SerializeField] private List<PoolStats> poolStats = new List<PoolStats>();

    private void Awake()
    {
        if (poolParent == null)
            poolParent = transform;
    }

    private void Update()
    {
        if (showPoolStats)
        {
            UpdatePoolStats();
        }
    }

    /// <summary>
    /// 預先建立物件池
    /// </summary>
    public void PreloadPool(GameObject prefab, int count = -1)
    {
        if (prefab == null)
        {
            LogError("預製體為空");
            return;
        }

        if (count == -1)
            count = defaultPoolSize;

        string prefabName = prefab.name;

        // 註冊預製體
        if (!prefabRegistry.ContainsKey(prefabName))
        {
            prefabRegistry[prefabName] = prefab;
        }

        // 初始化物件池
        if (!availablePools.ContainsKey(prefabName))
        {
            availablePools[prefabName] = new Queue<GameObject>();
            activePools[prefabName] = new List<GameObject>();
        }

        // 創建物件
        for (int i = 0; i < count; i++)
        {
            GameObject obj = CreateNewObject(prefab);
            obj.SetActive(false);
            availablePools[prefabName].Enqueue(obj);
        }

        LogInfo($"預載物件池：{prefabName} x{count}");
    }

    /// <summary>
    /// 從物件池獲取敵人
    /// </summary>
    public GameObject GetEnemy(GameObject prefab)
    {
        if (prefab == null)
        {
            LogError("預製體為空");
            return null;
        }

        string prefabName = prefab.name;

        // 註冊預製體（如果尚未註冊）
        if (!prefabRegistry.ContainsKey(prefabName))
        {
            prefabRegistry[prefabName] = prefab;
        }

        // 初始化物件池（如果尚未初始化）
        if (!availablePools.ContainsKey(prefabName))
        {
            availablePools[prefabName] = new Queue<GameObject>();
            activePools[prefabName] = new List<GameObject>();
        }

        GameObject enemy = null;

        // 從可用池中獲取
        if (availablePools[prefabName].Count > 0)
        {
            enemy = availablePools[prefabName].Dequeue();
        }
        else if (autoExpand && GetTotalCount(prefabName) < maxPoolSize)
        {
            // 自動擴展物件池
            enemy = CreateNewObject(prefab);
            LogInfo($"擴展物件池：{prefabName}");
        }
        else
        {
            LogWarning($"物件池已滿或無可用物件：{prefabName}");
            return null;
        }

        if (enemy != null)
        {
            // 重置物件狀態
            ResetObject(enemy);

            // 啟用物件
            enemy.SetActive(true);

            // 移至活動池
            activePools[prefabName].Add(enemy);

            LogInfo($"獲取敵人：{prefabName}，活動數量：{activePools[prefabName].Count}");
        }

        return enemy;
    }

    /// <summary>
    /// 歸還敵人到物件池
    /// </summary>
    public void ReturnEnemy(GameObject enemy)
    {
        if (enemy == null)
            return;

        string prefabName = GetPrefabName(enemy);

        if (string.IsNullOrEmpty(prefabName))
        {
            LogWarning($"無法識別敵人類型：{enemy.name}");
            Destroy(enemy);
            return;
        }

        // 確保物件池存在
        if (!activePools.ContainsKey(prefabName))
        {
            LogWarning($"找不到對應的物件池：{prefabName}");
            Destroy(enemy);
            return;
        }

        // 從活動池中移除
        if (activePools[prefabName].Remove(enemy))
        {
            // 重置物件狀態
            ResetObject(enemy);

            // 停用物件
            enemy.SetActive(false);

            // 歸還到可用池
            availablePools[prefabName].Enqueue(enemy);

            LogInfo($"歸還敵人：{prefabName}，可用數量：{availablePools[prefabName].Count}");
        }
        else
        {
            LogWarning($"敵人不在活動池中：{enemy.name}");
            Destroy(enemy);
        }
    }

    /// <summary>
    /// 創建新物件
    /// </summary>
    private GameObject CreateNewObject(GameObject prefab)
    {
        GameObject obj = Instantiate(prefab, poolParent);

        // 添加物件池標識組件
        PooledObject pooledComponent = obj.GetComponent<PooledObject>();
        if (pooledComponent == null)
        {
            pooledComponent = obj.AddComponent<PooledObject>();
        }
        pooledComponent.Initialize(this, prefab.name);

        return obj;
    }

    /// <summary>
    /// 重置物件狀態
    /// </summary>
    private void ResetObject(GameObject obj)
    {
        // 重置位置和旋轉
        obj.transform.position = Vector3.zero;
        obj.transform.rotation = Quaternion.identity;

        // 重置物件組件
        var resetComponent = obj.GetComponent<IPoolable>();
        if (resetComponent != null)
        {
            resetComponent.OnSpawned();
        }

        // 停止所有 DOTween 動畫
        DOTween.Kill(obj.transform);

        // 重置其他可能的狀態
        var rigidbody = obj.GetComponent<Rigidbody>();
        if (rigidbody != null)
        {
            rigidbody.linearVelocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
        }

        var rigidbody2D = obj.GetComponent<Rigidbody2D>();
        if (rigidbody2D != null)
        {
            rigidbody2D.linearVelocity = Vector2.zero;
            rigidbody2D.angularVelocity = 0f;
        }
    }

    /// <summary>
    /// 獲取物件的預製體名稱
    /// </summary>
    private string GetPrefabName(GameObject obj)
    {
        var pooledComponent = obj.GetComponent<PooledObject>();
        if (pooledComponent != null)
        {
            return pooledComponent.PrefabName;
        }

        // 備用方案：使用物件名稱（移除 (Clone) 後綴）
        string name = obj.name;
        if (name.EndsWith("(Clone)"))
        {
            name = name.Substring(0, name.Length - 7);
        }

        return name;
    }

    /// <summary>
    /// 獲取指定類型的總數量
    /// </summary>
    private int GetTotalCount(string prefabName)
    {
        int total = 0;

        if (availablePools.ContainsKey(prefabName))
            total += availablePools[prefabName].Count;

        if (activePools.ContainsKey(prefabName))
            total += activePools[prefabName].Count;

        return total;
    }

    /// <summary>
    /// 更新物件池統計
    /// </summary>
    private void UpdatePoolStats()
    {
        poolStats.Clear();

        foreach (var prefabName in prefabRegistry.Keys)
        {
            var stats = new PoolStats
            {
                prefabName = prefabName,
                available = availablePools.ContainsKey(prefabName) ? availablePools[prefabName].Count : 0,
                active = activePools.ContainsKey(prefabName) ? activePools[prefabName].Count : 0
            };
            stats.total = stats.available + stats.active;

            poolStats.Add(stats);
        }
    }

    /// <summary>
    /// 清理物件池
    /// </summary>
    public void ClearPool(string prefabName = null)
    {
        if (string.IsNullOrEmpty(prefabName))
        {
            // 清理所有物件池
            foreach (var pool in availablePools.Values)
            {
                while (pool.Count > 0)
                {
                    var obj = pool.Dequeue();
                    if (obj != null)
                        Destroy(obj);
                }
            }

            foreach (var pool in activePools.Values)
            {
                for (int i = pool.Count - 1; i >= 0; i--)
                {
                    if (pool[i] != null)
                        Destroy(pool[i]);
                }
                pool.Clear();
            }

            availablePools.Clear();
            activePools.Clear();
            prefabRegistry.Clear();

            LogInfo("清理所有物件池");
        }
        else
        {
            // 清理指定物件池
            if (availablePools.ContainsKey(prefabName))
            {
                while (availablePools[prefabName].Count > 0)
                {
                    var obj = availablePools[prefabName].Dequeue();
                    if (obj != null)
                        Destroy(obj);
                }
                availablePools.Remove(prefabName);
            }

            if (activePools.ContainsKey(prefabName))
            {
                for (int i = activePools[prefabName].Count - 1; i >= 0; i--)
                {
                    if (activePools[prefabName][i] != null)
                        Destroy(activePools[prefabName][i]);
                }
                activePools.Remove(prefabName);
            }

            prefabRegistry.Remove(prefabName);

            LogInfo($"清理物件池：{prefabName}");
        }
    }

    /// <summary>
    /// 獲取物件池統計資訊
    /// </summary>
    public string GetPoolStatsString()
    {
        var sb = new System.Text.StringBuilder();
        sb.AppendLine("=== 物件池統計 ===");

        foreach (var stats in poolStats)
        {
            sb.AppendLine($"{stats.prefabName}: 可用={stats.available}, 活動={stats.active}, 總計={stats.total}");
        }

        return sb.ToString();
    }

    // 日誌方法
    private void LogInfo(string message)
    {
        if (enableDebugLog)
            Debug.Log($"[EnemyPool] {message}");
    }

    private void LogWarning(string message)
    {
        if (enableDebugLog)
            Debug.LogWarning($"[EnemyPool] {message}");
    }

    private void LogError(string message)
    {
        Debug.LogError($"[EnemyPool] {message}");
    }

    private void OnDestroy()
    {
        ClearPool();
    }

    // 編輯器方法
    [ContextMenu("Print Pool Stats")]
    public void PrintPoolStats()
    {
        Debug.Log(GetPoolStatsString());
    }
}

/// <summary>
/// 物件池標識組件
/// </summary>
public class PooledObject : MonoBehaviour
{
    private EnemyPool pool;
    private string prefabName;

    public string PrefabName => prefabName;

    public void Initialize(EnemyPool enemyPool, string name)
    {
        pool = enemyPool;
        prefabName = name;
    }

    /// <summary>
    /// 歸還到物件池
    /// </summary>
    public void ReturnToPool()
    {
        if (pool != null)
        {
            pool.ReturnEnemy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}