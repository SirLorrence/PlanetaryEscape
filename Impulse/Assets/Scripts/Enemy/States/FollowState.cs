using Enemy.Enemy_Types;
using UnityEngine;
using UnityEngine.UI;

namespace Enemy.States
{
	public class FollowState : NpcState
	{
		public FollowState(AIEntity aiEntity) : base(aiEntity) {
		}

		public override void DoActions() {
			var targets = aiEntity.PlayersInRange();
			aiEntity.targetPlayer = aiEntity.ClosestTarget(targets);
			aiEntity.navAgent.SetDestination(aiEntity.targetPlayer.position);

			if (aiEntity.PlayerFound) {
				aiEntity.navAgent.speed = (aiEntity.CheckIfAggro())
					? aiEntity.chaseSpeed  * aiEntity.speedMultiplier
					: aiEntity.followSpeed * aiEntity.speedMultiplier;
			}

			if (aiEntity.InRange) aiEntity.PushState(new AttackState(aiEntity));
		}
	}
}