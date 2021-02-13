using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandIK : MonoBehaviour
{
	public Animator animator;
	public bool ikActive = false;


	public Transform weapon;
	public Transform rightHandObj = null;
	public Transform leftHandObj = null;
	public Transform lookObj = null;


	private void OnEnable() {
		// animator = GetComponent<Animator>();
	}

	private void OnAnimatorIK(int layerIndex) {
		if (animator) {
			if (ikActive) {
				if (rightHandObj != null) {
					animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
					animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);

					animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandObj.position);
					animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandObj.rotation);
				}

				if (leftHandObj != null) {
					animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
					animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);

					animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandObj.position);
					animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandObj.rotation);
				}
			}
			else {
				animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
				animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);

				animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);
				animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0);
				animator.SetLookAtWeight(0);
			}
		}
		else {
			weapon.transform.SetParent(null);
		}
	}
}