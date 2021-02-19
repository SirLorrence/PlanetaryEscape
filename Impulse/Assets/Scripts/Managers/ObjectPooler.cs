using System;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Collections;

public class ObjectPooler : NetworkBehaviour
{
    public bool debug = false;
    [Header("GameObjects to be Pooled")] public GameObjectToBePooled[] gameObjectsToBePooled;

    private List<GameObject> pooledGameObjects = new List<GameObject>();
    private float timer;

    private bool isInitialized = false;

    #region Singleton

    //Singleton Instantiation
    public static ObjectPooler Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this.gameObject);
        else
            Instance = this;

        DontDestroyOnLoad(this);
    }

    #endregion

    public void Initialize()
    {
        if (debug) {print("Pooling has started"); timer = Time.realtimeSinceStartup; } 

        for (int i = 0; i < gameObjectsToBePooled.Length; i++)
        {
            for (int j = 0; j < gameObjectsToBePooled[i].amountToBePooled; j++)
            {
                GameObject go = Instantiate(gameObjectsToBePooled[i].gameObjectToBePooled);
                go.SetActive(false);
                go.transform.SetParent(transform);
                pooledGameObjects.Add(go);
            }
        }
        isInitialized = true;

        if (debug) {print("Pooling has ended, " + pooledGameObjects.Count + " Pooled Objects"); print("Generating Pool Took " + (Time.realtimeSinceStartup - timer));}
    }

    public GameObject GetGameObject(int index)
    {
        if (!isInitialized) Initialize();

        int startPosInList = 0;
        for (int i = 0; i < index; i++)
        {
            startPosInList += gameObjectsToBePooled[i].amountToBePooled;
        }

        if (debug) print("Start Position in List is " + startPosInList);

        for (int i = startPosInList; i < startPosInList + gameObjectsToBePooled[index].amountToBePooled; i++)
        {
            if (!pooledGameObjects[i].activeSelf) { if (debug) print("Object " + pooledGameObjects[i].name + " Found at position " + i); return pooledGameObjects[i];}
        }

        if (debug) print("No Objects Ready in Pool " + gameObjectsToBePooled[index].name);

        if (gameObjectsToBePooled[index].loadMoreIfNoneLeft)
        {
            if (debug) print("Added Object in Pool " + gameObjectsToBePooled[index].name);
            pooledGameObjects.Insert(startPosInList + gameObjectsToBePooled[index].amountToBePooled, gameObjectsToBePooled[index].gameObjectToBePooled);
            gameObjectsToBePooled[index].amountToBePooled++;
            return pooledGameObjects[startPosInList + gameObjectsToBePooled[index].amountToBePooled - 1];
        }

        return null;
    }

    public void ResetPool()
    {
        foreach (var objectInPool in pooledGameObjects)
        {
            if (objectInPool.activeSelf)
            {
                SetObjectInPool(objectInPool);
            }
        }
    }

    public void SetObjectInPool(GameObject go)
    {
        if (debug) print(go.name + " Set in Pool");

        go.SetActive(false);
        go.transform.position = Vector3.zero;
    }
    [Command]
    public void CmdSpawnBulletOnServer(GameObject go)
    {
        NetworkServer.Spawn(go);
    }
}

[Serializable]
public struct GameObjectToBePooled
{
    [Header("Object Info")]
    public string name;
    public int amountToBePooled;
    public GameObject gameObjectToBePooled;

    [Header("Settings")] 
    public bool loadMoreIfNoneLeft;
}