using UnityEngine;
using System.Collections;
using Mirror;

// Enum for possible game states on the client
enum EClientGameState
{
	k_EClientGameActive,
	k_EClientGameWinner,
	k_EClientGameLoser,
};


public class Player : NetworkBehaviour
{
	public GameObject localPlayer;
	public GameObject networkedPlayer;

	// Start is called before the first frame update
	void Start() {
		if (hasAuthority)
			localPlayer.SetActive(true);
		else
			networkedPlayer.SetActive(true);
	}
	SteamStatsAndAchievements m_StatsAndAchievements;

	private void OnEnable()
	{
		m_StatsAndAchievements = FindObjectOfType<SteamStatsAndAchievements>();

		m_StatsAndAchievements.OnGameStateChange(EClientGameState.k_EClientGameActive);
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
