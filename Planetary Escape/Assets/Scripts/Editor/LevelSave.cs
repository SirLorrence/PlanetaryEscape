using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameManager))]
public class LevelSave : Editor
{
    public override void OnInspectorGUI()
    {
        GameManager mapPreview = (GameManager)target;
        //if (DrawDefaultInspector())
        //    GameManager.Instance.SaveLevel();

        //if (GUILayout.Button("Save Current Level"))
        //    GameManager.Instance.SaveLevel();
    }
}