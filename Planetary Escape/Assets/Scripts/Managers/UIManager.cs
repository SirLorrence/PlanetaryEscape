using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Screen GameObjects")] public GameObject mainMenuGO;
    public GameObject settingsGO;
    public GameObject upgradeTerminalGO;
    public GameObject resultsScreenGO;
    public GameObject hudGO;
    public GameObject pauseMenuGO;

    [Header("Text Objects")] public Text healthText;
    public Text currentLevelText;
    public Text shieldText;
    public Text enemyRemainingText;
    public Text scoreText;
    public Text resultsScoreText;
    public Text resultsEnemiesKilledText;
    public Text resultsLevelsClearedText;
    public Text upgradePointsText;

    [Header("Level Loader")] public LevelLoader levelLoader;

    //[Header("Visuals")] public GameObject crosshairTexture;
    //Current UI element reference
    private GameObject currentScreenGO;

    void Start()
    {
        currentScreenGO = hudGO;
        GameManager.Instance.uiManager = this;
        ResetAllHUDUI();
        ShowHUD();
    }

    public void LoadMenu()
    {
        levelLoader.LoadLevel(0);
    }

    public void LoadGame()
    {
        GameManager.Instance.ResetLevel();
        GameManager.Instance.currentLevel = 1;
        levelLoader.LoadLevel(1);
    }

    public void LoadLevel(int index)
    {
        GameManager.Instance.currentLevel = index;
        levelLoader.LoadLevel(index);
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

    public void ResetAllHUDUI()
    {
        UpdateHealthText();
        UpdateShieldText();
        UpdateScoreText();
        UpdateCurrentLevelText();
        UpdateEnemyRemainingText();
    }

    public void UpdateResultsUI()
    {
        resultsLevelsClearedText.text = GameManager.Instance.currentLevel.ToString();
        resultsScoreText.text = GameManager.Instance.score.ToString();
        resultsEnemiesKilledText.text = GameManager.Instance.enemiesKilled.ToString();
    }

    #endregion

    #region UpdateIndividualUI

    public void UpdateHealthText()
    {
        healthText.text = GameManager.Instance.PlayerHealth.ToString();
    }

    public void UpdateShieldText()
    {
        shieldText.text = GameManager.Instance.PlayerShield.ToString();
    }

    public void UpdateScoreText()
    {
        scoreText.text = "Upgrade Points: "+ GameManager.Instance.upgradePoints;
    }

    public void UpdateCurrentLevelText()
    {
        //currentLevelText.text = GameManager.Instance.currentLevel.ToString();
    }

    public void UpdateEnemyRemainingText()
    {
        //enemyRemainingText.text = "Enemies: " +  GameManager.Instance.enemiesRemaining;
    }

    public void UpdateUpgradeCostText()
    {
        upgradePointsText.text = GameManager.Instance.upgradePoints.ToString();
    }

    #endregion

    #region ShowScreens

    public void ShowMainMenu()
    {
        currentScreenGO.SetActive(false);
        currentScreenGO = mainMenuGO;
        currentScreenGO.SetActive(true);
        Cursor.visible = true;
    }

    public void ShowSettings()
    {
        currentScreenGO.SetActive(false);
        currentScreenGO = settingsGO;
        currentScreenGO.SetActive(true);
        Cursor.visible = true;
    }

    public void ShowPause()
    {
        currentScreenGO.SetActive(false);
        currentScreenGO = pauseMenuGO;
        currentScreenGO.SetActive(true);
        Cursor.visible = true;
    }

    public void ShowHUD()
    {
        currentScreenGO.SetActive(false);
        currentScreenGO = hudGO;
        currentScreenGO.SetActive(true);
        Cursor.visible = false;
    }

    public void ShowUpgrades()
    {
        currentScreenGO.SetActive(false);
        currentScreenGO = upgradeTerminalGO;
        currentScreenGO.SetActive(true);
        UpdateUpgradeCostText();
        Cursor.visible = true;
    }

    public void ShowResults()
    {
        currentScreenGO.SetActive(false);
        currentScreenGO = resultsScreenGO;
        currentScreenGO.SetActive(true);
        UpdateResultsUI();
        Cursor.visible = true;
    }

    #endregion
}