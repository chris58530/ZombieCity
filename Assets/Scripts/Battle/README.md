# 波次系統使用指南

## 📋 系統概述

這是一個完整的波次管理系統，支援：
- 🎯 **多波次管理**：可設定無限數量的波次
- ⏰ **精確時間控制**：每個敵人可指定在波次中的出現時間
- 🛣️ **DOTween Pro 路徑**：完整支援 DOTween Pro 的路徑編輯功能
- 🎮 **設計師友好**：提供可視化編輯器和時間軸預覽
- 🔄 **物件池優化**：避免頻繁創建銷毀造成的性能問題

## 🚀 快速開始

### 1. 創建波次數據
```csharp
// 在 Project 視窗中右鍵 -> Create -> Battle System -> Wave Data
// 這會創建一個新的 WaveData ScriptableObject
```

### 2. 設置場景
```csharp
// 在場景中添加以下組件：
// - MultiWaveManager（主管理器）
// - WaveRunner（波次執行器）
// - EnemyPool（敵人物件池）
// - DOTweenPathManager（路徑管理器）
```

### 3. 編輯路徑
```csharp
// 1. 創建空物件
GameObject pathObject = new GameObject("EnemyPath");

// 2. 添加 DOTweenPath 組件
DOTweenPath path = pathObject.AddComponent<DOTweenPath>();

// 3. 在 Scene 視窗中編輯路徑點
// 4. 在 WaveData 中引用這個 DOTweenPath
```

## 📊 核心組件說明

### SpawnEntry（出怪條目）
```csharp
[System.Serializable]
public class SpawnEntry
{
    public GameObject enemyPrefab;    // 敵人預製體
    public int spawnCount;            // 生成數量
    public float spawnTime;           // 在波次中的第幾秒出現
    public float spawnInterval;       // 多個敵人間的間隔
    public DOTweenPath doTweenPath;   // 移動路徑
    public float moveDuration;        // 移動持續時間
    // ... 更多設定
}
```

### WaveData（波次數據）
```csharp
[CreateAssetMenu(fileName = "New Wave Data", menuName = "Battle System/Wave Data")]
public class WaveData : ScriptableObject
{
    public string waveName;                    // 波次名稱
    public List<SpawnEntry> spawnEntries;      // 出怪條目列表
    public WaveCompletionType completionType; // 完成條件
    public int rewardCoins;                    // 獎勵金幣
    // ... 更多設定
}
```

## 🛠️ 使用步驟

### 步驟 1：創建敵人預製體
1. 確保敵人預製體有必要的組件：
   - Collider（碰撞器）
   - 血量系統（實現 IHealth 介面）
   - 移動系統（實現 IMovement 介面）

```csharp
// 範例敵人腳本
public class Enemy : MonoBehaviour, IHealth, IMovement, IPoolable
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private float moveSpeed = 2f;
    
    public void SetMaxHealth(int health) { maxHealth = health; }
    public void SetSpeedMultiplier(float multiplier) { /* 實現速度調整 */ }
    public void OnSpawnFromPool() { /* 從物件池生成時的重置邏輯 */ }
    public void OnReturnToPool() { /* 回到物件池時的清理邏輯 */ }
}
```

### 步驟 2：設置路徑
1. 在場景中創建空物件
2. 添加 `DOTweenPath` 組件
3. 在 Scene 視窗中編輯路徑點
4. 調整路徑設定（持續時間、動畫類型等）

### 步驟 3：創建波次數據
1. 在 Project 視窗右鍵 → Create → Battle System → Wave Data
2. 在 Inspector 中設置：
   - 波次名稱和描述
   - 添加出怪條目
   - 設置每個條目的敵人、時間、路徑
   - 配置完成條件和獎勵

### 步驟 4：配置管理器
```csharp
// 在 MultiWaveManager 中設置
public class MultiWaveManager : MonoBehaviour
{
    [SerializeField] private List<WaveData> waves; // 拖入創建的 WaveData
    
    void Start()
    {
        StartAllWaves(); // 開始所有波次
    }
}
```

