using System.Collections;
using Enemy.Enemy_Types;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy.States
{
	public class BasicCombatState : NpcState
	{
		public bool hasPath = false;
		private Vector3 moveArea;


		public override void DoActions() {
			Combat();
		}

		public void Combat() {
			Debug.Log("Combat");
			// Debug.Log(entity.navAgent.remainingDistance);
			entity.targetPlayer = entity.ClosestTarget(entity.targets);

			// if (entity.weapon != null) {
			// 	entity.weapon.FireWeapon();
			// }

			// entity.navAgent.SetDestination(entity.targetPlayer.position);
			// if (Vector3.Distance(entity.targetPlayer.position, entity.transform.position) < entity.exclusionZone)
			// NewPath();

			if (entity.navAgent.remainingDistance < 1f) {
				hasPath = false;
			}

			if (!hasPath)
				entity.StartCoroutine(FindPath());

			entity.navAgent.SetDestination(moveArea);

			// var r = Quaternion.LookRotation(entity.targetPlayer.position).eulerAngles;
			entity.transform.LookAt(new Vector3(entity.targetPlayer.position.x, entity.transform.position.y
				, entity.targetPlayer.position.z));
		}

		IEnumerator FindPath() {
			Debug.Log("Waiting");
			// yield return new WaitForSeconds(Random.Range(5, 20));
			yield return new WaitForSeconds(1f);
			Debug.Log("Going to new path");
			NewPath();
			yield return null;
		}

		void NewPath() {
			var randomPointx = Random.Range(entity.exclusionZone, entity.combatDonut) * (Random.Range(0, 2) * 2 - 1);
			var randomPointz = Random.Range(entity.exclusionZone, entity.combatDonut) * (Random.Range(0, 2) * 2 - 1);

			moveArea = new Vector3(entity.targetPlayer.position.x + randomPointx, entity.targetPlayer.position.y,
				entity.targetPlayer.position.z                    + randomPointz);

			hasPath = true;
		}


		public BasicCombatState(AIEntity aiEntity) : base(aiEntity) {
			entity.detectionRadius += 10;
		}
	}
}