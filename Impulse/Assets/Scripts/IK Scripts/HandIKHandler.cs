using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandIKHandler : MonoBehaviour
{
	public Transform gripRef, holdRef, weaponHolding;
	private void Update() {
		foreach (Transform w in transform) {
			if (w.gameObject.activeInHierarchy) {
				HandleIK(w);
				weaponHolding = w;
			}
		}
	}
	
	public void HandleIK(Transform weapon) {
		var grip = weapon.transform.Find("Grip");
		var holder = weapon.transform.Find("Holder");

		gripRef.position = grip.transform.position;
		gripRef.rotation = grip.transform.rotation;

		holdRef.position = holder.transform.position;
		holdRef.rotation = holder.transform.rotation;
	}
}