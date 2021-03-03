using System;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class WeaponSelect : MonoBehaviour
{
	[SerializeField]private int weaponSelected = 0;
	public Transform arms;
	public Transform body;

	[HideInInspector] public int weaponCount; 
	public int WeaponSelected {
		get => weaponSelected;
		set => weaponSelected = value;
	}
	private void OnEnable() => weaponCount = arms.childCount;

	private void Update() {
		for (int i = 0; i < weaponCount; i++) {
			if (i == weaponSelected) {
				arms.GetChild(i).gameObject.SetActive(true);
				body.GetChild(i).gameObject.SetActive(true);
				
				// selectedWeapon = arms.GetChild(i);
			}
			else {
				arms.GetChild(i).gameObject.SetActive(false);
				body.GetChild(i).gameObject.SetActive(false);
			}
		}
	}
}