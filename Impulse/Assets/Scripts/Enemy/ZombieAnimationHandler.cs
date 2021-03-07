using System;
using UnityEngine;

namespace Enemy
{
	public class ZombieAnimationHandler : MonoBehaviour
	{
		public Animator animator;

		private static readonly int MoveX = Animator.StringToHash("movementX");
		private static readonly int MoveY = Animator.StringToHash("movementY");
		public static readonly int AggroCheck = Animator.StringToHash("isAggro");
		public static readonly int DeathTrigger = Animator.StringToHash("Death");
		public static readonly int AttackTrigger = Animator.StringToHash("Attack");

		public void AnimHandler(float x, float y, bool aggroBool) {
			animator.SetFloat(MoveX, x, 0.1f, Time.deltaTime);
			animator.SetFloat(MoveY, y, 0.1f, Time.deltaTime);
			animator.SetBool(AggroCheck, aggroBool);
		}

		public void SetTrigger(int id) => animator.SetTrigger(id);

		private void Start() {
			animator = GetComponent<Animator>();
			if (animator == null) animator = GetComponentInChildren<Animator>();
		}
	}
}