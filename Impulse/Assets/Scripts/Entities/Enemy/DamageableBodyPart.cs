using System;
using System.Collections;
using Entities.Enemy.Enemy_Types;
using Managers;
using UnityEngine;

namespace Entities.Enemy
{
	public class DamageableBodyPart : MonoBehaviour
	{
		private enum BodyParts
		{
			Head,
			Body,
			Arm,
			Leg
		}

		[SerializeField] private BodyParts bodyPart;
		private AIEntity entity;
		private void Awake() => entity = GetComponentInParent<AIEntity>();

		public IEnumerator DealDamage(float amount) {
			var multiplier = bodyPart switch {
				BodyParts.Head => 2f,
				BodyParts.Body => 1f,
				BodyParts.Arm  => .5f,
				BodyParts.Leg  => .5f,
				_              => throw new ArgumentOutOfRangeException()
			};
			Debug.Log($"Damage: {amount * multiplier}");
			SoundManager.Instance.PlayAudio(AudioTypes.SFX_COMMON_ZOMBIE_GROAN);
			entity.TakeDamage(Mathf.FloorToInt(amount * multiplier));
			yield return null;
		}
	}
}