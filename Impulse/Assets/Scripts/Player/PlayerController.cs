using System;
using UnityEngine;
using Mirror;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
	#region Variables

	[Header("Player Information")]
	// public GameObject playerGO;
	// public Transform orientation;
	// public CapsuleCollider capsuleCollider;
	public int playerNum = 0;

	public CameraFollow cameraFollow;
	public GameObject camera;

	[Header("Gun Info")] public GameObject gun;


	[Header("Speed Settings")] public float sprintMult = 1.5f;
	public float airSpeedMult = 0.25f;

	[Header("Friction Settings")] public float counterMovement = 0.10f;
	private float threshold = 0.01f;
	public float maxSlopeAngle = 35f;

	[Header("Layer Masks")] public LayerMask whatIsGround;
	public LayerMask whatIsWall;

	[Header("Jump Settings")] public float jumpCooldown = 0.25f;
	public float jumpForce = 550f;
	public float gravityForce = 20;
	public bool airJumps = true;
	public int amountOfAirJumps = 2;

	[Header("Crouch Settings")] public float slideForce = 400;
	public float slideCounterMovement = 0.2f;

	// [Header("Wall Running Settings")] public float wallRunForce;
	// public float maxWallRunTime;
	// public float maxWallSpeed;
	// public bool isLeftWall;
	// public bool isRightWall;
	// public bool isWallRunning;
	// public float maxCameraTilt;
	// public float cameraTilt;

	[Header("Speed Settings")] public float moveSpeed = 5000;
	public float maxSpeed;
	public float maxWalkSpeed = 10;
	public float maxSprintSpeed = 20;
	public float maxCrouchSpeed = 10;

	//Physics
	private bool cancellingGrounded;


	//Jumping
	private bool readyToJump = true;
	private bool grounded;
	private int currentJumpsRemaining = 1;

	//Input

	private float x, z, pitchRotation, yawRotation, sensitivityY, sensitivityX;
	private bool jumping, sprinting, crouching, shooting;

	//Sliding
	private Vector3 normalVector = Vector3.up;

	//movement variables
	private Rigidbody rb;
	private Vector3 movement;


	//animation variables 
	private Animator anim;
	private bool crouchBool;
	[SerializeField] private bool slideBool;
	private static readonly int XInput = Animator.StringToHash("xInput");
	private static readonly int ZInput = Animator.StringToHash("zInput");
	private static readonly int IsCrouch = Animator.StringToHash("isCrouch");
	private static readonly int IsSliding = Animator.StringToHash("isSliding");
	private static readonly int IsRunning = Animator.StringToHash("isSprinting");

	//collider variables
	private CapsuleCollider collider;
	private float colliderCenterScale, colliderHeight;
	private float crouchValue = 0.25f;


	public enum MovementAction
	{
		walking,
		crouching,
		running,
	}

	public MovementAction movementAction;

	#endregion

	void Awake() {
		rb = GetComponent<Rigidbody>();

		collider = GetComponent<CapsuleCollider>();
		colliderCenterScale = collider.center.y;
		colliderHeight = collider.height;

		anim = GetComponentInChildren<Animator>();
		if (anim == null) Debug.LogWarning("No animator found");
		else {
			crouchBool = anim.GetBool(IsCrouch);
			slideBool = anim.GetBool(IsSliding);
		}
	}

	void Start() {
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		currentJumpsRemaining = amountOfAirJumps;
	}

	// Gets User Input from the Input Manager
	public void UpdatePlayer(float x, float y, bool jumping, bool crouching, bool sprinting, bool shooting,
	                         bool startCrouch, bool stopCrouch, bool reload) {
		this.x = x;
		this.z = y;
		this.jumping = jumping;
		this.crouching = crouching;
		this.sprinting = sprinting;
		this.shooting = shooting;

		//Crouching
		if (startCrouch) StartCrouch();
		if (stopCrouch) StopCrouch();

		// if (shooting) GlobalShootingSystem.Instance.Shoot(gun, camera.transform.position, camera.transform.forward);
		// if (reload) GlobalShootingSystem.Instance.Reload(gun);

		Movement();
	}

	void AnimationActions() {
	}

	private void StartCrouch() {
		// playerGO.transform.localScale = crouchScale;
		// capsuleCollider.height = crouchScale.y * 2;
		// playerGO.transform.position = new Vector3(playerGO.transform.position.x, playerGO.transform.position.y - 0.5f,
		// 	playerGO.transform.position.z);
		crouching = true;

		if (grounded && rb.velocity.magnitude > 0.5f) {
			rb.AddForce(transform.forward * slideForce);
		}
	}

	private void StopCrouch() {
		crouching = false;
		// playerGO.transform.localScale = playerScale;
		// capsuleCollider.height = playerScale.y * 2;
		// playerGO.transform.position = new Vector3(playerGO.transform.position.x, playerGO.transform.position.y + 0.5f,
		// 	playerGO.transform.position.z);
	}

	private void Movement() {
		AddGravity();
		SpeedHandler();
		GroundCheck();
		CrouchColliderHandler();
		//Find actual velocity relative to where player is looking
		Vector2 mag = FindVelRelativeToLook();
		float xMag = mag.x, yMag = mag.y;

		ApplyFriction(x, z, mag);
		if (readyToJump && jumping) Jump();


		//If sliding down a ramp, add force down so player stays grounded and builds speed
		if (crouching && grounded && readyToJump) {
			rb.AddForce(Vector3.down * Time.deltaTime * 3000);
			return;
		}

		//If speed is larger than maxSpeed, overwrite the input
		if (x > 0 && xMag > maxSpeed) x = 0;
		if (x < 0 && xMag < -maxSpeed) x = 0;
		if (z > 0 && yMag > maxSpeed) z = 0;
		if (z < 0 && yMag < -maxSpeed) z = 0;

		//For Change In Movement
		float multiplier = 1f, multiplierV = 1f;

		// // Reduce Movement in air
		if (!grounded) {
			multiplier = airSpeedMult;
			multiplierV = airSpeedMult;
		}

		//Increased Movement if Sprinting 
		if (grounded && sprinting && !crouching) {
			multiplierV = sprintMult;
		}

		// Movement while sliding
		if (grounded && crouching) multiplierV = 0f;

		//Apply forces to move player
		rb.AddForce(transform.forward * z * moveSpeed * Time.deltaTime * multiplier * multiplierV);
		rb.AddForce(transform.right   * x * moveSpeed * Time.deltaTime * multiplier);
	}

	private void AddGravity() {
		rb.AddForce(Vector3.down * Time.deltaTime * gravityForce);
	}

	private void Jump() {
		if (grounded && readyToJump) {
			currentJumpsRemaining = amountOfAirJumps;
			//If jumping while falling, reset y velocity.
			Vector3 vel = rb.velocity;
			if (rb.velocity.y < 0.5f)
				rb.velocity = new Vector3(vel.x, 0, vel.z);
			else if (rb.velocity.y > 0)
				rb.velocity = new Vector3(vel.x, vel.y / 2, vel.z);

			//Add jump forces
			rb.AddForce(Vector2.up   * jumpForce * 1.5f);
			rb.AddForce(normalVector * jumpForce * 0.5f);

			if (!airJumps) {
				readyToJump = false;
				Invoke(nameof(ResetJump), jumpCooldown);
			}
		}
		else if (airJumps && currentJumpsRemaining > 0) {
			--currentJumpsRemaining;

			//If jumping while falling, reset y velocity.
			Vector3 vel = rb.velocity;
			if (rb.velocity.y < 0.5f)
				rb.velocity = new Vector3(vel.x, 0, vel.z);
			else if (rb.velocity.y > 0)
				rb.velocity = new Vector3(vel.x, vel.y / 2, vel.z);

			//Add jump forces
			rb.AddForce(Vector2.up   * jumpForce * 1.5f);
			rb.AddForce(normalVector * jumpForce * 0.5f);
		}
	}

	private void ResetJump() {
		readyToJump = true;
	}

	private void WaitForGrounded() {
		currentJumpsRemaining = amountOfAirJumps;
	}

	private void ApplyFriction(float x, float y, Vector2 mag) {
		if (!grounded || jumping) return;

		//Slow down sliding
		if (crouching) {
			rb.AddForce(moveSpeed * Time.deltaTime * -rb.velocity.normalized * slideCounterMovement);
			return;
		}

		//Counter movement
		if (Math.Abs(mag.x) > threshold && Math.Abs(x) < 0.05f || (mag.x < -threshold && x > 0) ||
		    (mag.x                                                       > threshold  && x < 0)) {
			rb.AddForce(moveSpeed * transform.right * Time.deltaTime * -mag.x * counterMovement);
		}

		if (Math.Abs(mag.y) > threshold && Math.Abs(y) < 0.05f || (mag.y < -threshold && y > 0) ||
		    (mag.y                                                       > threshold  && y < 0)) {
			rb.AddForce(moveSpeed * transform.forward * Time.deltaTime * -mag.y * counterMovement);
		}

		//Limit diagonal running. This will also cause a full stop if sliding fast and un-crouching, so not optimal.
		if (Mathf.Sqrt((Mathf.Pow(rb.velocity.x, 2) + Mathf.Pow(rb.velocity.z, 2))) > maxSpeed) {
			float fallSpeed = rb.velocity.y;
			Vector3 n = rb.velocity.normalized * maxSpeed;
			rb.velocity = new Vector3(n.x, fallSpeed, n.z);
		}
	}

	//Find the velocity relative to where the player is looking
	//Useful for vectors calculations regarding movement and limiting movement
	public Vector2 FindVelRelativeToLook() {
		float lookAngle = transform.eulerAngles.y;
		float moveAngle = Mathf.Atan2(rb.velocity.x, rb.velocity.z) * Mathf.Rad2Deg;

		float u = Mathf.DeltaAngle(lookAngle, moveAngle);
		float v = 90 - u;

		float magnitude = rb.velocity.magnitude;
		float yMag = magnitude * Mathf.Cos(u * Mathf.Deg2Rad);
		float xMag = magnitude * Mathf.Cos(v * Mathf.Deg2Rad);

		return new Vector2(xMag, yMag);
	}

	private bool IsFloor(Vector3 v) => Vector3.Angle(Vector3.up, v) < maxSlopeAngle;

	void GroundCheck() {
		grounded = Physics.CheckSphere(transform.position, .5f);
	}

