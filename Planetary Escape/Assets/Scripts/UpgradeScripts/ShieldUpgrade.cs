public class ShieldUpgrade : Upgrade
{
    protected override void LevelStats(int level) => PlayerStats.MAX_SHIELD += 5;
}