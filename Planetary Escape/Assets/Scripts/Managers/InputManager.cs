using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{
    [Header("Required Scripts")]
    public PlayerController playerController;
    public PlayerShoot playerShooting;
    public PlayerStats playerStats;
    public CameraFollow cameraFollow;

    //private GameObject pauseMenu;
    private bool pause;
    private bool freeze = false;
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
        if (cameraFollow == null) cameraFollow = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>();
        cameraFollow.CameraMove(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
    }
    
    void FixedUpdate()
    {
        if (playerController == null) playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        if (playerShooting == null) playerShooting = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerShoot>();
        if (playerStats == null) playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        
        if (Time.timeScale == 0 || freeze)
            return;

        if (Input.GetMouseButton(0))
        {
            if (time + (1 / playerStats.FireRate) < Time.time)
            {
                playerShooting.Shoot();
                time = Time.time;
            }
        }
        playerController.Move(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }

    public void Unfreeze() => freeze = false;

    public void Freeze() => freeze = true;
}
