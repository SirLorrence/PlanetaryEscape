using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region Variables

    [Header("Prefabs")]
    public GameObject playerBullet;
    public GameObject enemyBullet;
    public GameObject healthPickup;
    public GameObject skillPickup;
    public SelectEnemy typeOfEnemies;

    [Header("Number of Pooled Objects")]
    public int NumberOfPlayerBullets;
    public int NumberOfEnemyBullets;
    public int NumberOfEnemies;

    [Header("Game Information")]
    public int score = 0;
    public float upgradePoints;
    
    //List of the objects that have been pooled
    List<GameObject> PlayerBulletPool = new List<GameObject>();
    List<GameObject> EnemyBulletPool = new List<GameObject>();
    List<GameObject> EnemyPool = new List<GameObject>();
    List<GameObject> HealthPickupPool = new List<GameObject>();
    List<GameObject> SkillPickupPool = new List<GameObject>();

    //Enemy
    [HideInInspector]
    public enum EnemyTypes
    {
        Weak, 
        Medium,
        Strong
    }

    //Player
    private GameObject player;
    private PlayerStats playerStats;

    //showing remaining enemies
    [HideInInspector] public int enemiesRemaining = 0;
    [HideInInspector] public int enemiesKilled = 0;

    //UI
    //public GameObject upgradeScreen;
    //public GameObject hud;
    //public GameObject pauseMenu;
    //public GameObject resultsScreen;
    //ResultScreen results;

    //public Text healthText;
    //public Text waveText;
    //public Text ammoText;
    //public Text enemyRemainText;
    //public Text scoreText;



    #endregion

    #region Singleton

    //Singleton Instantiation
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this.gameObject);
        else
            Instance = this;

        DontDestroyOnLoad(this);

        player = GameObject.FindWithTag("Player");
        playerStats = player.GetComponent<PlayerStats>();
    }

    #endregion

    void Start()
    {
        //scoreText.text = score.ToString();
        //waveText.text = waveNumber.ToString();
        //enemyRemainText.text = enemiesRemaining.ToString();
        //results = resultsScreen.GetComponent<ResultScreen>();
    }

    public GameObject GetEnemy(EnemyTypes type)
    {
        if (EnemyPool.Count == 0) //If list empty, fill
        {
            StartCoroutine(InitEnemies());
        }

        foreach (var Enemy in EnemyPool)
        {
            if (Enemy.activeSelf == false && Enemy.GetComponent<EnemyController>().enemyType == type)
            {
                return Enemy;
            }
        }

        return null;
    }

    IEnumerator InitEnemies()
    {
        for (int enemyType = 0; enemyType <= 2; enemyType++)
        {
            for (int i = 0; i < NumberOfEnemies; i++)
            {
                GameObject go = Instantiate(typeOfEnemies.SetEnemyType(enemyType), new Vector3(-100, 12, 70),
                    Quaternion.identity) as GameObject;
                go.SetActive(false);
                go.transform.SetParent(gameObject.transform);
                EnemyPool.Add(go);
                yield return new WaitForSecondsRealtime(0.01f);
            }
        }

        yield return null;
    }

    public void SpawnItem(GameObject go)
    {
        //Gets a random number between  0 & 1 
        float rand = UnityEngine.Random.value;
        //Based on the number and drop a pickup if condition is met
        if (rand <= 0.1f)
        {
            GameObject temp = GetHealthPickup();
            temp.transform.position = go.transform.position;
            temp.SetActive(true);
        }
        else if (rand <= 0.2f && rand > 0.1f)
        {
            GameObject temp = GetSkillPickup();
            temp.transform.position = go.transform.position;
            temp.SetActive(true);
        }
    }
    public void SetEnemy(GameObject go) // removes enemies
    {
        go.SetActive(false);
        IncreaseScore(10);
        enemiesRemaining--;
        enemiesKilled++;

        //GameManager.Instance.enemyRemainText.text = GameManager.Instance.enemiesRemaining.ToString();

        go.transform.position = Vector3.zero;
    }

    public GameObject GetPlayerBullet()
    {
        if (PlayerBulletPool.Count == 0) //If list empty, fill
        {
            StartCoroutine(InitBullets());
        }

        foreach (var bullet in PlayerBulletPool)
        {
            if (!bullet.activeSelf)
            {
                return bullet;
            }
        }

        return null;
    }

    IEnumerator InitBullets()
    { 
        for (int i = 0; i < NumberOfPlayerBullets; i++)
        {
            GameObject go = Instantiate(playerBullet, Vector3.zero, Quaternion.identity) as GameObject;
            go.SetActive(false);
            go.transform.SetParent(gameObject.transform);
            PlayerBulletPool.Add(go);
            yield return 0;
        }
        yield return null;
    }

    public void SetObjectInPool(GameObject go)
    {
        go.SetActive(false);
        go.transform.position = Vector3.zero;
    }

    public GameObject GetEnemyBullet()
    {
        if (EnemyBulletPool.Count == 0) //If list empty, fill
        {
            StartCoroutine(InitEnemyBullets());
        }

        foreach (var bullet in EnemyBulletPool)
        {
            if (!bullet.activeSelf)
            {
                return bullet;
            }
        }

        return null;
    }

    IEnumerator InitEnemyBullets()
    { 
        for (int i = 0; i < NumberOfEnemyBullets; i++)
        {
            GameObject go = Instantiate(enemyBullet, Vector3.zero, Quaternion.identity) as GameObject;
            go.SetActive(false);
            go.transform.SetParent(gameObject.transform);
            EnemyBulletPool.Add(go);
            yield return 0;
        }
        yield return null;
    }

    public void IncreaseScore(int addedScore)
    {
        score += addedScore;
        Debug.Log("Score:  " + score);

        // updating score
        //scoreText.text = score.ToString();

    }

    public void PlayerDead() => StartCoroutine(Death());

    public void TakeDamage(GameObject hitObject, int damageAmount) => hitObject.GetComponent<Healable>().Health -= damageAmount;

    IEnumerator Death()
    {
        GameObject go = GameObject.FindGameObjectWithTag("Player");
        go.GetComponentInChildren<Animator>().SetBool("isDead", true);
        yield return new WaitForSeconds(2);
        ShowResults();
        Time.timeScale = 0;
        yield return null;
    }

    // methods to show UI seperately
    #region ShowUI
    public void ShowPause()
    {
        //pauseMenu.SetActive(true);
        //resultsScreen.SetActive(false);
        //hud.SetActive(false);
        //upgradeScreen.SetActive(false);
    }

    public void ShowHUD()
    {
        //pauseMenu.SetActive(false);
        //resultsScreen.SetActive(false);
        //hud.SetActive(true);
        //upgradeScreen.SetActive(false);
    }

    public void ShowUpgrades()
    {
        //pauseMenu.SetActive(false);
        //resultsScreen.SetActive(false);
        //hud.SetActive(false);
        //upgradeScreen.SetActive(true);
    }

    public void ShowResults()
    {
        //pauseMenu.SetActive(false);
        //resultsScreen.SetActive(true);
        //hud.SetActive(false);
        //upgradeScreen.SetActive(false);
    }
    #endregion

    #region Pick Ups
   public void SpawnSatellite()
    {
    }

    public void SpawnPower()
    {
    }

    public GameObject GetHealthPickup()
    {
        if (HealthPickupPool.Count == 0) //If list empty, fill
        {
            for (int i = 0; i < 10; i++)
            {
                GameObject go = Instantiate(healthPickup, Vector3.zero, Quaternion.identity) as GameObject;
                go.transform.SetParent(gameObject.transform);
                go.SetActive(false);
                HealthPickupPool.Add(go);
            }
        }

        foreach (var pickup in HealthPickupPool)
        {
            if (!pickup.activeSelf)
            {
                return pickup;
            }
        }

        return null;
    }

    public GameObject GetSkillPickup()
    {
        if (SkillPickupPool.Count == 0) //If list empty, fill
        {
            for (int i = 0; i < 10; i++)
            {
                GameObject go = Instantiate(skillPickup, Vector3.zero, Quaternion.identity) as GameObject;
                go.SetActive(false);
                go.transform.SetParent(gameObject.transform);
                SkillPickupPool.Add(go);
            }
        }

        foreach (var pickup in SkillPickupPool)
        {
            if (!pickup.activeSelf)
            {
                return pickup;
            }
        }

        return null;
    }

    #endregion

    public void Heal(int amount) => playerStats.Health += amount;

    public void ResetLevel()
    {
        print("Reset");
        playerStats.GetComponentInChildren<Animator>().SetBool("isDead", false);
        playerStats.OnReset();
        player.GetComponent<PlayerMovement>().ResetPosition();
        ResetPools();

        ShowHUD();
        Time.timeScale = 1;
    }

    void ResetPools()
    {
        foreach (var bullet in PlayerBulletPool)
        {
            if (bullet.activeSelf)
            {
                SetObjectInPool(bullet);
            }
        }

        foreach (var enemyBullet in EnemyBulletPool)
        {
            if (enemyBullet.activeSelf)
            {
                SetObjectInPool(enemyBullet);
            }
        }

        foreach (var enemy in EnemyPool)
        {
            if (enemy.activeSelf)
            {
                SetObjectInPool(enemy);
            }
        }

        foreach (var pickup in HealthPickupPool)
        {
            if (pickup.activeSelf)
            {
                SetObjectInPool(pickup);
            }
        }

        foreach (var pickup in SkillPickupPool)
        {
            if (pickup.activeSelf)
            {
                SetObjectInPool(pickup);
            }
        }
    }
}