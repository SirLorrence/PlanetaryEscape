using System.Collections;
using Entities.Enemy.Enemy_Types;
using Managers;
using UnityEngine;

namespace Entities.Enemy.States
{
	public class AttackState : NpcState
	{
		private Vector3 targetMovePoint;
		private bool _attacking;

		public AttackState(AIEntity aiEntity) : base(aiEntity) {
			aiEntity.detectionRadius += 10;
			aiEntity.navAgent.SetDestination(aiEntity.transform.position);
		}

		public override void DoActions() {
			if (!aiEntity.InRange     && !_attacking) aiEntity.PopState();
			else if (aiEntity.InRange && !_attacking) Attack(); 
			
			if (aiEntity.health <= 0) aiEntity.SetState(new DeathState(aiEntity));
		}

		public void Attack() {
			_attacking = true;
			int choice = aiEntity.GetRandomAttack();
			aiEntity.animationHandler.animator.SetInteger(ZombieAnimationHandler.AttackChoice, choice);
			aiEntity.animationHandler.animator.SetTrigger(ZombieAnimationHandler.AttackTrigger);
			aiEntity.StartCoroutine(WaitForAnimationFinish(aiEntity));
		}

		public override IEnumerator WaitForAnimationFinish(AIEntity entity) {
			SoundManager.Instance.PlayAudio(AudioTypes.SFX_COMMON_ZOMBIE_ATTACK);
			float animationTime = entity.animationHandler.animator.GetCurrentAnimatorStateInfo(0).length;
			yield return new WaitForSeconds(animationTime);
			_attacking = false;
			yield return null;
		}
	}
}