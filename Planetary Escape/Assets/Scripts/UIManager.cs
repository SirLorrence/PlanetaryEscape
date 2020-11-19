using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Screen GameObjects")]
    public GameObject mainMenuGO;
    public GameObject settingsGO;
    public GameObject upgradeTerminalGO;
    public GameObject resultsScreenGO;
    public GameObject hudGO;
    public GameObject pauseMenuGO;

    [Header("Text Objects")]
    public Text healthText;
    public Text currentLevelText;
    public Text shieldText;
    public Text enemyRemainingText;
    public Text scoreText;

    [Header("Level Loader")]
    public LevelLoader levelLoader;

    private int cost = 1;
    private GameObject currentScreenGO;

    void Start()
    {
        GameManager.Instance.uiManager = this;
    }

    public void LoadGame()
    {
        levelLoader.LoadLevel(1);
    }

    public void Quit()
    {
        #if UNITY_STANDALONE
        Application.Quit();
        #endif
        
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    public void ResetGame() => GameManager.Instance.ResetLevel();

    #region UpdateUIInGroups

    public void ResetAllUI()
    {
        UpdateHealthText();
        UpdateShieldText();
        UpdateScoreText();
        UpdateCurrentLevelText();
        UpdateEnemyRemainingText();
    }

    #endregion

    #region UpdateIndividualUI
    public void UpdateHealthText()
    {
        healthText.text = GameManager.Instance.playerStats.Health.ToString();
    }

    public void UpdateShieldText()
    {
        shieldText.text = GameManager.Instance.playerStats.Shield.ToString();
    }

    public void UpdateScoreText()
    {
        scoreText.text = GameManager.Instance.score.ToString();
    }

    public void UpdateCurrentLevelText()
    {
        currentLevelText.text = GameManager.Instance.currentLevel.ToString();
    }

    public void UpdateEnemyRemainingText()
    {
        enemyRemainingText.text = GameManager.Instance.enemiesRemaining.ToString();
    }

    #endregion

    #region ShowScreens

    public void ShowMainMenu()
    {
        currentScreenGO.SetActive(false);
        currentScreenGO = mainMenuGO;
        currentScreenGO.SetActive(true);
    }
    public void ShowSettings()
    {
        currentScreenGO.SetActive(false);
        currentScreenGO = settingsGO;
        currentScreenGO.SetActive(true);
    }

    public void ShowPause()
    {
        currentScreenGO.SetActive(false);
        currentScreenGO = pauseMenuGO;
        currentScreenGO.SetActive(true);
    }

    public void ShowHUD()
    {
        currentScreenGO.SetActive(false);
        currentScreenGO = hudGO;
        currentScreenGO.SetActive(true);
    }

    public void ShowUpgrades()
    {
        currentScreenGO.SetActive(false);
        currentScreenGO = upgradeTerminalGO;
        currentScreenGO.SetActive(true);
    }

    public void ShowResults()
    {
        currentScreenGO.SetActive(false);
        currentScreenGO = resultsScreenGO;
        currentScreenGO.SetActive(true);
    }
    #endregion
}
