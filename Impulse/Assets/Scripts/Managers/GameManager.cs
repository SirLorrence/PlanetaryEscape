using System;
using System.Security.Cryptography;
using UnityEngine;

namespace Managers
{
	[RequireComponent(typeof(DropManager), typeof(ObjectPooler))]
	[RequireComponent(typeof(SoundManager), typeof(WaveManager))]
	public class GameManager : MonoBehaviour
	{
		#region Singleton

		private static GameManager _instance;

		public static GameManager Instance {
			get {
				if (_instance == null) {
					{
						var manager = new GameObject("Game Manager");
						_instance = manager.AddComponent<GameManager>();
					}
				}

				return _instance;
			}
		}

		private void Awake() {
			if (_instance != null) Destroy(this);
			DontDestroyOnLoad(this);
		}

		#endregion
	}
}