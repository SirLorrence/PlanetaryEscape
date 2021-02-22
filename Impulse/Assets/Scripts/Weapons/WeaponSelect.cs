using System;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class WeaponSelect : MonoBehaviour
{
	public int weaponSelected = 0;
	public Transform arms;
	public Transform body;
	private void Update() {
		for (int i = 0; i < arms.transform.childCount; i++) {
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