using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using UnityEngine;

public class WeaponADS : MonoBehaviour
{
	public bool toggelAimSights;

	public Transform Hip;
	public Transform weapon;
	public Transform Aim;

	public float smooth;

	private void Update() {
		if (Input.GetMouseButton(1) || toggelAimSights) {
			LerpPosition(Aim.position, smooth);
		}
		else LerpPosition(Hip.position, smooth);
	}


	void LerpPosition(Vector3 targetLoc, float duration) {
			var startpos = weapon.position;
			weapon.position = Vector3.Lerp(startpos, targetLoc,  duration * Time.deltaTime);
			Debug.Log("Done");
	}
}