using System.Collections;
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
				if (aiEntity.CheckIfAggro()) {
					aiEntity.navAgent.speed = aiEntity.chaseSpeed * aiEntity.speedMultiplier;
				}
				else aiEntity.navAgent.speed = aiEntity.followSpeed * aiEntity.speedMultiplier;

				changedSpeed = true;
			}

			if (aiEntity.zAnimator.animator.GetCurrentAnimatorStateInfo(0).IsName("Scream")) {
				aiEntity.StartCoroutine(Rage());
			}

			if (aiEntity.InRange)
				aiEntity.PushState(new AttackState(aiEntity));
		}

		private IEnumerator Rage() {
			float animationTime = aiEntity.zAnimator.animator.GetCurrentAnimatorStateInfo(0).length;
			aiEntity.navAgent.SetDestination(aiEntity.transform.position);
			Debug.Log("Wait");
			yield return new WaitForSeconds(animationTime);
		}
	}
}

