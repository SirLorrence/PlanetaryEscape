using UnityEngine;

public class WeaponUpgrade : Upgrade
{
     bool valueCheck;
    private void OnEnable()
    {
        level = GameManager.Instance.GUN_LEVEL;
        valueCheck = false;
        if (!valueCheck)
        {
            if (level != 0)
            {
                cost = 5 * (int)Mathf.Pow(2, level);
                valueCheck = false;
                print("value check ran");
            }
        }
    }
    protected override void LevelStats(int level)
    {
        // PlayerStats.Instance.Damage *= level;
        // PlayerGameStats.Instance.playerBulletSpeed *= level / 2;

    }
}