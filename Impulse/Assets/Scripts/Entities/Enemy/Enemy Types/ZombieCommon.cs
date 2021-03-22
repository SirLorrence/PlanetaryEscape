
namespace Entities.Enemy.Enemy_Types
{
	public class ZombieCommon : AIEntity
	{
		private void OnEnable() {
			SetAggroLevel();
			navAgent.speed = wonderSpeed * speedMultiplier;
		}
		public override void Update() {
			currentState.DoActions();
			base.Update();
		}
	}
}