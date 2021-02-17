using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class PTest : MonoBehaviour
{
	[Header("Developer Values")] public float moveSpeed;
	public float maxSpeed;
	public Transform cam;
	public Transform camPivot;
	public bool testBool;
	[Range(0, 1)] public float rigWeight;
	public Rig slideLayer;

	public enum MovementAction
	{
		walking,
		crouching,
		running,
	}

	[Header("Friction Settings")] public float counterMovement = 0.10f;

	private float threshold = 0.01f;
	// public float maxSlopeAngle = 35f;


	public MovementAction movementAction;
	[Header("Player Input Values")] public float sensitivityX = 5f;
	public float sensitivityY = 5f;


	//movement variables
	private Rigidbody rb;
	private Vector3 movement;
	private float crouchSpeed;
	private float walkSpeed;
	private float runSpeed;


	//animation variables 
	private Animator anim;
	private bool crouchBool;
	[SerializeField] private bool slideBool;
	private static readonly int XInput = Animator.StringToHash("xInput");
	private static readonly int ZInput = Animator.StringToHash("zInput");
	private static readonly int IsCrouch = Animator.StringToHash("isCrouch");
	private static readonly int IsSliding = Animator.StringToHash("isSliding");
	private static readonly int IsRunning = Animator.StringToHash("isSprinting");

	//camera variables
	private float pitchRotation;
	private float yawRotation;
	private float maxX = 60;
	private float minX = -60;

	//collider variables
	private CapsuleCollider collider;
	private float colliderCenterScale;
	private float colliderHeight;
	private float crouchValue = 0.25f;


	float x;
	float z;


	// Start is called before the first frame update
	void Start() {
		collider = GetComponent<CapsuleCollider>();
		colliderCenterScale = collider.center.y;
		colliderHeight = collider.height;

		rb = GetComponent<Rigidbody>();

		anim = GetComponentInChildren<Animator>();
		if (anim == null) Debug.LogWarning("No animator found");
		else {
			crouchBool = anim.GetBool(IsCrouch);
			slideBool = anim.GetBool(IsSliding);
		}

		crouchSpeed = maxSpeed / 2;
		walkSpeed = maxSpeed;
		runSpeed = maxSpeed * 2;
	}

	// Update is called once per frame
	void Update() {
		pitchRotation += Input.GetAxisRaw("Mouse X") * sensitivityY;
		yawRotation += Input.GetAxisRaw("Mouse Y")   * sensitivityX;
		yawRotation = Mathf.Clamp(yawRotation, minX, maxX);
		
		x = Input.GetAxis("Horizontal");
		z = Input.GetAxis("Vertical");
		//
		// // movement = (x * transform.right + z * transform.forward).normalized;
		//
		//
		anim.SetFloat(XInput, x, 0.1f, Time.deltaTime);
		anim.SetFloat(ZInput, z, 0.1f, Time.deltaTime);

		// rb.velocity = (transform.forward * z * maxSpeed) + (transform.right * x * maxSpeed) +
		//               (transform.up      * rb.velocity.y);

		slideLayer.weight = rigWeight;

		if (Input.GetKeyDown(KeyCode.LeftControl)) {
			crouchBool = !crouchBool;
			anim.SetBool(IsCrouch, crouchBool);
		}

		}

		private void FixedUpdate() {
			rb.MovePosition(transform.position+(movement * maxSpeed * Time.deltaTime));
			 HandleMovement();
		
		}

		private void LateUpdate() {
			gameObject.transform.localEulerAngles = new Vector3(0, pitchRotation, 0);
			// if inverse positive Y
			cam.transform.localEulerAngles = new Vector3(-yawRotation, 0, 0);
			cam.position = camPivot.position;
		}

		private void HandleMovement() {
			SpeedHandler();
			CrouchMotionHandler();
		}


		void MovementHandler() {
			//gravity
			rb.AddForce(Vector3.down * Time.deltaTime * 20);

			rb.velocity = (transform.forward * z * maxSpeed) + (transform.right * x * maxSpeed) +
			              (transform.up      * rb.velocity.y);
		}

		private void SpeedHandler() {
			switch (movementAction) {
				case MovementAction.crouching:
					maxSpeed = crouchSpeed;
					break;
				case MovementAction.walking:
					maxSpeed = walkSpeed;
					break;
				case MovementAction.running:
					maxSpeed = runSpeed;
					break;
			}
		}

		void SliderHandler() {
			slideBool = !slideBool;
			rb.AddForce(transform.forward * 10, ForceMode.VelocityChange);
			// if (rb.velocity.sqrMagnitude < .01f && rb.angularVelocity.sqrMagnitude < 0.01f) slideBool = !slideBool;
			if (rb.velocity.magnitude < 0.5f) slideBool = !slideBool;
			anim.SetBool(IsSliding, slideBool);
		}

		private void CrouchMotionHandler() {
			if (crouchBool) movementAction = MovementAction.crouching;
			else movementAction = MovementAction.walking;

			CrouchColliderHandler();
		}


		private void CrouchColliderHandler() {
			if (crouchBool || slideBool) {
				collider.center = new Vector3(0, colliderCenterScale - crouchValue, 0);
				collider.height = colliderHeight - (crouchValue * 2);
			}
			else {
				collider.center = new Vector3(0, colliderCenterScale, 0);
				collider.height = colliderHeight;
			}
		}
	}