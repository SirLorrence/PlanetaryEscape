using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject Settings;
    public GameObject Terminal;
    public GameObject HUD;
    public LevelLoader levelLoader;
    private int cost = 1;

    public void LoadMenu()
    {
        Settings.SetActive(false);
        MainMenu.SetActive(true);
    }

    public void LoadSettings()
    {
        MainMenu.SetActive(false);
        Settings.SetActive(true);
    }

    public void Play()
    {
        GameManager.Instance.ShowHUD();
    }

    public void LoadGame()
    {
        MainMenu.SetActive(false);
        Settings.SetActive(false);
        levelLoader.LoadLevel(1);
    }
    public void Quit()
    {
        Application.Quit();
    }

    public void UpgradeHealth()
    {
        //Debug.Log("Health Before:  " + GameManager.Instance.baseHealth);
        //if (GameManager.Instance.score > cost)
        //{
        //    GameManager.Instance.baseHealth += 1;
        //    GameManager.Instance.score -= cost;
        //    Debug.Log("Health Updated:  " + GameManager.Instance.baseHealth);
        //}
    }

    public void ResetGame() => GameManager.Instance.ResetLevel();
}
