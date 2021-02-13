using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Unity.Mathematics;
using UnityEditor.IMGUI.Controls;

public class CameraFollow : MonoBehaviour
{
	// public Transform target;
	public float smooth = 5.0f;

	public float sensitivityX = 10f;
	public float sensitivityY = 10f;

	//Vertical Lock Degree
	[SerializeField] private float minX = -60f;
	[SerializeField] private float maxX = 60f;

	private float pitchRotation;
	private float yawRotation;

	private Vector3 positionOffset;
	public Transform Pivot;
	public Animator anim;

	public Transform cam;
	public Transform weaponSlot;

	[SerializeField] private Transform chest;
	[SerializeField] private Transform spine;
	[SerializeField] private Transform head;


	// private void Start() {
	// 	// positionOffset = gameObject.transform.position - Pivot.position;
	// 	if (anim != null) {
	// 		chest = anim.GetBoneTransform(HumanBodyBones.Chest);
	// 		spine = anim.GetBoneTransform(HumanBodyBones.Spine);
	// 		head = anim.GetBoneTransform(HumanBodyBones.Head);
	// 	}
	// }


	// private void Update() {
	//     UpdateCamera();
	// }

	public void UpdateCamera() {
		yawRotation += Input.GetAxisRaw("Mouse Y")   * sensitivityX;
		pitchRotation += Input.GetAxisRaw("Mouse X") * sensitivityY;

		yawRotation = Mathf.Clamp(yawRotation, minX, maxX);
		// pitchRotation = Mathf.Clamp(pitchRotation, -60, 60);

		Pivot.localEulerAngles = new Vector3(0, pitchRotation, 0);
		gameObject.transform.localEulerAngles = new Vector3(-yawRotation, pitchRotation, 0);
	}

	public void Update() {
		yawRotation += Input.GetAxisRaw("Mouse Y")   * sensitivityX;
		pitchRotation += Input.GetAxisRaw("Mouse X") * sensitivityY;
		yawRotation = Mathf.Clamp(yawRotation, minX, maxX);
	
		// //rotation of attached object
		// gameObject.transform.localEulerAngles = new Vector3(0, pitchRotation, 0);
		// //movement of camera
		// gameObject.transform.localEulerAngles = new Vector3(-yawRotation, 0, 0);
		//
		// chest.rotation *= Quaternion.Slerp(Quaternion.identity, gameObject.transform.localRotation, 0.25f);
		// spine.rotation *= Quaternion.Slerp(Quaternion.identity, gameObject.transform.localRotation, 0.25f);
	}

	private void LateUpdate() {
		// //rotation of attached object
		// gameObject.transform.localEulerAngles = new Vector3(0, pitchRotation, 0);
		// // //movement of camera
		cam.transform.localEulerAngles = new Vector3(-yawRotation, 0, 0);

		// chest.rotation *= Quaternion.Slerp(Quaternion.identity, transform.localRotation, 0.25f);
		// spine.rotation *= Quaternion.Slerp(Quaternion.identity, transform.localRotation, 0.25f);

		// chest.rotation *= Quaternion.Slerp(Quaternion.identity, cam.localRotation, 0.25f);
		// spine.rotation *= Quaternion.Slerp(Quaternion.identity, cam.localRotation, 0.25f);
		// weaponSlot.rotation = cam.rotation;
		// head.rotation = cam.rotation;
		cam.position = Pivot.position;
	}
}