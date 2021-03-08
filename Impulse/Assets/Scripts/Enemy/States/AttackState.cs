using System.Collections;
using System.Configuration;
using Enemy.Enemy_Types;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy.States
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
			aiEntity.zAnimator.animator.SetInteger(ZombieAnimationHandler.AttackChoice, choice);
			aiEntity.zAnimator.animator.SetTrigger(ZombieAnimationHandler.AttackTrigger);
			aiEntity.StartCoroutine(FinishAttack());
			Debug.Log("Combat");
		}

		IEnumerator FinishAttack() {
			float animationTime = aiEntity.zAnimator.animator.GetCurrentAnimatorStateInfo(0).length;
			yield return new WaitForSeconds(animationTime);
		}
	}
}