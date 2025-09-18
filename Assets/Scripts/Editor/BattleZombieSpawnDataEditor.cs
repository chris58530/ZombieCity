using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BattleZombieSpawnData))]
public class BattleZombieSpawnDataEditor : Editor
{
    private SerializedProperty waveSettingsProp;
    private SerializedProperty previewInfoProp;

    private void OnEnable()
    {
        waveSettingsProp = serializedObject.FindProperty("waveSettings");
        previewInfoProp = serializedObject.FindProperty("previewInfo");
    }

    public override void OnInspectorGUI()
    {
        BattleZombieSpawnData data = (BattleZombieSpawnData)target;

        serializedObject.Update();

        // 標題
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("戰鬥殭屍生成資料", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        // 預覽資訊
        EditorGUILayout.PropertyField(previewInfoProp);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("波次設定", EditorStyles.boldLabel);

        // 快速操作按鈕
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("新增波次", GUILayout.Height(30)))
        {
            AddNewWave();
        }
        if (GUILayout.Button("移除最後波次", GUILayout.Height(30)) && waveSettingsProp.arraySize > 0)
        {
            waveSettingsProp.arraySize--;
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();

        // 波次列表
        for (int i = 0; i < waveSettingsProp.arraySize; i++)
        {
            DrawWaveElement(i);
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawWaveElement(int waveIndex)
    {
        SerializedProperty waveProp = waveSettingsProp.GetArrayElementAtIndex(waveIndex);
        SerializedProperty spawnDataProp = waveProp.FindPropertyRelative("spawnData");
        SerializedProperty waveNoteProp = waveProp.FindPropertyRelative("waveNote");

        // 波次標題框
        EditorGUILayout.BeginVertical("box");

        // 波次標題
        EditorGUILayout.BeginHorizontal();
        bool foldout = EditorPrefs.GetBool($"Wave_{waveIndex}_Foldout", true);
        foldout = EditorGUILayout.Foldout(foldout, $"第 {waveIndex + 1} 波 ({spawnDataProp.arraySize} 隻殭屍)", true);
        EditorPrefs.SetBool($"Wave_{waveIndex}_Foldout", foldout);

        GUILayout.FlexibleSpace();

        if (GUILayout.Button("複製波次", GUILayout.Width(80)))
        {
            DuplicateWave(waveIndex);
        }
        if (GUILayout.Button("刪除", GUILayout.Width(50)))
        {
            waveSettingsProp.DeleteArrayElementAtIndex(waveIndex);
            return;
        }
        EditorGUILayout.EndHorizontal();

        if (foldout)
        {
            EditorGUI.indentLevel++;

            // 波次備註
            EditorGUILayout.PropertyField(waveNoteProp, new GUIContent("波次備註"));

            EditorGUILayout.Space(5);

            // 殭屍操作按鈕
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("新增殭屍"))
            {
                AddZombieToWave(spawnDataProp);
            }
            if (GUILayout.Button("清空此波次") && EditorUtility.DisplayDialog("確認", "確定要清空此波次的所有殭屍嗎？", "確定", "取消"))
            {
                spawnDataProp.arraySize = 0;
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space(5);

            // 殭屍列表
            for (int j = 0; j < spawnDataProp.arraySize; j++)
            {
                DrawZombieElement(spawnDataProp, j, waveIndex);
            }

            EditorGUI.indentLevel--;
        }

        EditorGUILayout.EndVertical();
        EditorGUILayout.Space();
    }

    private void DrawZombieElement(SerializedProperty spawnDataProp, int zombieIndex, int waveIndex)
    {
        SerializedProperty zombieProp = spawnDataProp.GetArrayElementAtIndex(zombieIndex);
        SerializedProperty prefabProp = zombieProp.FindPropertyRelative("zombiePrefab");
        SerializedProperty spawnSecondProp = zombieProp.FindPropertyRelative("spawnSecond");
        SerializedProperty levelProp = zombieProp.FindPropertyRelative("level");
        SerializedProperty noteProp = zombieProp.FindPropertyRelative("zombieNote");

        EditorGUILayout.BeginVertical("helpbox");

        // 殭屍標題
        EditorGUILayout.BeginHorizontal();
        string zombieName = prefabProp.objectReferenceValue != null ? prefabProp.objectReferenceValue.name : "未設定殭屍";
        EditorGUILayout.LabelField($"殭屍 {zombieIndex + 1}: {zombieName}", EditorStyles.boldLabel);

        GUILayout.FlexibleSpace();
        if (GUILayout.Button("刪除", GUILayout.Width(50)))
        {
            spawnDataProp.DeleteArrayElementAtIndex(zombieIndex);
            return;
        }
        EditorGUILayout.EndHorizontal();

        // 殭屍屬性
        EditorGUILayout.PropertyField(prefabProp, new GUIContent("殭屍預製體"));

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PropertyField(spawnSecondProp, new GUIContent("生成時間 (秒)"));
        EditorGUILayout.PropertyField(levelProp, new GUIContent("等級"));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.PropertyField(noteProp, new GUIContent("備註"));

        EditorGUILayout.EndVertical();
    }

    private void AddNewWave()
    {
        waveSettingsProp.arraySize++;
        SerializedProperty newWave = waveSettingsProp.GetArrayElementAtIndex(waveSettingsProp.arraySize - 1);
        SerializedProperty spawnData = newWave.FindPropertyRelative("spawnData");
        SerializedProperty waveNote = newWave.FindPropertyRelative("waveNote");

        spawnData.arraySize = 0;
        waveNote.stringValue = $"第 {waveSettingsProp.arraySize} 波";
    }

    private void DuplicateWave(int waveIndex)
    {
        waveSettingsProp.InsertArrayElementAtIndex(waveIndex);
    }

    private void AddZombieToWave(SerializedProperty spawnDataProp)
    {
        spawnDataProp.arraySize++;
        SerializedProperty newZombie = spawnDataProp.GetArrayElementAtIndex(spawnDataProp.arraySize - 1);
        SerializedProperty levelProp = newZombie.FindPropertyRelative("level");
        SerializedProperty spawnSecondProp = newZombie.FindPropertyRelative("spawnSecond");
        SerializedProperty noteProp = newZombie.FindPropertyRelative("zombieNote");

        levelProp.intValue = 1;
        spawnSecondProp.floatValue = 0f;
        noteProp.stringValue = "新殭屍";
    }
}