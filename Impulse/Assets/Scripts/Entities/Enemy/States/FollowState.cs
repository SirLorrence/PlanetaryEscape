using System.Collections;
using Entities.Enemy.Enemy_Types;

namespace Entities.Enemy.States
{
	public class FollowState : NpcState
	{
		//to only run the loop once
		private bool changedSpeed;

		public FollowState(AIEntity aiEntity) : base(aiEntity) {
			changedSpeed = false;
		}

		public override void DoActions() {
			var targets = aiEntity.PlayersInRange();
			aiEntity.targetPlayer = aiEntity.ClosestTarget(targets);
			if (aiEntity.targetPlayer == null) aiEntity.FindRandomTarget();

			aiEntity.navAgent.SetDestination(aiEntity.targetPlayer.position);

			if (aiEntity.PlayerFound && !changedSpeed) {
				if (aiEntity.CheckIfAggro()) {
					aiEntity.navAgent.speed = aiEntity.chaseSpeed * aiEntity.speedMultiplier;
				}
				else aiEntity.navAgent.speed = aiEntity.followSpeed * aiEntity.speedMultiplier;

				changedSpeed = true;
			}

			if (aiEntity.animationHandler.animator.GetCurrentAnimatorStateInfo(0).IsName("Scream")) {
				aiEntity.StartCoroutine(WaitForAnimationFinish(aiEntity));
			}

			if (aiEntity.InRange)
				aiEntity.PushState(new AttackState(aiEntity));
		}

		public override IEnumerator WaitForAnimationFinish(AIEntity entity) {
			aiEntity.navAgent.SetDestination(aiEntity.transform.position);
			return base.WaitForAnimationFinish(entity);
		}

	
	}
}

