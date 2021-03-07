using System;
using UnityEngine;


public class AttackHandler : MonoBehaviour
{
	public bool zoneOccupied;
	private void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Player")) zoneOccupied = true;
	}

	private void OnTriggerExit(Collider other) {
		if (other.CompareTag("Player")) zoneOccupied = false;
	}
}