## 🎨 編輯器功能

### 波次編輯器
- **自動排序**：按時間自動排序出怪條目
- **複製條目**：快速複製相似的出怪設定
- **即時預覽**：顯示波次統計資訊
- **數據驗證**：檢查設定是否有效

### 時間軸視圖
```csharp
// 在 WaveData Inspector 中點擊 "時間軸視圖" 按鈕
// 可視化顯示所有敵人的出現時間和持續時間
```

### 路徑編輯工具
```csharp
// DOTweenPathManager 提供的功能：
pathManager.CreateNewPath("PathName", waypoints);  // 創建新路徑
pathManager.PreviewPath(doTweenPath);              // 預覽路徑
pathManager.OptimizePath(doTweenPath);             // 優化路徑點
```

## 🔧 進階設定

### 自定義完成條件
```csharp
public enum WaveCompletionType
{
    AllEnemiesDefeated,  // 所有敵人被擊敗
    TimeLimit,           // 時間限制
    PlayerReachGoal,     // 玩家到達目標
    Custom               // 自定義條件
}
```

### 敵人屬性覆寫
```csharp
// 在 SpawnEntry 中可以覆寫：
entry.overrideHP = true;
entry.customHP = 150;           // 自定義血量
entry.overrideSpeed = true;
entry.customSpeedMultiplier = 1.5f; // 速度倍率
```

### 物件池設定
```csharp
// 預載特定敵人到物件池
enemyPool.PreloadPool(enemyPrefab, 10);

// 設置物件池大小限制
enemyPool.maxPoolSize = 50;
enemyPool.autoExpand = true;
```

## 🔄 與現有系統整合

### 方式 1：完全替換
```csharp
// 停用舊的 BattleZombieSpawnerView
zombieSpawner.enabled = false;

// 啟用新系統
multiWaveManager.StartAllWaves();
```

### 方式 2：混合使用
```csharp
// 使用新系統管理波次邏輯
// 但復用舊系統的敵人生成機制
public class HybridIntegration : MonoBehaviour
{
    private void OnEnemySpawned(WaveRunner runner, SpawnEntry entry, GameObject enemy)
    {
        // 將新系統生成的敵人註冊到舊系統
        legacySpawner.RegisterEnemy(enemy);
    }
}
```

## 📝 最佳實踐

### 1. 路徑設計
- 使用簡潔的路徑點，避免過度複雜
- 考慮路徑長度與移動時間的比例
- 為不同類型敵人設計不同的路徑

### 2. 時間安排
- 避免同時間大量敵人生成
- 考慮玩家的反應時間
- 在高難度波次間添加休息時間

### 3. 性能優化
- 使用物件池避免頻繁實例化
- 合理設置路徑解析度
- 避免過多並發的 DOTween 動畫

### 4. 調試建議
- 啟用調試日誌查看執行狀態
- 使用時間軸視圖檢查時間安排
- 利用驗證功能檢查數據完整性

## 🎯 範例場景設置

```csharp
// 完整的場景設置範例：

// 1. 創建主物件
GameObject battleManager = new GameObject("BattleManager");

// 2. 添加核心組件
MultiWaveManager waveManager = battleManager.AddComponent<MultiWaveManager>();
WaveRunner waveRunner = battleManager.AddComponent<WaveRunner>();
EnemyPool enemyPool = battleManager.AddComponent<EnemyPool>();

// 3. 創建路徑管理器
GameObject pathManagerObject = new GameObject("PathManager");
DOTweenPathManager pathManager = pathManagerObject.AddComponent<DOTweenPathManager>();

// 4. 設置引用
waveManager.waveRunner = waveRunner;
waveManager.enemyPool = enemyPool;
waveRunner.enemyPool = enemyPool;

// 5. 添加波次數據
waveManager.waves.Add(yourWaveData);

// 6. 開始戰鬥
waveManager.StartAllWaves();
```

這套系統提供了完整的波次管理解決方案，既保持了高度的可定制性，又提供了設計師友好的編輯體驗。您可以根據專案需求進行進一步的客製化。