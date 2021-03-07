using Enemy.Enemy_Types;
using UnityEngine;
using UnityEngine.UI;

namespace Enemy.States
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
				aiEntity.navAgent.speed = (aiEntity.CheckIfAggro())
					? aiEntity.chaseSpeed  * aiEntity.speedMultiplier
					: aiEntity.followSpeed * aiEntity.speedMultiplier;
				changedSpeed = true;
			}

			if (aiEntity.InRange) aiEntity.PushState(new AttackState(aiEntity));
		}
	}
}