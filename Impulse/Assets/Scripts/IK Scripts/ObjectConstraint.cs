using UnityEngine;

namespace IK_Scripts
{
	public class ObjectConstraint : MonoBehaviour
	{
		public Transform constrainObject;
		public Transform sourceObject;
		[Range(0,1)]
		public float weight;



		// private void OnAnimatorIK(int layerIndex) {
		// 	
		// 	constrainObject.position = sourceObject.position;
		// 	constrainObject.rotation = sourceObject.rotation;
		// }
		private void LateUpdate() {
			constrainObject.position = sourceObject.position;
			constrainObject.rotation = sourceObject.rotation;
		}
	}
}