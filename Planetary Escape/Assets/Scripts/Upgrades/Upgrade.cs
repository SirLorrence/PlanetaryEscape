using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public abstract class Upgrade : MonoBehaviour
{
    public int cost;

    private int level = 0;

    private Slider levelSlider;

    private void Start()
    {
        levelSlider = this.gameObject.GetComponentInChildren<Slider>();
    }

    private void Update()
    {
        levelSlider.value = level;
    }

    public void LevelUp()
    {
        if (cost < GameManager.Instance.upgradePoints)
        {
            if (level != 5) //max level
            {
                level += 1;
                LevelStats(level);
                cost = (int) Mathf.Floor(cost * 2); // check this later
                PlayerStats.UpgradeEnable = true;
            }
        }
        else print("dont have enough points");
    }
    
    protected abstract void LevelStats(int level);
}