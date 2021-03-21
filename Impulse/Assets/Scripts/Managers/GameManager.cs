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
			_instance = this;
			SceneManager.LoadSceneAsync((int) SceneIndex.MAIN_MENU, LoadSceneMode.Additive);
		}

		#endregion

		[SerializeField] private GameObject pauseMenu;
		[SerializeField] private GameObject resultsMenu;
		public GameObject loadingScreen;

		private List<AsyncOperation> sceneLoading = new List<AsyncOperation>();

		#region Level Management

		// public void LoadMenu() {
		// 	loadingScreen.gameObject.SetActive(true);
		// 	sceneLoading.Add(SceneManager.UnloadSceneAsync((int) SceneIndex.GAME));
		// 	sceneLoading.Add(SceneManager.LoadSceneAsync((int) SceneIndex.MAIN_MENU, LoadSceneMode.Additive));
		// 	StartCoroutine(GetLoadProgress());
		// }

		public void LoadGame() {
			loadingScreen.gameObject.SetActive(true);
			sceneLoading.Add(SceneManager.UnloadSceneAsync((int) SceneIndex.MAIN_MENU));
			sceneLoading.Add(SceneManager.LoadSceneAsync((int) SceneIndex.GAME, LoadSceneMode.Additive));
			StartCoroutine(GetLoadProgress());
		}
		private IEnumerator GetLoadProgress() {
			for (int i = 0; i < sceneLoading.Count; i++) {
				while (!sceneLoading[i].isDone) {
					yield return null;
				}
			}
			loadingScreen.SetActive(false);
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

	public enum SceneIndex
	{
		MANAGER,
		MAIN_MENU,
		GAME,
	}
	
}