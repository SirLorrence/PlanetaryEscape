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
			Combat();
		}

		public void Combat() {
			Debug.Log("Combat");
			if (!aiEntity.InRange) aiEntity.PopState();
		}
	}
}