using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{
    [Header("Required Scripts")]
    public PlayerMovement playerMovement;
    public PlayerShoot playerShooting;
    public PlayerStats playerStats;

    //private GameObject pauseMenu;
    private bool pause;
    private float time = 0;
    #region Singleton
    //Singleton Instantiation
    public static InputManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this.gameObject);
        else
            Instance = this;

        DontDestroyOnLoad(this);

        print("Input Manager");
    }
    #endregion

    void Start()
    {
        //playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        //playerShooting = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerShoot>();

        //pauseMenu = GameManager.Instance.pauseMenu;
        pause = false;
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        //{
        //    if (pause)
        //    {
        //        Debug.Log("Hello");
        //        ResumeGame();
        //    }

        //    else
        //    {
        //        Debug.Log("goodbye");
        //        PauseGame();
        //    }
        //}
    }
    
    void FixedUpdate()
    {
        if (playerMovement == null) playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        if (playerShooting == null) playerShooting = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerShoot>();
        if (playerStats == null) playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();

        if (Time.timeScale == 0)
            return;

        if (Input.GetMouseButton(0))
        {
            if (time + (1 / playerStats.FireRate) < Time.time)
            {
                playerShooting.Shoot();
                time = Time.time;
            }
        }
        playerMovement.Move(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }
}
