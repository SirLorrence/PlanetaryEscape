using UnityEngine;
using Random = UnityEngine.Random;


namespace Managers
{
	public class DropManager : MonoBehaviour
	{
		#region Singleton

		private static DropManager instance;

		public static DropManager dropManager {
			get {
				if (instance == null) {
					instance = FindObjectOfType<DropManager>();
					if (instance == null) {
						GameObject manager = new GameObject("Drop Manager");
						instance = manager.AddComponent<DropManager>();
					}
				}

				return instance;
			}
		}

		private void Awake() {
			if (instance != null) Destroy(this);
			DontDestroyOnLoad(this);
		}

		#endregion

		public GameObject[] items;

		public GameObject GetRandomItem() {
			int randomValue = Random.Range(0, items.Length + 1);
			return items[randomValue];
		}
	}
}