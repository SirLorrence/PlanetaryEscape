using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetRef : MonoBehaviour
{
	public GameObject weaponsGameObject;
	public Transform grip;

	private void Start() {
		grip = weaponsGameObject.transform.Find("Grip");
	}

	private void Update() {
		transform.position = grip.position;
		transform.rotation = grip.rotation;

	}
}