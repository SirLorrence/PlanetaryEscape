using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevel;

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
    public float upgradePoints = 10;
    
    
    //Store upgrade level values 
    public int HEALTH_LEVEL;
    public int SHIELD_LEVEL;
    public int SPEED_LEVEL;
    public int GUN_LEVEL;
    

    [Header("Level Information")] 
    public GameObject[] levels;

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
    //Game Stats
    [HideInInspector] public int currentLevel = 0;
    [HideInInspector] public UIManager uiManager;

    //Player
    [HideInInspector] public GameObject player;
    [HideInInspector] public PlayerStats playerStats;
    [HideInInspector] public PlayerMovement playerMovement;

    //showing remaining enemies
    [HideInInspector] public int enemiesRemaining = 0;
    [HideInInspector] public int enemiesKilled = 0;
    #endregion

    public int PlayerHealth
    {
        get
        {
            if (player == null) player = GameObject.FindWithTag("Player");
            if (playerStats == null) playerStats = player.GetComponent<PlayerStats>();
            return playerStats.Health;
        }
    }

    public int PlayerShield
    {
        get
        {
            if (player == null) player = GameObject.FindWithTag("Player");
            if (playerStats == null) playerStats = player.GetComponent<PlayerStats>();
            return playerStats.Shield;
        }
    }

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
        for (int i = 0; i < levels.Length; i++)
        {
            if (i != currentLevel)
                levels[i].SetActive(false);
        }
    }
    public void SpawnItem(GameObject go)
    {
        GameObject temp = GetSkillPickup();
        temp.transform.position = go.transform.position + new Vector3(0,0.5f, 0);
        temp.SetActive(true);

        //Gets a random number between  0 & 1 
        float rand = UnityEngine.Random.value;
        //Based on the number and drop a pickup if condition is met
        if (rand <= 0.5f)
        {
            temp = GetHealthPickup();
            temp.transform.position = go.transform.position;
            temp.SetActive(true);
        }
        else if (rand <= 0.2f && rand > 0.1f)
        {
            temp = GetSkillPickup();
            temp.transform.position = go.transform.position;
            temp.SetActive(true);
        }
    }

    public void IncreaseScore(int addedScore)
    {
        score += addedScore;
        Debug.Log("Score: " + score);

        uiManager.UpdateScoreText();
    }
    public void IncreaseUpgradeScore(int addedPoints)
    {
        upgradePoints += addedPoints;
        Debug.Log("Upgrade Points: " + upgradePoints);
    }

    public void PlayerDead() => StartCoroutine(Death());

    public void TakeDamage(GameObject hitObject, int damageAmount)
    {
        var objectHealth = hitObject.GetComponent<Healable>();
        if (objectHealth.Shield > 0) objectHealth.Shield -= damageAmount;
        else objectHealth.Health -= damageAmount;
    }

    IEnumerator Death()
    {
        GameObject go = GameObject.FindGameObjectWithTag("Player");
        go.GetComponentInChildren<Animator>().SetBool("isDead", true);
        yield return new WaitForSeconds(2);
        uiManager.ShowResults();
        Time.timeScale = 0;
        Debug.Log("Dead");
        yield return null;
    }

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

        if (player == null) player = GameObject.FindGameObjectWithTag("Player");
        if (playerStats == null) playerStats = player.GetComponent<PlayerStats>();
        if (playerMovement == null) playerMovement = player.GetComponent<PlayerMovement>();

        playerStats.GetComponentInChildren<Animator>().SetBool("isDead", false);
        playerStats.OnReset();
        playerMovement.ResetPosition();
        ResetPools();

        score = 0;
        enemiesKilled = 0;
        
        uiManager.ResetAllHUDUI();
        Time.timeScale = 1;
    }

    public void LoadNextLevel()
    {
        currentLevel++;
        if (currentLevel == levels.Length)
        {
            //Win code
            print("Win");   
        }
        else
        {
            levels[currentLevel].SetActive(true);
        }
    }

    public void UnLoadLastLevel()
    {
        if (currentLevel != 0)
        {
            levels[currentLevel - 1].SetActive(false);
        }
    }

    public void SaveLevel()
    {
        
    }

    #region Object Pooling

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

    public void SetEnemy(GameObject go) // removes enemies
    {
        go.SetActive(false);
        IncreaseScore(10);
        enemiesRemaining--;
        enemiesKilled++;

        uiManager.UpdateEnemyRemainingText();
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

    #endregion
}