using Entities.Enemy.Enemy_Types;
using UnityEngine;

namespace Entities.Enemy.States
{
	public class AttackState : NpcState
	{
		private Vector3 targetMovePoint;

		public AttackState(AIEntity aiEntity) : base(aiEntity) {
			aiEntity.detectionRadius += 10;
			aiEntity.navAgent.SetDestination(aiEntity.transform.position);
		}

		public override void DoActions() {
			Attack();
			if (!aiEntity.InRange) aiEntity.PopState();
		}

		public void Attack() {
			int choice = aiEntity.GetRandomAttack();
			aiEntity.animationHandler.animator.SetInteger(ZombieAnimationHandler.AttackChoice, choice);
			aiEntity.animationHandler.animator.SetTrigger(ZombieAnimationHandler.AttackTrigger);
			aiEntity.StartCoroutine(WaitForAnimationFinish(aiEntity));
			Debug.Log("Combat");
		}
	}
}