using System;
using System.Security.Cryptography;
using UnityEngine;

namespace Managers
{
	[RequireComponent(typeof(DropManager), typeof(ObjectPooler))]
	[RequireComponent(typeof(SoundManager), typeof(WaveManager))]
	public class GameManager : MonoBehaviour
	{
		#region Variables
		public int zombiesKilled = 0;
		public float timeSurvived = 0;
		public int wavesSurvived = 0;
        #endregion

        #region Mutators
        #endregion

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

		#region Unity Messages
		private void Update()
		{
			WaveManager.Instance.UpdateWaves();
		}

		private void Start()
        {
			ObjectPooler.Instance.Initialize();
			WaveManager.Instance.EndOfWave += OnEndOfWave;

		}
		private void OnDisable()
        {
			WaveManager.Instance.EndOfWave -= OnEndOfWave;

		}

        #endregion

        #region Callbacks
        private void OnEndOfWave()
        {
			//SoundManager.Instance.PlayAudio(AudioTypes.CompletedWave);
			wavesSurvived++;
		}
		#endregion

		public void ZombieKilled()
        {
			zombiesKilled++;
			WaveManager.Instance.currentZombieCount--;

		}
	}
}