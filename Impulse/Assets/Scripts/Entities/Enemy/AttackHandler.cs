using System.Collections;
using Entities.Enemy.Enemy_Types;
using UnityEngine;

namespace Entities.Enemy
{
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
	}
}