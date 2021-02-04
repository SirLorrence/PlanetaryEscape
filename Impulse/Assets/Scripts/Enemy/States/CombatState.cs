using System.Collections;
using Enemy.Enemy_Types;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy.States
{
	public class CombatState : NpcState
	{
		private Vector3 targetMovePoint;
		private bool isDone;
		private bool onEnter;

		public override void DoActions() {
			Combat();
		}
		public void Combat() {
			Debug.Log("Combat");
			// Debug.Log(entity.navAgent.remainingDistance);
			entity.targetPlayer = entity.ClosestTarget(entity.targets);
			if (entity.navAgent.hasPath == false && onEnter) {
				entity.navAgent.SetDestination(NewPath());
				onEnter = false;
			}
			else if (entity.navAgent.remainingDistance < 1f) {
				Debug.Log("Point Reached");
				entity.StartCoroutine(FindPath());
			}
			
			// if (Vector3.Distance(entity.targetPlayer.transform.position, entity.transform.position) <
			//     entity.exclusionZone) {
			// 	var playerPosition = entity.transform.position - entity.targetPlayer.position;
			// 	var backUp = entity.transform.position         + playerPosition;
			// 	entity.navAgent.SetDestination(backUp);
			// }
			
			entity.transform.LookAt(new Vector3(entity.targetPlayer.position.x, entity.transform.position.y
				, entity.targetPlayer.position.z));
		}
		IEnumerator FindPath() {
			isDone = false;
			Debug.Log("Waiting");
			entity.navAgent.ResetPath();
			yield return new WaitForSeconds(1f);
			Debug.Log("Going to new path");
			if (!isDone) targetMovePoint = NewPath();
			entity.navAgent.SetDestination(targetMovePoint);
		}

		Vector3 NewPath() {
			var randomPointX = Random.Range(entity.exclusionZone + 1, entity.combatDonut) * (Random.Range(0, 2) * 2 - 1);
			var randomPointZ = Random.Range(entity.exclusionZone + 1, entity.combatDonut) * (Random.Range(0, 2) * 2 - 1);

			var path = new Vector3(entity.targetPlayer.position.x + randomPointX, entity.targetPlayer.position.y,
				entity.targetPlayer.position.z                    + randomPointZ);

			Debug.LogWarning("New Path Ran");
			isDone = true;
			return path;
		}
		
		public CombatState(AIEntity aiEntity) : base(aiEntity) {
			entity.detectionRadius += 10;
			onEnter = true;
		}
	}
}