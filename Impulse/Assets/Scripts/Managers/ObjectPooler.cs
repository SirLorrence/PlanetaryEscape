using System;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class ObjectPooler : MonoBehaviour
    {
        #region Variables
        public bool debug = false;
        [Header("GameObjects to be Pooled")] public GameObjectToBePooled[] gameObjectsToBePooled;

        private List<GameObject> _pooledGameObjects = new List<GameObject>();
        private float timer;

        private bool isInitialized = false;
        #endregion

        #region Singleton

        //Singleton Instantiation
        public static ObjectPooler Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(gameObject);
            else
                Instance = this;

        }

        #endregion

        #region Initialize
        public void Initialize()
        {
            if (debug) { print("Pooling has started"); timer = Time.realtimeSinceStartup; }

            for (int i = 0; i < gameObjectsToBePooled.Length; i++)
            {
                for (int j = 0; j < gameObjectsToBePooled[i].amountToBePooled; j++)
                {
                    GameObject go = Instantiate(gameObjectsToBePooled[i].gameObjectToBePooled);
                    go.SetActive(false);
                    go.transform.SetParent(transform);
                    _pooledGameObjects.Add(go);
                }
            }
            isInitialized = true;

            if (debug) { print("Pooling has ended, " + _pooledGameObjects.Count + " Pooled Objects"); print("Generating Pool Took " + (Time.realtimeSinceStartup - timer)); }
        }

        #endregion

        #region Modifying the Pool 
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
                if (!_pooledGameObjects[i].activeSelf) { if (debug) print("Object " + _pooledGameObjects[i].name + " Found at position " + i); return _pooledGameObjects[i];}
            }

            if (debug) print("No Objects Ready in Pool " + gameObjectsToBePooled[index].name);

            if (gameObjectsToBePooled[index].loadMoreIfNoneLeft)
            {
                if (debug) print("Added Object in Pool " + gameObjectsToBePooled[index].name);
                _pooledGameObjects.Insert(startPosInList + gameObjectsToBePooled[index].amountToBePooled, gameObjectsToBePooled[index].gameObjectToBePooled);
                gameObjectsToBePooled[index].amountToBePooled++;
                return _pooledGameObjects[startPosInList + gameObjectsToBePooled[index].amountToBePooled - 1];
            }

            return null;
        }

        public bool AreObjectsLeftInPool(string name)
        {
            if (!isInitialized) return false;

            int startPosInList = 0;
            int index = 0;
            for (int i = 0; i < gameObjectsToBePooled.Length; i++)
            {
                if (gameObjectsToBePooled[i].name != name)
                {
                    startPosInList += gameObjectsToBePooled[i].amountToBePooled;
                }
                else
                {
                    index = i;
                    break;
                }
            }

            for (int i = startPosInList; i < startPosInList + gameObjectsToBePooled[index].amountToBePooled; i++)
            {
                if (!_pooledGameObjects[i].activeSelf) return true;
            }

            return false;
        }

        public void ResetPool()
        {
            foreach (var objectInPool in _pooledGameObjects)
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
        #endregion
    }

    #region Structs
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
    #endregion
}