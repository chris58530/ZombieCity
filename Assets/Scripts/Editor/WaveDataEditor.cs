using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

/// <summary>
/// WaveData 自定義編輯器
/// </summary>
[CustomEditor(typeof(WaveData))]
public class WaveDataEditor : Editor
{
    private SerializedProperty waveNameProp;
    private SerializedProperty waveDescriptionProp;
    private SerializedProperty waveDurationProp;
    private SerializedProperty spawnEntriesProp;
    private SerializedProperty completionTypeProp;
    private SerializedProperty timeLimitProp;
    private SerializedProperty rewardCoinsProp;
    private SerializedProperty rewardExpProp;

    private bool showSpawnEntries = true;
    private bool showRewards = true;
    private bool showPreview = true;

    private Vector2 scrollPosition;

    private void OnEnable()
    {
        waveNameProp = serializedObject.FindProperty("waveName");
        waveDescriptionProp = serializedObject.FindProperty("waveDescription");
        waveDurationProp = serializedObject.FindProperty("waveDuration");
        spawnEntriesProp = serializedObject.FindProperty("spawnEntries");
        completionTypeProp = serializedObject.FindProperty("completionType");
        timeLimitProp = serializedObject.FindProperty("timeLimit");
        rewardCoinsProp = serializedObject.FindProperty("rewardCoins");
        rewardExpProp = serializedObject.FindProperty("rewardExp");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        WaveData waveData = (WaveData)target;

        EditorGUILayout.Space();

        // 標題
        EditorGUILayout.LabelField("波次設計工具", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        // 基本資訊
        DrawBasicInfo();

        EditorGUILayout.Space();

        // 出怪條目
        DrawSpawnEntries();

        EditorGUILayout.Space();

        // 完成條件
        DrawCompletionSettings();

        EditorGUILayout.Space();

        // 獎勵設定
        DrawRewardSettings();

        EditorGUILayout.Space();

        // 預覽和工具
        DrawPreviewAndTools(waveData);

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawBasicInfo()
    {
        EditorGUILayout.LabelField("基本資訊", EditorStyles.boldLabel);

        EditorGUILayout.PropertyField(waveNameProp, new GUIContent("波次名稱"));
        EditorGUILayout.PropertyField(waveDescriptionProp, new GUIContent("波次描述"));
        EditorGUILayout.PropertyField(waveDurationProp, new GUIContent("波次持續時間 (秒)", "0表示無限制"));
    }

    private void DrawSpawnEntries()
    {
        showSpawnEntries = EditorGUILayout.Foldout(showSpawnEntries, "出怪設定", true);

        if (showSpawnEntries)
        {
            EditorGUI.indentLevel++;

            // 工具列
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("添加出怪條目", GUILayout.Width(120)))
            {
                spawnEntriesProp.arraySize++;
                SerializedProperty newEntry = spawnEntriesProp.GetArrayElementAtIndex(spawnEntriesProp.arraySize - 1);
                ResetSpawnEntry(newEntry);
            }

            if (GUILayout.Button("排序 (按時間)", GUILayout.Width(120)))
            {
                SortSpawnEntriesByTime();
            }

            if (GUILayout.Button("清空所有", GUILayout.Width(80)))
            {
                if (EditorUtility.DisplayDialog("確認清空", "確定要清空所有出怪條目嗎？", "確定", "取消"))
                {
                    spawnEntriesProp.arraySize = 0;
                }
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();

            // 出怪條目列表
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Height(300));

            for (int i = 0; i < spawnEntriesProp.arraySize; i++)
            {
                DrawSpawnEntry(i);
            }

            EditorGUILayout.EndScrollView();

            EditorGUI.indentLevel--;
        }
    }

    private void DrawSpawnEntry(int index)
    {
        SerializedProperty entry = spawnEntriesProp.GetArrayElementAtIndex(index);

        EditorGUILayout.BeginVertical("box");

        // 標題列
        EditorGUILayout.BeginHorizontal();

        SerializedProperty enemyPrefabProp = entry.FindPropertyRelative("enemyPrefab");
        SerializedProperty spawnTimeProp = entry.FindPropertyRelative("spawnTime");

        string title = $"條目 {index + 1}";
        if (enemyPrefabProp.objectReferenceValue != null)
        {
            title += $": {enemyPrefabProp.objectReferenceValue.name}";
        }
        title += $" (第 {spawnTimeProp.floatValue:F1} 秒)";

        EditorGUILayout.LabelField(title, EditorStyles.boldLabel);

        GUILayout.FlexibleSpace();

        // 操作按鈕
        if (GUILayout.Button("↑", GUILayout.Width(20)) && index > 0)
        {
            spawnEntriesProp.MoveArrayElement(index, index - 1);
        }

        if (GUILayout.Button("↓", GUILayout.Width(20)) && index < spawnEntriesProp.arraySize - 1)
        {
            spawnEntriesProp.MoveArrayElement(index, index + 1);
        }

        if (GUILayout.Button("複製", GUILayout.Width(40)))
        {
            DuplicateSpawnEntry(index);
        }

        if (GUILayout.Button("×", GUILayout.Width(20)))
        {
            spawnEntriesProp.DeleteArrayElementAtIndex(index);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
            return;
        }

        EditorGUILayout.EndHorizontal();

        // 內容
        EditorGUI.indentLevel++;

        EditorGUILayout.PropertyField(enemyPrefabProp, new GUIContent("敵人預製體"));

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PropertyField(entry.FindPropertyRelative("spawnCount"), new GUIContent("數量"));
        EditorGUILayout.PropertyField(spawnTimeProp, new GUIContent("時間 (秒)"));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.PropertyField(entry.FindPropertyRelative("spawnInterval"), new GUIContent("間隔 (秒)"));

        // 路徑設定
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("路徑設定", EditorStyles.boldLabel);

        EditorGUILayout.PropertyField(entry.FindPropertyRelative("pathType"), new GUIContent("路徑類型"));

        PathType pathType = (PathType)entry.FindPropertyRelative("pathType").enumValueIndex;

        if (pathType == PathType.DOTweenPath)
        {
            EditorGUILayout.PropertyField(entry.FindPropertyRelative("doTweenPath"), new GUIContent("DOTween 路徑"));
        }
        else
        {
            EditorGUILayout.PropertyField(entry.FindPropertyRelative("customPathPoints"), new GUIContent("自定義路徑點"), true);
        }

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PropertyField(entry.FindPropertyRelative("moveDuration"), new GUIContent("移動時間"));
        EditorGUILayout.PropertyField(entry.FindPropertyRelative("pathEase"), new GUIContent("動畫類型"));
        EditorGUILayout.EndHorizontal();

        // 屬性覆寫
        SerializedProperty overrideHPProp = entry.FindPropertyRelative("overrideHP");
        SerializedProperty overrideSpeedProp = entry.FindPropertyRelative("overrideSpeed");

        if (overrideHPProp.boolValue || overrideSpeedProp.boolValue)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("屬性覆寫", EditorStyles.boldLabel);

            if (overrideHPProp.boolValue)
            {
                EditorGUILayout.PropertyField(entry.FindPropertyRelative("customHP"), new GUIContent("自定義 HP"));
            }

            if (overrideSpeedProp.boolValue)
            {
                EditorGUILayout.PropertyField(entry.FindPropertyRelative("customSpeedMultiplier"), new GUIContent("速度倍率"));
            }
        }

        // 折疊選項
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PropertyField(overrideHPProp, new GUIContent("覆寫 HP"), GUILayout.Width(80));
        EditorGUILayout.PropertyField(overrideSpeedProp, new GUIContent("覆寫速度"), GUILayout.Width(80));
        EditorGUILayout.EndHorizontal();

        EditorGUI.indentLevel--;

        EditorGUILayout.EndVertical();
        EditorGUILayout.Space();
    }

