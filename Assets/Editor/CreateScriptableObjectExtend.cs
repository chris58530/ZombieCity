using UnityEngine;
using UnityEditor;

public class CreateScriptableObjectExtend 
{
     static bool isInRenameMode = true;
    [MenuItem("Assets/Create/SO _F11", false)]
    static void CreateScriptableObject()
    {
        UnityEngine.Object obj = Selection.activeObject;

        if (obj != null && obj.GetType() == typeof(MonoScript))
        {
            var script = obj as MonoScript;

            if (script != null && script.GetClass().IsSubclassOf(typeof(ScriptableObject)))
            {
                var asset = ScriptableObject.CreateInstance(script.GetClass());
               
                if (isInRenameMode)
                {
                    ProjectWindowUtil.CreateAsset(asset, $"{script.name}.asset");
                    Selection.activeObject = obj;
                }
                else
                {
                    AssetDatabase.CreateAsset(asset, $"{script.name}.asset");
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                    Selection.activeObject = asset;
                }
            }
        }
    }
}
