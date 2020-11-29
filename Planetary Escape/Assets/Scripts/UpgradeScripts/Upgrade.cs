using System;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public abstract class Upgrade : MonoBehaviour
{
    public int cost;

    private int level = 0;

    public Slider levelSlider;
    private Text costText;

    private void Start()
    {
        costText = this.gameObject.GetComponentInChildren<Text>();
    }

    private void Update()
    {
        levelSlider.value = level;
        costText.text = cost.ToString();
    }
    public void LevelUp()
    {
        if ( GameManager.Instance.upgradePoints >= cost )
        {
            if (level != 5) //max level
            {
                level += 1;
                LevelStats(level);
                PlayerStats.UpgradeEnable = true;
                GameManager.Instance.upgradePoints -= cost;
                cost = (int)Mathf.Floor(cost * 2); // check this later
                GameManager.Instance.uiManager.UpdateUpgradeCostText();
            }
        }
        else print("dont have enough points");
    }

    protected abstract void LevelStats(int level);
}