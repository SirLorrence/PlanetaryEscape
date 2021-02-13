using System;
using UnityEngine;

public class WeaponSelect : MonoBehaviour
{
	public int weaponSelected = 0;

	public Transform gripRef;
	public Transform holdRef;
	public Transform selectedWeapon;


	private void Update() {
		int i = 0;
		foreach (Transform w in transform) {
			if (i == weaponSelected) {
				w.gameObject.SetActive(true);
				HandleIK(w);
				selectedWeapon = w;
			}
			else w.gameObject.SetActive(false);
			++i;
		}
	}

	void HandleIK(Transform weapon) {
		var grip = weapon.transform.Find("Grip");
		var holder = weapon.transform.Find("Holder");

		gripRef.position = grip.transform.position;
		gripRef.rotation = grip.transform.rotation;
		
		holdRef.position = holder.transform.position;
		holdRef.rotation = holder.transform.rotation;
	}
}