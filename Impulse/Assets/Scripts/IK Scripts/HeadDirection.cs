using System;
using UnityEngine;

namespace IK_Scripts
{
	public class HeadDirection : MonoBehaviour

	{
		protected Animator animator;

		// protected Rigidbody rigidbody;
		public CameraFollow cameraFollow;
		public Transform camPivot;
		[Range(0, 1)] public float weight;

		private void OnEnable() {
			animator = GetComponent<Animator>();
			cameraFollow = GetComponentInChildren<CameraFollow>();
			// rigidbody = GetComponent<Rigidbody>();
			// rigidbody.isKinematic = true;
		}

		private void OnAnimatorIK(int layerIndex) {
			if (animator) {
				animator.SetLookAtWeight(weight);
				animator.SetLookAtPosition(camPivot.position);
			}
		}
	}
}