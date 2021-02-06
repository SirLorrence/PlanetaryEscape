using System;
using Enemy.Enemy_Types;
using UnityEngine;

namespace Enemy.States
{
	public class CoverState : NpcState
	{
		public CoverState(AIEntity aiEntity) : base(aiEntity) {
		}

		public override void DoActions() {
			entity.coverPos = CoverCalutations();
			entity.navAgent.SetDestination(entity.coverPos);
		}


		private Vector3 CoverCalutations() {
			var coverPosition = GetCoverPosition();
			var t = Vector3.Normalize(coverPosition - entity.targetPlayer.position);
			var goTo = (new Vector3(t.x, 0, t.z) * 1f) + coverPosition;
			return goTo;
		}
		Vector3 GetCoverPosition() {
			Vector3 cover = new Vector3();
			double nearestCover = 15f;

			LayerMask coverMask = LayerMask.GetMask("Cover");

			var coverObjects = Physics.OverlapSphere(entity.transform.position, entity.detectionRadius, coverMask);

			foreach (var c in coverObjects) {
				var distance = Vector3.Distance(entity.transform.position, c.transform.position);
				Debug.DrawLine(entity.transform.position, c.transform.position);
				if (distance < nearestCover) {
					nearestCover = distance;
					cover = c.transform.position;
				}
			}
			// Debug.Log("Cover options: " + coverObjects.Length);
			Debug.DrawLine(entity.transform.position, cover, Color.blue);
			return cover;
		}

	}
}