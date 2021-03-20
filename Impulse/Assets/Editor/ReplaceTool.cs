using UnityEditor;
using UnityEngine;
	public class ReplaceTool : EditorWindow
	{
		[SerializeField] private GameObject prefab;
		private GameObject[] objectsSelected;
		private GameObject newGameObject;

		
		//opens the editor window
		[MenuItem("Tools/Replace Prefab")]
		static void CreateReplaceWindow() => GetWindow<ReplaceTool>(); 

		//draws the elements in the window
		private void OnGUI() {
			//prefab slot
			prefab = (GameObject) EditorGUILayout.ObjectField("Replace with: ", prefab, typeof(GameObject), false);
			
			if (GUILayout.Button("Replace")) {
				objectsSelected = Selection.gameObjects; //objects that are selected in the hierarchy 
				foreach (var gameObject in objectsSelected) {
					var prefabType = PrefabUtility.GetPrefabAssetType(prefab); 
					//checks what type of prefab is being change... documentation on it explains the difference 
					if (prefabType == PrefabAssetType.Regular)  {
						newGameObject = (GameObject) PrefabUtility.InstantiatePrefab(prefab); 
					}
					else {
						newGameObject = Instantiate(prefab);
						newGameObject.name = prefab.name;
					}

					//if something breaks
					if (newGameObject == null) {
						Debug.LogError("Not working");
						break;
					}

					Undo.RegisterCreatedObjectUndo(newGameObject, "Replace with Prefabs");
					
					//sets the object to the same units that it's being replaced with
					newGameObject.transform.parent = gameObject.transform.parent;
					newGameObject.transform.localPosition = gameObject.transform.localPosition;
					newGameObject.transform.localRotation = gameObject.transform.localRotation;
					newGameObject.transform.localScale = gameObject.transform.localScale;
					
					//replaces it same order of the hierarchy... pretty cool
					newGameObject.transform.SetSiblingIndex(gameObject.transform.GetSiblingIndex()); 
					Undo.DestroyObjectImmediate(gameObject);
				}
			}

			GUI.enabled = false;
			EditorGUILayout.LabelField("Selected: " + Selection.objects.Length);
		}
	}
