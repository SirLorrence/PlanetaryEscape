using System;
using System.Collections;
using System.IO.Compression;
using UnityEngine;

/// <summary>
/// This class handles the attack hit boxes using the animation event
/// </summary>
public class AttackHandler : MonoBehaviour
{
	public int amount;
	public GameObject leftHitBox;
	public GameObject rightHitBox;

	private Quaternion rotationZero = new Quaternion(0, 0, 0, 0);

	private void Start() {
		leftHitBox.GetComponent<DamageBox>().damageAmount = amount;
		rightHitBox.GetComponent<DamageBox>().damageAmount = amount;
		leftHitBox.SetActive(false);
		rightHitBox.SetActive(false);
	}
	public void EnableRightHit() => rightHitBox.SetActive(true);

	public void DisableRightHit() {
		StartCoroutine(ResetRotation());
		rightHitBox.SetActive(false);
	}

	public void EnableLeftHit() => leftHitBox.SetActive(true);

	public void DisableLeftHit() {
		StartCoroutine(ResetRotation());
		leftHitBox.SetActive(false);
	}

	IEnumerator ResetRotation() {
		float animTime = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;
		yield return new WaitForSeconds(animTime);
		transform.rotation = rotationZero;
	}

// public void EnableHeadHit() {
// }
//
// public void DisableHeadHit() {
// }
}