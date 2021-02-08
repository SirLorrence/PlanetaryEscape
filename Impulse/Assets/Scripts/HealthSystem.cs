using System;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
	public HealthData healthData;
	[Header("Set initial Values")] public int maxHealth;
	public int maxAmour;

	private void Start() {
		healthData = ScriptableObject.CreateInstance<HealthData>();
		healthData.name = "Health Information";
		healthData.SetValues(maxHealth, maxAmour);
	}

	public void TakeDamage(int damage) => healthData.ApplyValueChangeToBoth(-damage);
	public void Health(int health) => healthData.ApplyValueChangeToHealth(health);
	public void AddArmor(int armor) => healthData.ApplyValueChangeToArmor(armor);
}