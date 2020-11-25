public class SpeedUpgrade : Upgrade
{
    protected override void LevelStats(int level) => PlayerStats.MAX_SPEED += .1f;
}
