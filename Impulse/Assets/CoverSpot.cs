using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class CoverSpot : MonoBehaviour
{
	private Color spotColor;

	public bool availableSpot;

	public bool badSpot;

	// Start is called before the first frame update
	private void OnTriggerStay(Collider other) {
		availableSpot = true;
		if (other.gameObject.CompareTag("Player"))
			badSpot = true;
	}

	private void OnTriggerExit(Collider other) {
		availableSpot = false;
		badSpot = false;
	}

	private void OnDrawGizmos() {
		Gizmos.color = (availableSpot) ? Color.red : Color.green;

		Gizmos.DrawWireSphere(transform.position, .25f);
	}
}