//Handle ground detection

	// private void OnCollisionStay(Collision other) {
	// 	//Make sure we are only checking for walkable layers
	// 	int layer = other.gameObject.layer;
	// 	if (whatIsGround != (whatIsGround | (1 << layer))) return;
	//
	// 	//Iterate through every collision in a physics update
	// 	for (int i = 0; i < other.contactCount; i++) {
	// 		Vector3 normal = other.contacts[i].normal;
	// 		if (IsFloor(normal)) {
	// 			currentJumpsRemaining = amountOfAirJumps;
	// 			grounded = true;
	// 			cancellingGrounded = false;
	// 			normalVector = normal;
	// 			CancelInvoke(nameof(StopGrounded));
	// 		}
	// 	}
	//
	//
	// 	//Invoke ground cancel, since we can't check normals with CollisionExit
	// 	float delay = 3f;
	// 	if (!cancellingGrounded) {
	// 		cancellingGrounded = true;
	// 		Invoke(nameof(StopGrounded), Time.deltaTime * delay);
	// 	}
	// }

	private void StopGrounded() {
		grounded = false;
	}


	private void SpeedHandler() {
		switch (movementAction) {
			case MovementAction.crouching:
				maxSpeed = maxCrouchSpeed;
				break;
			case MovementAction.walking:
				maxSpeed = maxWalkSpeed;
				break;
			case MovementAction.running:
				maxSpeed = maxSprintSpeed;
				break;
		}
	}


	private void CrouchColliderHandler() {
		if (crouching) {
			collider.center = new Vector3(0, colliderCenterScale - crouchValue, 0);
			collider.height = colliderHeight - (crouchValue * 2);
		}
		else {
			collider.center = new Vector3(0, colliderCenterScale, 0);
			collider.height = colliderHeight;
		}
	}

	private void OnDrawGizmosSelected() {
		Gizmos.DrawSphere(transform.position, .5f);
	}
}