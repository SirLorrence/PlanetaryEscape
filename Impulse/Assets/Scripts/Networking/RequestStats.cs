using TMPro;
using UnityEngine;

namespace Networking
{
    public class RequestStats : MonoBehaviour
    {
        [SerializeField] private TMP_Text totalKills;
        [SerializeField] private TMP_Text maxKills;
        [SerializeField] private TMP_Text timePlayed;
        [SerializeField] private TMP_Text totalGamesPlayed;


        [SerializeField] private SteamStatsAndAchievements steamStatsAndAchievements;

        private void OnEnable()
        {
            RequestTotalKillsData();
            RequestMaxKillsData();
            RequestTimePlayedData();
            RequestGamesPlayedData();
        }

        public void RequestTotalKillsData()
        {
            totalKills.text = "";
            if (steamStatsAndAchievements != null)
            {
                if (steamStatsAndAchievements.AreStatsValid)
                {
                    totalKills.text = steamStatsAndAchievements.TotalKills + " Total Kills";
                }
            }
        
        }
        public void RequestMaxKillsData()
        {
            totalKills.text = "";
            if (steamStatsAndAchievements != null)
            {
                if (steamStatsAndAchievements.AreStatsValid)
                {
                    maxKills.text = "Max Kills " + steamStatsAndAchievements.MaxKills;
                }
            }
        }
        public void RequestTimePlayedData()
        {
            totalKills.text = "";
            if (steamStatsAndAchievements != null)
            {
                if (steamStatsAndAchievements.AreStatsValid)
                {
                    timePlayed.text = steamStatsAndAchievements.TotalTimeSurvived + " seconds Played";
                }
            }
        }
        public void RequestGamesPlayedData()
        {
            totalKills.text = "";
            if (steamStatsAndAchievements != null)
            {
                if (steamStatsAndAchievements.AreStatsValid)
                {
                    totalGamesPlayed.text = steamStatsAndAchievements.GamesPlayed + " Games Played";
                }
            }
        }
    }
}
