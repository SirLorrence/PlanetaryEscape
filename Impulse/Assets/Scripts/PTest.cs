using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PTest : MonoBehaviour
{
	public float moveSpeed;
	public float sensitivityX = 5f;
	public float sensitivityY = 5f;
	public Transform cam;
	public Transform camPivot;


	private Rigidbody rb;
	private Animator anim;
	private Vector3 movement;
	private float pitchRotation;
	private float yawRotation;
	private float maxX = 60;
	private float minX = -60;

	// Start is called before the first frame update
	void Start() {
		rb = GetComponent<Rigidbody>();
		if (rb == null) {
			Debug.LogError("No rigidbody");
		}

		anim = GetComponentInChildren<Animator>();
		if (anim == null) Debug.LogError("No animator found");
	}

	// Update is called once per frame
	void Update() {
		pitchRotation += Input.GetAxisRaw("Mouse X") * sensitivityY;
		yawRotation += Input.GetAxisRaw("Mouse Y")   * sensitivityX;
		yawRotation = Mathf.Clamp(yawRotation, minX, maxX);


		float x = Input.GetAxis("Horizontal");
		float z = Input.GetAxis("Vertical");
		movement = (x * transform.right + z * transform.forward).normalized;

		anim.SetFloat("xInput", x, 0.1f, Time.deltaTime);
		anim.SetFloat("zInput", z, 0.1f, Time.deltaTime);
	}

	private void FixedUpdate() {
		rb.MovePosition(transform.position + (movement * moveSpeed * Time.deltaTime));
		if (Input.GetKeyDown(KeyCode.LeftControl)) {
			var value = anim.GetBool("isCrouch");
			value = !value;
			anim.SetBool("isCrouch", value);
		}
	}

	private void LateUpdate() {
		gameObject.transform.localEulerAngles = new Vector3(0, pitchRotation, 0);
		// if inverse positive Y
		cam.transform.localEulerAngles = new Vector3(-yawRotation, 0, 0);
		cam.position = camPivot.position;
	}
}