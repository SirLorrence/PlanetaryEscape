using System;
using UnityEngine;

namespace Entities
{
	[RequireComponent(typeof(Rigidbody))]
	public class GameEntity : MonoBehaviour
	{
		[Header("Health Values")] public int health;
		public int armor;
		public bool isAlive;
		protected int maxHealth;
		protected int maxArmor;
		protected Rigidbody rb;
		protected Vector3 entityPosition;


		protected virtual void Awake() {
			rb = GetComponent<Rigidbody>();
		}


		#region Health System

		public virtual void SetHealth(int value) {
			health = value;
			maxHealth = value;
		}

		public virtual void SetArmor(int value) {
			armor = value;
			maxArmor = value;
		}

		public virtual void TakeDamage(int damage) {
			if (armor != 0) {
				ApplyValueChangeToArmor(-damage);
				if (armor < 0) {
					ApplyValueChangeToHealth(armor);
					armor = 0;
				}
			}
			else {
				if (health != 0) ApplyValueChangeToHealth(-damage);
				else health = 0;
			}
		}

		public virtual void Health(int value) => ApplyValueChangeToHealth(value);
		public virtual void AddArmor(int value) => ApplyValueChangeToArmor(value);

		#endregion


		#region Value Management

		/// <summary>
		/// Changes the value for only the armor value
		/// </summary>
		public void ApplyValueChangeToArmor(int value) => armor = ValueChange(armor, maxArmor, value);

		/// <summary>
		/// Changes the value for only the health value
		/// </summary>
		public void ApplyValueChangeToHealth(int value) => health = ValueChange(health, maxHealth, value);


		/// <summary>
		/// Applies changes to the target value with in its range. (0, Set Max)
		/// </summary>
		/// <param name="targetValue">The value you want to change</param>
		/// <param name="maxValue">This is the maximum value it can go to. Cannot be zero</param>
		/// <param name="value">This is the value of how it can be changed.</param>
		/// <returns></returns>
		private int ValueChange(int targetValue, int maxValue, int value) {
			targetValue += value;
			if (targetValue >= maxValue) targetValue = maxValue;
			return targetValue;
		}

		#endregion
	}
}