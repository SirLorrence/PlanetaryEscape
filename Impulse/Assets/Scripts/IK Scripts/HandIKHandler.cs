using UnityEngine;

namespace IK_Scripts
{
	/// <summary>
	/// Handles the left hand IK, needs to be on a Game object that has the weapon as its child. 
	/// </summary>
	public class HandIKHandler : MonoBehaviour
	{
		public Transform targetHoldRef;
		private void Update() {
			foreach (Transform w in transform) {
				if (w.gameObject.activeInHierarchy) {
					HandleIK(w);
				}
			}
		}
		public void HandleIK(Transform weapon) {
			var holder = weapon.transform.Find("Holder");
			targetHoldRef.position = holder.transform.position;
			targetHoldRef.rotation = holder.transform.rotation;
		}
	}
}