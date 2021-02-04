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
			var coverPosition = GetCoverPosition();
			Debug.DrawLine(entity.transform.position, coverPosition, Color.red);
		}
		
		Vector3 GetCoverPosition() {
			Vector3 cover = new Vector3();
			
			float nearestCover = 15f;

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
			Debug.Log("Cover options: " + coverObjects.Length);
			return cover;
		}
	}
}