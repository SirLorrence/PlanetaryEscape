using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardFX : MonoBehaviour
{
	/*
	 =============================================
	 Billboard effect will display the UI element that
	 is on, to always be shown in the direction of the player.
	  The object must be on a canvas that is rendering to world space
	 ============================================= 
	 */
	public Camera camera;
	private Quaternion originalRotation;
	void Start() => originalRotation = transform.rotation;
	private void LateUpdate() {
		transform.rotation = camera.transform.rotation * originalRotation;
	}
}