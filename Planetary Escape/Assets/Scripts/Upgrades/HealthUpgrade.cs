public class HealthUpgrade : Upgrade
{
    protected override void LevelStats(int level) => PlayerStats.MAX_HEALTH += 10;
}