using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CoverBlock : MonoBehaviour
{
	// public GameObject[] coverPositions;
	//
	// public int maxCapacity;
	// public int occupied;
	// public bool unavailableCover;
	//
	// private void Start() {
	// 	List<GameObject> objects = new List<GameObject>();
	// 	foreach (Transform child in this.transform) {
	// 		objects.Add(child.gameObject);
	// 	}
	// 	coverPositions = objects.ToArray();
	// }
	//
	// private void LateUpdate() {
	// 	occupied = 0;
	// 	for (int i = 0; i < coverPositions.Length; i++) {
	// 		var spot = coverPositions[i].GetComponent<TestDamage>();
	// 		if (spot.availableSpot) occupied++;
	//
	// 		if (spot.badSpot) {
	// 			unavailableCover = true;
	// 			break;
	// 		}
	// 		else {
	// 			unavailableCover = false;
	// 		}
	// 	}
	// }
}