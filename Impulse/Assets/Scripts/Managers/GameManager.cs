using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevel;

public class GameManager : MonoBehaviour
{
    #region Variables

    [Header("Editor Settings")] 
    public bool debug;


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
    }

    #endregion

    public void ResetGame()
    {

    }
}