using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


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
		[SerializeField] private GameObject loadingScreen;
		[SerializeField] private GameObject pauseMenu;
		[SerializeField] private GameObject optionsMenu;
		[SerializeField] private GameObject resultsMenu;
		[SerializeField] private GameObject creditsMenu;


		private List<AsyncOperation> sceneLoading = new List<AsyncOperation>();

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
			else _instance = this;
			SceneManager.LoadSceneAsync((int) SceneIndex.MAIN_MENU, LoadSceneMode.Additive);
		}

		#endregion

		#region Mutators

		#endregion

		#region Level Management

		public void LoadMenu() {
			TogglePauseMenu(false);
			loadingScreen.gameObject.SetActive(true);
			sceneLoading.Add(SceneManager.UnloadSceneAsync((int) SceneIndex.GAME));
			sceneLoading.Add(SceneManager.LoadSceneAsync((int) SceneIndex.MAIN_MENU, LoadSceneMode.Additive));
			StartCoroutine(GetLoadProgress());
		}

		public void LoadGame() {
			TogglePauseMenu(false);
			loadingScreen.gameObject.SetActive(true);
			sceneLoading.Add(SceneManager.UnloadSceneAsync((int) SceneIndex.MAIN_MENU));
			sceneLoading.Add(SceneManager.LoadSceneAsync((int) SceneIndex.GAME, LoadSceneMode.Additive));
			StartCoroutine(GetLoadProgress());
		}

		public void ResetGame() {
			TogglePauseMenu(false);
			loadingScreen.gameObject.SetActive(true);
			sceneLoading.Add(SceneManager.UnloadSceneAsync((int) SceneIndex.GAME));
			sceneLoading.Add(SceneManager.LoadSceneAsync((int) SceneIndex.GAME, LoadSceneMode.Additive));
			StartCoroutine(GetLoadProgress());
		}

		private IEnumerator GetLoadProgress() {
			for (int i = 0; i < sceneLoading.Count; i++) {
				while (!sceneLoading[i].isDone) {
					yield return null;
				}
			}

			while (!ObjectPooler.Instance.isInitialized) {
				ObjectPooler.Instance.Initialize();
				yield return null;
			}

			loadingScreen.SetActive(false);
		}

		#endregion

		#region Unity Messages

		private void Update() {
			WaveManager.Instance.UpdateWaves();
		}

		private void Start() {
			WaveManager.Instance.EndOfWave += OnEndOfWave;
		}

		private void OnDisable() {
			WaveManager.Instance.EndOfWave -= OnEndOfWave;
		}

		#endregion

		#region Callbacks

		private void OnEndOfWave() {
			//SoundManager.Instance.PlayAudio(AudioTypes.CompletedWave);
			wavesSurvived++;
		}

		#endregion

		public void ZombieKilled() {
			zombiesKilled++;
			WaveManager.Instance.currentZombieCount--;
		}

		public void TogglePauseMenu(bool value) {
			pauseMenu.SetActive(value);
			Time.timeScale = (value) ? 0 : 1f;
		}

		public void ToggleOptions() => optionsMenu.SetActive(true);
		public void ToggleCredits() => creditsMenu.SetActive(true);


		public void ToggleBack() {
			optionsMenu.SetActive(false);
			creditsMenu.SetActive(false);
		}
	}

	public enum SceneIndex
	{
		MANAGER,
		MAIN_MENU,
		GAME,
	}
}