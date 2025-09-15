using UnityEngine;
using UnityEditor;

/// <summary>
/// 波次時間軸預覽視窗
/// </summary>
public class WaveTimelineWindow : EditorWindow
{
    private WaveData waveData;
    private Vector2 scrollPosition;
    private float timelineScale = 50f; // 每秒的像素數
    private float timelineHeight = 30f;
    private Color[] entryColors = new Color[]
    {
        Color.red, Color.green, Color.blue, Color.yellow, Color.magenta, Color.cyan
    };

    public static void ShowWindow(WaveData data)
    {
        WaveTimelineWindow window = GetWindow<WaveTimelineWindow>("波次時間軸");
        window.waveData = data;
        window.minSize = new Vector2(600, 400);
        window.Show();
    }

    private void OnGUI()
    {
        if (waveData == null)
        {
            EditorGUILayout.LabelField("請選擇一個 WaveData");
            return;
        }

        EditorGUILayout.LabelField($"波次: {waveData.waveName}", EditorStyles.boldLabel);

        // 控制項
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("時間軸縮放:", GUILayout.Width(80));
        timelineScale = EditorGUILayout.Slider(timelineScale, 10f, 200f);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();

        // 時間軸
        DrawTimeline();
    }

    private void DrawTimeline()
    {
        if (waveData.spawnEntries.Count == 0)
        {
            EditorGUILayout.LabelField("沒有出怪條目");
            return;
        }

        float maxTime = Mathf.Max(waveData.GetCalculatedDuration(), 30f);
        float timelineWidth = maxTime * timelineScale;

        // 捲軸區域
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        GUILayoutOption[] layoutOptions = new GUILayoutOption[]
        {
            GUILayout.Width(timelineWidth + 50),
            GUILayout.Height((waveData.spawnEntries.Count + 2) * (timelineHeight + 5) + 50)
        };

        Rect timelineRect = GUILayoutUtility.GetRect(timelineWidth + 50,
            (waveData.spawnEntries.Count + 2) * (timelineHeight + 5) + 50);

        // 繪製背景
        EditorGUI.DrawRect(timelineRect, new Color(0.2f, 0.2f, 0.2f, 0.1f));

        // 繪製時間刻度
        DrawTimeScale(timelineRect, maxTime);

        // 繪製出怪條目
        for (int i = 0; i < waveData.spawnEntries.Count; i++)
        {
            DrawSpawnEntryTimeline(timelineRect, waveData.spawnEntries[i], i);
        }

        EditorGUILayout.EndScrollView();

        // 圖例
        DrawLegend();
    }

    private void DrawTimeScale(Rect timelineRect, float maxTime)
    {
        float y = timelineRect.y + 10;

        // 繪製主軸線
        Vector3 start = new Vector3(timelineRect.x + 30, y, 0);
        Vector3 end = new Vector3(timelineRect.x + 30 + maxTime * timelineScale, y, 0);

        Handles.color = Color.white;
        Handles.DrawLine(start, end);

        // 繪製時間刻度
        for (int second = 0; second <= maxTime; second += 5)
        {
            float x = timelineRect.x + 30 + second * timelineScale;

            // 刻度線
            Vector3 tickStart = new Vector3(x, y - 5, 0);
            Vector3 tickEnd = new Vector3(x, y + 5, 0);
            Handles.DrawLine(tickStart, tickEnd);

            // 時間標籤
            Rect labelRect = new Rect(x - 15, y + 8, 30, 20);
            EditorGUI.LabelField(labelRect, $"{second}s", EditorStyles.centeredGreyMiniLabel);
        }
    }

    private void DrawSpawnEntryTimeline(Rect timelineRect, SpawnEntry entry, int index)
    {
        float y = timelineRect.y + 40 + index * (timelineHeight + 5);
        float startX = timelineRect.x + 30 + entry.spawnTime * timelineScale;

        // 計算條目持續時間
        float duration = (entry.spawnCount - 1) * entry.spawnInterval;
        float width = Mathf.Max(duration * timelineScale, 5f);

        // 繪製條目矩形
        Rect entryRect = new Rect(startX, y, width, timelineHeight);
        Color entryColor = entryColors[index % entryColors.Length];
        entryColor.a = 0.7f;

        EditorGUI.DrawRect(entryRect, entryColor);

        // 繪製邊框
        entryColor.a = 1f;
        Handles.color = entryColor;

        // 使用 Handles.DrawLines 繪製矩形邊框
        Vector3[] corners = new Vector3[]
        {
            new Vector3(entryRect.xMin, entryRect.yMin, 0),
            new Vector3(entryRect.xMax, entryRect.yMin, 0),
            new Vector3(entryRect.xMax, entryRect.yMax, 0),
            new Vector3(entryRect.xMin, entryRect.yMax, 0),
            new Vector3(entryRect.xMin, entryRect.yMin, 0)
        };
        Handles.DrawPolyLine(corners);

        // 標籤
        string label = "";
        if (entry.enemyPrefab != null)
        {
            label = entry.enemyPrefab.name;
        }
        label += $" x{entry.spawnCount}";

        Rect labelRect = new Rect(startX + 2, y + 2, width - 4, timelineHeight - 4);

        GUIStyle labelStyle = new GUIStyle(EditorStyles.miniLabel);
        labelStyle.normal.textColor = Color.white;
        labelStyle.fontStyle = FontStyle.Bold;

        EditorGUI.LabelField(labelRect, label, labelStyle);

        // 左側標籤（時間）
        Rect timeRect = new Rect(timelineRect.x, y, 25, timelineHeight);
        EditorGUI.LabelField(timeRect, $"{entry.spawnTime:F1}s", EditorStyles.miniLabel);
    }

    private void DrawLegend()
    {
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("圖例:", EditorStyles.boldLabel);

        for (int i = 0; i < waveData.spawnEntries.Count && i < entryColors.Length; i++)
        {
            EditorGUILayout.BeginHorizontal();

            // 顏色方塊
            Rect colorRect = GUILayoutUtility.GetRect(15, 15);
            EditorGUI.DrawRect(colorRect, entryColors[i % entryColors.Length]);

            // 描述
            string description = $"條目 {i + 1}";
            if (waveData.spawnEntries[i].enemyPrefab != null)
            {
                description += $": {waveData.spawnEntries[i].enemyPrefab.name}";
            }

            EditorGUILayout.LabelField(description);

            EditorGUILayout.EndHorizontal();
        }
    }
}