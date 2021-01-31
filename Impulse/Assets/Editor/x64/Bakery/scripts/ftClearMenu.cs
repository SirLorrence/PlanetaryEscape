using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class ftClearMenu
{
    [MenuItem("Bakery/Utilities/Clear baked data", false, 44)]
    private static void ClearBakedData()
    {
        int val = EditorUtility.DisplayDialogComplex("Bakery", "Clear all Bakery data for currently loaded scenes?", "Clear data", "Clear all (data and settings)", "Cancel");
        if (val == 0)
        {
            var newStorages = new List<GameObject>();
            var sceneCount = SceneManager.sceneCount;
            for(int i=0; i<sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                if (!scene.isLoaded) continue;
                var go = ftLightmaps.FindInScene("!ftraceLightmaps", scene);
                if (go == null) continue;
                var storage = go.GetComponent<ftLightmapsStorage>();
                if (storage != null)
                {
                    var newGO = new GameObject();
                    var newStorage = newGO.AddComponent<ftLightmapsStorage>();
                    ftLightmapsStorage.CopySettings(storage, newStorage);
                    newStorages.Add(newGO);
                }
                Undo.DestroyObjectImmediate(go);
            }
            LightmapSettings.lightmaps = new LightmapData[0];
            for(int i=0; i<newStorages.Count; i++)
            {
                newStorages[i].name = "!ftraceLightmaps";
            }
        }
        else if (val == 1)
        {
            var sceneCount = SceneManager.sceneCount;
            for(int i=0; i<sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                if (!scene.isLoaded) continue;
                var go = ftLightmaps.FindInScene("!ftraceLightmaps", scene);
                if (go == null) continue;
                Undo.DestroyObjectImmediate(go);
            }
            LightmapSettings.lightmaps = new LightmapData[0];
        }
    }
}

