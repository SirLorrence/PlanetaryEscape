using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    static bool pause;
    // Start is called before the first frame update
    void Start()
    {
        pause = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P)){
            if (pause)
            {
                ResumeGame();
            }

            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        //GameManager.Instance.ShowPause();
        Time.timeScale = 0f;
        pause = true;
    }

    public void ResumeGame()
    {
        //GameManager.Instance.ShowHUD();
        Time.timeScale = 1f;
        pause = false;
    }

    public void QuitGame()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1f;
    }
}