    private void DrawCompletionSettings()
    {
        EditorGUILayout.LabelField("完成條件", EditorStyles.boldLabel);

        EditorGUILayout.PropertyField(completionTypeProp, new GUIContent("完成類型"));

        WaveCompletionType completionType = (WaveCompletionType)completionTypeProp.enumValueIndex;

        if (completionType == WaveCompletionType.TimeLimit)
        {
            EditorGUILayout.PropertyField(timeLimitProp, new GUIContent("時間限制 (秒)"));
        }
    }

    private void DrawRewardSettings()
    {
        showRewards = EditorGUILayout.Foldout(showRewards, "獎勵設定", true);

        if (showRewards)
        {
            EditorGUI.indentLevel++;

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(rewardCoinsProp, new GUIContent("金幣"));
            EditorGUILayout.PropertyField(rewardExpProp, new GUIContent("經驗"));
            EditorGUILayout.EndHorizontal();

            EditorGUI.indentLevel--;
        }
    }

    private void DrawPreviewAndTools(WaveData waveData)
    {
        showPreview = EditorGUILayout.Foldout(showPreview, "預覽和工具", true);

        if (showPreview)
        {
            EditorGUI.indentLevel++;

            // 統計資訊
            EditorGUILayout.LabelField("波次統計", EditorStyles.boldLabel);

            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField($"總敵人數量: {waveData.GetTotalEnemyCount()}");
            EditorGUILayout.LabelField($"計算持續時間: {waveData.GetCalculatedDuration():F1} 秒");
            EditorGUILayout.LabelField($"出怪條目數量: {waveData.spawnEntries.Count}");
            EditorGUILayout.EndVertical();

            EditorGUILayout.Space();

            // 工具按鈕
            EditorGUILayout.LabelField("工具", EditorStyles.boldLabel);

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("驗證數據"))
            {
                string errorMessage;
                if (waveData.ValidateWaveData(out errorMessage))
                {
                    EditorUtility.DisplayDialog("驗證結果", "波次數據驗證通過！", "確定");
                }
                else
                {
                    EditorUtility.DisplayDialog("驗證失敗", errorMessage, "確定");
                }
            }

            if (GUILayout.Button("預覽資訊"))
            {
                waveData.PreviewWaveInfo();
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("生成範例數據"))
            {
                GenerateSampleData();
            }

            if (GUILayout.Button("時間軸視圖"))
            {
                WaveTimelineWindow.ShowWindow(waveData);
            }

            EditorGUILayout.EndHorizontal();

            EditorGUI.indentLevel--;
        }
    }

    private void ResetSpawnEntry(SerializedProperty entry)
    {
        entry.FindPropertyRelative("enemyPrefab").objectReferenceValue = null;
        entry.FindPropertyRelative("spawnCount").intValue = 1;
        entry.FindPropertyRelative("spawnTime").floatValue = 0f;
        entry.FindPropertyRelative("spawnInterval").floatValue = 0.1f;
        entry.FindPropertyRelative("pathType").enumValueIndex = 0;
        entry.FindPropertyRelative("doTweenPath").objectReferenceValue = null;
        entry.FindPropertyRelative("moveDuration").floatValue = 5f;
        entry.FindPropertyRelative("pathEase").enumValueIndex = 0;
        entry.FindPropertyRelative("overrideHP").boolValue = false;
        entry.FindPropertyRelative("customHP").intValue = 100;
        entry.FindPropertyRelative("overrideSpeed").boolValue = false;
        entry.FindPropertyRelative("customSpeedMultiplier").floatValue = 1f;
    }

    private void DuplicateSpawnEntry(int index)
    {
        spawnEntriesProp.InsertArrayElementAtIndex(index);

        // 複製後的條目時間稍微延後
        SerializedProperty duplicatedEntry = spawnEntriesProp.GetArrayElementAtIndex(index + 1);
        SerializedProperty spawnTimeProp = duplicatedEntry.FindPropertyRelative("spawnTime");
        spawnTimeProp.floatValue += 1f;
    }

    private void SortSpawnEntriesByTime()
    {
        // 這裡需要手動排序，因為 SerializedProperty 不支持直接排序
        List<SpawnEntry> entries = new List<SpawnEntry>();

        WaveData waveData = (WaveData)target;
        entries.AddRange(waveData.spawnEntries);

        entries.Sort((a, b) => a.spawnTime.CompareTo(b.spawnTime));

        waveData.spawnEntries = entries;
        EditorUtility.SetDirty(target);
    }

    private void GenerateSampleData()
    {
        if (EditorUtility.DisplayDialog("生成範例數據", "這將清空現有數據並生成範例，確定繼續嗎？", "確定", "取消"))
        {
            WaveData waveData = (WaveData)target;

            waveData.waveName = "範例波次";
            waveData.waveDescription = "這是一個自動生成的範例波次";
            waveData.waveDuration = 30f;
            waveData.completionType = WaveCompletionType.AllEnemiesDefeated;
            waveData.rewardCoins = 100;
            waveData.rewardExp = 50;

            // 清空現有條目
            waveData.spawnEntries.Clear();

            // 添加幾個範例條目
            for (int i = 0; i < 3; i++)
            {
                SpawnEntry entry = new SpawnEntry();
                entry.spawnCount = Random.Range(1, 5);
                entry.spawnTime = i * 5f;
                entry.spawnInterval = 0.5f;
                entry.moveDuration = 10f;
                entry.pathType = PathType.Custom;
                entry.customPathPoints = new Vector3[]
                {
                    new Vector3(-10, 0, 0),
                    new Vector3(0, 2, 0),
                    new Vector3(10, 0, 0)
                };

                waveData.spawnEntries.Add(entry);
            }

            EditorUtility.SetDirty(target);
        }
    }
}