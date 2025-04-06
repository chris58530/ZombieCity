using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class LoadSceneCmd : ICommand
{
    public string[] sceneNames; 

    public override void Execute(MonoBehaviour mono)
    {
        mono.StartCoroutine(LoadScenesAndMoveObjects(sceneNames));
    }

    private IEnumerator LoadScenesAndMoveObjects(string[] sceneNames)
    {
        Scene currentScene = SceneManager.GetActiveScene();
        List<AsyncOperation> loadOperations = new List<AsyncOperation>();
        List<Scene> loadedScenes = new List<Scene>();

        // 開始加載所有場景
        foreach (var sceneName in sceneNames)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            if (asyncLoad == null)
            {
                Debug.LogError($"LoadSceneCmd: 無法載入場景 {sceneName}");
                continue;
            }

            loadOperations.Add(asyncLoad);
        }

        // 等待所有場景加載完成
        foreach (var op in loadOperations)
        {
            while (!op.isDone)
            {
                yield return null;
            }
        }

        // 將每個載入的場景的物件移動到目前場景
        foreach (var sceneName in sceneNames)
        {
            Scene loadedScene = SceneManager.GetSceneByName(sceneName);
            if (!loadedScene.IsValid())
            {
                Debug.LogError($"LoadSceneCmd: 無法找到載入的場景 {sceneName}");
                continue;
            }

            GameObject[] rootObjects = loadedScene.GetRootGameObjects();
            foreach (var obj in rootObjects)
            {
                SceneManager.MoveGameObjectToScene(obj, currentScene);
            }

            yield return SceneManager.UnloadSceneAsync(loadedScene);
        }

        SetComplete();
    }
}