using System;
using UnityEngine;

public class HealthUpgrade : Upgrade
{
    bool valueCheck;

    private void OnEnable()
    {
        
        level = GameManager.Instance.HEALTH_LEVEL;
        valueCheck = false;
        if (!valueCheck)
        {
            if (level != 0)
            {
                cost = cost * (int)Mathf.Pow(2, level);
                valueCheck = false;
                print("value check ran");
            }
        }
    }

    protected override void LevelStats(int level)
    {
        PlayerStats.MAX_HEALTH += 10;
        GameManager.Instance.HEALTH_LEVEL = level;
    }
}