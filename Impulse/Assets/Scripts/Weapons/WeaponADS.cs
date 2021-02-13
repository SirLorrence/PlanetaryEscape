using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using UnityEngine;

public class WeaponADS : MonoBehaviour
{
	public bool toggleAimSights;

	public Transform Hip;
	public Transform Aim;

	public WeaponSelect weaponSelected;
	public float smooth;

	
	private void Update() {
		if (Input.GetMouseButton(1) || toggleAimSights) {
			LerpPosition(Aim.position, smooth);
		}
		else LerpPosition(Hip.position, smooth);
	}


	void LerpPosition(Vector3 targetLoc, float duration) {
			var startpos = weaponSelected.selectedWeapon.position;
			weaponSelected.selectedWeapon.position = Vector3.Lerp(startpos, targetLoc,  duration * Time.deltaTime);
			Debug.Log("Done");
	}
}