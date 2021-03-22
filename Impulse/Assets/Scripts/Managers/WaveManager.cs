using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

namespace Managers
{
	public class WaveManager : MonoBehaviour
	{
		#region Variables

		#region Enums

		public enum WaveStates
		{
			Spawning,
			Waiting,
			StartWave,
			Transition,
			EndWave,
			Pause
		}

		#endregion

		public bool debug = false;
		public Action EndOfWave;

		public int spawnPointRangeMin = 10;
		public int spawnPointRangeMax = 25;

		[HideInInspector] public List<GameObject> spawnPoints;
		private const int _TimeBetweenWaves = 5;

		[ReadOnly] [SerializeField] private WaveStates _waveState = WaveStates.StartWave;
		private float _timer = 0;

		private readonly int[] _constZombieWaves = {8, 12, 18, 24, 30};
		private int _zombiesLeftToSpawn = 0;
		private int _maxActiveZombies = 0;
		[ReadOnly] [SerializeField] private int _currentWave = 0;
		[ReadOnly] public int currentZombieCount = 0;

		#endregion

		#region Mutators

		public int CurrentWave {
			get { return _currentWave; }
		}

		#endregion

		#region Singleton

		public static WaveManager Instance { get; private set; }

		private void Awake() {
			if (Instance != null && Instance != this)
				Destroy(this.gameObject);
			else
				Instance = this;
		}

		#endregion

		#region Update

		public void UpdateWaves() {
			switch (_waveState) {
				case WaveStates.Spawning:
					SpawnZombies();
					break;
				case WaveStates.Waiting:
					WaitingForEndOfWave();
					break;
				case WaveStates.StartWave:
					InitializeNextWave();
					break;
				case WaveStates.Transition:
					Delay();
					break;
				case WaveStates.EndWave:
					EndLastWave();
					break;
				case WaveStates.Pause:
					break;
			}
		}

		public void Restart() {
			_currentWave = 0;
			ChangeState(newState: WaveStates.StartWave);
		}

		#endregion

		#region Wave Calculation

		private bool AreZombiesLeftToBeSpawned() => _zombiesLeftToSpawn > 0;
		private bool RoomForMoreZombies() => currentZombieCount         < _maxActiveZombies;

		private void InitializeNextWave() {
			StopCoroutine(DelayTransition());
			_currentWave++;
			if (_currentWave <= _constZombieWaves.Length) {
				_zombiesLeftToSpawn = _constZombieWaves[CurrentWave - 1];
				_maxActiveZombies = 24;
			}
			else {
				_zombiesLeftToSpawn = (int) (CurrentWave * 0.25) * 24;
				_maxActiveZombies = 14 + CurrentWave * 2;
			}

			ChangeState(newState: WaveStates.Spawning);
		}

		private void EndLastWave() {
			EndOfWave?.Invoke();
			ChangeState(newState: WaveStates.Transition);
		}

		#endregion

		#region Wave States

		private void ChangeState(WaveStates newState) {
			_waveState = newState;
		}

		private void Delay() {
			StartCoroutine(DelayTransition());
			ChangeState(newState: WaveStates.Pause);
		}

		private IEnumerator DelayTransition() {
			yield return new WaitForSeconds(_TimeBetweenWaves);
			ChangeState(WaveStates.StartWave);
		}

		#endregion

		#region Zombie Spawning

		private void SpawnZombies() {
			if (!AreZombiesLeftToBeSpawned()) {
				ChangeState(WaveStates.Waiting);
				return;
			}

			if (!RoomForMoreZombies()) return;

			if (spawnPoints.Count == 0) {
				LogWarning("No availible SpawnPoints");
				return;
			}

			if (_timer > Time.time) return;

			_timer = Time.time + 1;
			int randPos = UnityEngine.Random.Range(0, spawnPoints.Count - 1);

			_zombiesLeftToSpawn--;
			currentZombieCount++;

			GameObject go = ObjectPooler.Instance.GetGameObject(0);
			go.transform.position = spawnPoints[randPos].transform.position;
			go.SetActive(true);
		}

		private void WaitingForEndOfWave() {
			if (currentZombieCount == 0)
				ChangeState(WaveStates.EndWave);
		}

		public void AddSpawnPoint(GameObject spawnpoint) {
			spawnPoints.Add(spawnpoint);
			Log("Spawnpoint added");
		}

		public void RemoveSpawnPoint(GameObject spawnpoint) {
			spawnPoints.Remove(this.gameObject);
			Log("Spawnpoint removed");
		}

		#endregion

		#region Logging Functions

		private void Log(string msg) {
			if (!debug) return;

			Debug.Log("[WAVEMANAGER]: " + msg);
		}

		private void LogWarning(string msg) {
			Debug.LogWarning("[WAVEMANAGER]: " + msg);
		}

		#endregion
	}
}