using System;
using UnityEngine;

namespace Enemy.Enemy_Types
{
	public class Turret : AIEntity
	{
		#region variables/fields

		[Range(0, 20)] public float rotationTime;
		[Header("Turret Restraints")] public int lockHorizontal, lockVertical; // lock the degrees


		[SerializeField] private Transform turretHead;
		private Quaternion restPos;
		private float minHorizontalR, maxHorizontalR, minVerticalR, maxVerticalR;
		

		#endregion


		void OnEnable() {
			fieldOfView = 360; // the turret is aware of everything...

			turretHead = transform.Find("Pivot");
			restPos = turretHead.rotation;
			
			var turretAxis = turretHead.eulerAngles;
			minHorizontalR = turretAxis.y - lockHorizontal;
			maxHorizontalR = turretAxis.y + lockHorizontal;
			minVerticalR = 0              - lockVertical;
			maxVerticalR = 0              + lockVertical;
		}

		void Update() {
			targets = PlayersInRange();
			if (targets.Length != 0 ) {
				FollowRotation();
			}
			else Reset();
		}

		void FollowRotation() {
			targetPlayer = ClosestTarget(targets);
			var targetLocation = targetPlayer.position - turretHead.position;
			var targetLookRotation = Quaternion.LookRotation(targetLocation);

			var rotation = Quaternion.Lerp(turretHead.rotation, targetLookRotation, rotationTime * Time.deltaTime)
				.eulerAngles;

			var rotationClampHorizontal = Mathf.Clamp(rotation.y, minHorizontalR, maxHorizontalR);
			var rotationClampVertical = ClampNegative(rotation.x, minVerticalR, maxVerticalR);

			turretHead.rotation = Quaternion.Euler(rotationClampVertical, rotationClampHorizontal, 0f);
			
		}

		private void Reset() {
			if (turretHead.rotation != restPos)
				turretHead.rotation = Quaternion.Slerp(turretHead.rotation, restPos, rotationTime * Time.deltaTime);
		}

		float ClampNegative(float rotationAxis, float min, float max) {
			var newMinNum = 360 + min;

			Debug.Log("Rotation X: " + Mathf.Floor(rotationAxis));

			if (rotationAxis > newMinNum || rotationAxis < max) {
				return rotationAxis;
			}
			else if (rotationAxis <= newMinNum && rotationAxis > 180f) {
				Debug.Log("min Hit");
				return min;
			}
			else if (rotationAxis >= max && rotationAxis < 180f) {
				Debug.Log("Max Hit");
				return max;
			}
			else return 0;
		}
	}
}