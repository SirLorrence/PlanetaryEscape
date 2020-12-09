using System;
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

    [Serializable]
    public enum Screens
    {
        Menu = 0, Settings = 1, Pause = 2, Hud = 3, Results = 4, Upgrade = 5
    }

    //Current UI element reference
    private GameObject currentScreenGO;

    void Start()
    {
        currentScreenGO = hudGO;
        GameManager.Instance.uiManager = this;
        ResetAllHUDUI();
        ChangeScreen((int)Screens.Hud);
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

    public void ChangeScreen(int screen)
    {
        currentScreenGO.SetActive(false);
        
        switch (screen)
        {
            case (int)Screens.Menu: currentScreenGO = mainMenuGO; Cursor.visible = true; break;
            case (int)Screens.Settings: currentScreenGO = settingsGO; Cursor.visible = true; break;
            case (int)Screens.Pause: currentScreenGO = pauseMenuGO; Cursor.visible = true; currentScreenGO.SetActive(true); InputManager.Instance.Freeze(); break;
            case (int)Screens.Hud: currentScreenGO = hudGO; Cursor.visible = false; InputManager.Instance.Unfreeze(); break;
            case (int)Screens.Results: currentScreenGO = resultsScreenGO; Cursor.visible = true; UpdateResultsUI(); currentScreenGO.SetActive(true); InputManager.Instance.Freeze(); break;
            case (int)Screens.Upgrade: currentScreenGO = upgradeTerminalGO; Cursor.visible = true; UpdateUpgradeCostText(); break;
        }

        currentScreenGO.SetActive(true);
    }
    #endregion
}