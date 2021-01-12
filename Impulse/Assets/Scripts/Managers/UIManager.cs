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

    [Header("Text Objects")]
    public Text hudInteractText;
    [Serializable]
    public enum Screens
    {
        Menu = 0, Settings = 1, Pause = 2, Hud = 3, Results = 4, Upgrade = 5
    }

    //Current UI element reference
    private GameObject currentScreenGO;

    #region Singleton

    //Singleton Instantiation
    public static UIManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this.gameObject);
        else
            Instance = this;

        DontDestroyOnLoad(this);
    }
    #endregion

    void Start()
    {
        currentScreenGO = hudGO;
        ChangeScreen((int)Screens.Hud);
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

    public void ResetGame() => GameManager.Instance.ResetGame();

    #region ShowScreens

    public void ChangeScreen(int screen)
    {
        currentScreenGO.SetActive(false);
        
        switch (screen)
        {
            case (int)Screens.Menu: currentScreenGO = mainMenuGO; Cursor.visible = true; break;
            case (int)Screens.Settings: currentScreenGO = settingsGO; Cursor.visible = true; break;
            case (int)Screens.Pause: currentScreenGO = pauseMenuGO; Cursor.visible = true; currentScreenGO.SetActive(true); break;
            case (int)Screens.Hud: currentScreenGO = hudGO; Cursor.visible = false;  break;
            case (int)Screens.Results: currentScreenGO = resultsScreenGO; Cursor.visible = true; currentScreenGO.SetActive(true);  break;
            case (int)Screens.Upgrade: currentScreenGO = upgradeTerminalGO; Cursor.visible = true; break;
        }

        currentScreenGO.SetActive(true);
    }
    #endregion

    #region Update Text

    public void UpdateHudInteract(bool active)
    {
        hudInteractText.gameObject.SetActive(active);
    }
    #endregion
}