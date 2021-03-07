using System.Collections;
using Enemy.Enemy_Types;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy.States
{
	public class AttackState : NpcState
	{
		private Vector3 targetMovePoint;
		private bool onEnter;

		public AttackState(AIEntity aiEntity) : base(aiEntity) {
			base.aiEntity.detectionRadius += 10;
			onEnter = true;
		}

		public override void DoActions() {
			Combat();
		}

		public void Combat() {
			Debug.Log("Combat");
			if (!aiEntity.InRange) aiEntity.PopState();
		}
	}
}