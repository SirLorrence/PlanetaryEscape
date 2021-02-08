using UnityEngine;

[CreateAssetMenu(fileName = "Health", menuName = "ScriptableObjects/HealthData")]
public class HealthData : ScriptableObject
{
	public int health;
	public int armor;

	private int maxHealth;
	private int maxArmor;


	/// <summary>
	///  Set Initial Values.
	/// Should only be used in Start or Awake
	/// </summary>
	/// <param name="healthValue">Enter health value that will be its Max Value</param>
	/// <param name="armorValue">Enter armor value that will be its Max Value</param>
	public void SetValues(int healthValue, int armorValue) {
		health = healthValue;
		armor = armorValue;

		maxHealth = healthValue;
		maxArmor = armorValue;
	}

	/// <summary>
	/// Applies new information to both health stats. First effects armor, then health. 
	/// </summary>
	public void ApplyValueChangeToBoth(int value) {
		if (armor != 0) {
			armor += value;
			if (armor >= maxArmor) armor = maxArmor;
			if (armor < 0) {
				ApplyValueChangeToHealth(armor);
				armor = 0;
			}
		}
		else {
			ApplyValueChangeToHealth(value);
		}
	}

	/// <summary>
	/// Applies value only to armor data
	/// </summary>
	public void ApplyValueChangeToArmor(int value) {
		armor += value;
		if (armor >= maxArmor) armor = maxArmor;
		if (armor < 0) armor = 0;
	}

	/// <summary>
	/// Applies value only to health data
	/// </summary>
	public void ApplyValueChangeToHealth(int value) {
		health += value;
		if (health >= maxHealth) health = maxHealth;
		if (health < 0) health = 0;
	}
}