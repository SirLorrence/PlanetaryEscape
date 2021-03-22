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
		[ReadOnly]public int amount;
		public GameObject leftHitBox;
		public GameObject rightHitBox;

		private Quaternion rotationZero = new Quaternion(0, 0, 0, 0);

		private DamageBox leftDamageBox;
		private DamageBox rightDamageBox;

		private void Awake() {
			leftDamageBox = leftHitBox.GetComponent<DamageBox>();
			rightDamageBox = rightHitBox.GetComponent<DamageBox>();
		}
		private void Start() {
			leftDamageBox.damageAmount = amount;
			rightDamageBox.damageAmount = amount;
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
			// transform.position = new Vector3(transform.position.x, 0, transform.position.z);
		}
	}
}