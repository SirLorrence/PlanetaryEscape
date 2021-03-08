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
		public static readonly int DeathTrigger = Animator.StringToHash("death");
		public static readonly int AttackTrigger = Animator.StringToHash("attack");
		public static readonly int AttackChoice = Animator.StringToHash("attackChoice");

		public void AnimHandler(float x, float y, bool aggroBool) {
			animator.SetFloat(MoveX, x, 0.1f, Time.deltaTime);
			animator.SetFloat(MoveY, y, 0.1f, Time.deltaTime);
			animator.SetBool(AggroCheck, aggroBool);
		}
		private void Start() {
			animator = GetComponent<Animator>();
			if (animator == null) animator = GetComponentInChildren<Animator>();
		}
	
	}
}