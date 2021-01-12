using UnityEngine;
public class ShieldUpgrade : Upgrade
{
    bool valueCheck;

    private void OnEnable()
    {
        level = GameManager.Instance.SHIELD_LEVEL;
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
        PlayerStats.MAX_SHIELD += 5;
        GameManager.Instance.SHIELD_LEVEL = level;
    }
}