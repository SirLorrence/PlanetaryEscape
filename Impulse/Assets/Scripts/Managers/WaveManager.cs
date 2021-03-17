using System.Collections;
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
            EndWave
        }
        #endregion

        private const int _TimeBetweenWaves = 5;
    
    
    
    
        private WaveStates _waveState = WaveStates.StartWave;
        private readonly int[] _constZombieWaves = {8, 12, 18, 24, 30};
        private int _zombiesLeftToSpawn = 0;
        private int _maxActiveZombies = 0;
        private int _currentWave = 0;
        #endregion

        #region Mutators
        public int CurrentWave
        {
            get { return _currentWave; }
        }
        #endregion

        #region Update
        private void Update()
        {
            switch (_waveState)
            {
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
            }
        }
        #endregion

        #region Wave Calculation
        private bool AreZombiesLeftToBeSpawned() => _zombiesLeftToSpawn > 0;
        private void InitializeNextWave()
        {
            _currentWave++;
            if (_currentWave <= _constZombieWaves.Length)
            {
                _zombiesLeftToSpawn = _constZombieWaves[CurrentWave - 1];
                _maxActiveZombies = 24;
            }
            else
            {
                _zombiesLeftToSpawn = (int)(CurrentWave * 0.25) * 24;
                _maxActiveZombies = 14 + CurrentWave * 2;
            }
            ChangeState(newState: WaveStates.Spawning);
        }
        private void EndLastWave()
        {
            ChangeState(newState: WaveStates.Transition);
        }
        #endregion

        #region Wave States
        private void ChangeState(WaveStates newState)
        {
            _waveState = newState;
        }
        private void Delay() => StartCoroutine(DelayTransition());

        private IEnumerator DelayTransition()
        {
            yield return new WaitForSeconds(_TimeBetweenWaves);
            ChangeState(WaveStates.StartWave);
            yield break;
        }
        #endregion

        #region Zombie Spawning
        private void SpawnZombies()
        {
            if (!AreZombiesLeftToBeSpawned()) 
            {
                ChangeState(WaveStates.Waiting);
                return;
            }

            //--------------------------------------------
            //Spawning Zombies happens here
            //--------------------------------------------
        }

        //----------------------------------------------------------------------
        private void WaitingForEndOfWave()
        {
            if (_zombiesLeftToSpawn == 0) //Active Zombies in Game, NOT LEFT TO BE SPAWNED
                ChangeState(WaveStates.Transition);
        }
        //----------------------------------------------------------------------
        #endregion
    }
}
