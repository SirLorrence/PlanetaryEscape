using System.Collections.Generic;
using Mirror;
using UnityEngine;

namespace Entities.Player
{
	// Enum for possible game states on the client
	enum EClientGameState
	{
		k_EClientGameAlive,
		k_EClientGameDown,
		k_EClientGameOver,
	};


	public class PlayerObjectInformation : NetworkBehaviour
	{
		public GameObject localPlayer;
		public GameObject networkedPlayer;

		private GameObject[] playerObjects = new GameObject[0];
		private int spectatorIndex = 0;
		//public SteamStatsAndAchievements m_StatsAndAchievements;

		public override void OnStartAuthority()
		{
			localPlayer.SetActive(true);
			networkedPlayer.SetActive(false);

			base.OnStartAuthority();
		}

		private void OnEnable()
		{
			//if (m_StatsAndAchievements != null)
			//{
			//m_StatsAndAchievements = FindObjectOfType<SteamStatsAndAchievements>();
			//}

			//m_StatsAndAchievements.OnGameStateChange(EClientGameState.k_EClientGameAlive);
		}

		public void BeginSpectateNext()
		{
			//Ensures we have the reference to every player
			if (NetworkServer.connections.Count != playerObjects.Length) playerObjects = GameObject.FindGameObjectsWithTag("Player");

			for (int i = 0; i < playerObjects.Length; i++)
			{
				//Increments the Spectator index
				if (spectatorIndex < playerObjects.Length - 1) spectatorIndex++;
				else spectatorIndex = 0;

				//Checks if the 3rd person body on the selected player is active
				if (playerObjects[spectatorIndex].transform.GetChild(1))
				{
					//Disables the Body and enables the FPS Camera
					playerObjects[spectatorIndex].transform.GetChild(1).gameObject.SetActive(false);
					playerObjects[spectatorIndex].transform.GetChild(0).gameObject.SetActive(true);
				}
			}
			Debug.Log("No Players availible to be spectated");
		}
		


		public void BeginSpectatePrevious()
		{
			//Ensures we have the reference to every player
			if (NetworkServer.connections.Count != playerObjects.Length) playerObjects = GameObject.FindGameObjectsWithTag("Player");

			for (int i = 0; i < playerObjects.Length; i++)
			{
				//Increments the Spectator index
				if (spectatorIndex > 1) spectatorIndex--;
				else spectatorIndex = playerObjects.Length - 1;

				//Checks if the 3rd person body on the selected player is active
				if (playerObjects[spectatorIndex].transform.GetChild(1))
				{
					//Disables the Body and enables the FPS Camera
					playerObjects[spectatorIndex].transform.GetChild(1).gameObject.SetActive(false);
					playerObjects[spectatorIndex].transform.GetChild(0).gameObject.SetActive(true);
					break;
				}
			}

			Debug.Log("No Players availible to be spectated");
		}

		//-----------------------------------------------------------------------------
		// Purpose: Testing
		//-----------------------------------------------------------------------------
		//private void OnGUI()
		//{
		//	m_StatsAndAchievements.Render();
		//	GUILayout.Space(10);

		//	if (GUILayout.Button("Set State to Active"))
		//	{
		//		m_StatsAndAchievements.OnGameStateChange(EClientGameState.k_EClientGameActive);
		//	}
		//	if (GUILayout.Button("Set State to Winner"))
		//	{
		//		m_StatsAndAchievements.OnGameStateChange(EClientGameState.k_EClientGameWinner);
		//	}
		//	if (GUILayout.Button("Set State to Loser"))
		//	{
		//		m_StatsAndAchievements.OnGameStateChange(EClientGameState.k_EClientGameLoser);
		//	}
		//	if (GUILayout.Button("Add Distance Traveled +100"))
		//	{
		//		m_StatsAndAchievements.AddDistanceTraveled(100.0f);
		//	}
		//}
	}
}