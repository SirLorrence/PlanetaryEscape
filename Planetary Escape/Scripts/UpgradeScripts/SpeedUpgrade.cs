using UnityEngine;
public class SpeedUpgrade : Upgrade
{
    bool valueCheck;

    private void OnEnable()
    {
        level = GameManager.Instance.SPEED_LEVEL;
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
        PlayerStats.MAX_SPEED += .1f;
        level = GameManager.Instance.SPEED_LEVEL = level;
    }
}