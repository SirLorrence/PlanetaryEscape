using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompletionManager : MonoBehaviour
{
    public GameObject[] enemies;
    public GameObject enemyHolder;
    public GameObject doorCollider;

    private void OnEnable()
    {
        //-------Auto fill array
        enemies = new GameObject[enemyHolder.transform.childCount];
        for (int i = 0; i < enemyHolder.transform.childCount; i++)
        {
            enemies[i] = enemyHolder.transform.GetChild(0).gameObject;
        }
    }

    void FixedUpdate()
    {
        if (IsCompletedCheck()) AdvanceLevel();
    }

    bool IsCompletedCheck()
    {
        foreach (var go in enemies)
            if (go.activeSelf)
                return false;
        return true;
    }

    void AdvanceLevel()
    {
        //Put Advanced Level Code Here
        Debug.Log("AdvancedLevel");
        doorCollider.SetActive(true);
        gameObject.SetActive(false);
    }
}