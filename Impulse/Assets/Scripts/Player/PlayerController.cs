using System;
using UnityEngine;
using Mirror;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : NetworkBehaviour
{
	[Header("Player Information")] public int playerNum = 0;
	public float sensitivityY, sensitivityX;
	public Transform camera;
	public Transform camPivot;
	public PlayerShoot playerShoot;

	[Header("Speed Settings")] public float sprintMult = 1.5f;
	public float airSpeedMult = 0.25f;

	[Header("Friction Settings")] public float counterMovement = 0.10f;
	private float threshold = 0.01f;
	public float maxSlopeAngle = 35f;


	[Header("Jump Settings")] public float jumpCooldown = 0.25f;
	public float jumpForce = 550f;
	public float gravityForce = 20;
	public int amountOfAirJumps = 2;
	public LayerMask layerMask;

	[SerializeField] private bool readyToJump = true;
	[SerializeField] private bool grounded;


	[Header("Crouch Settings")] public float slideForce = 400;
	public float slideCounterMovement = 0.2f;

	[Header("Speed Settings")] public float moveSpeed = 5000;
	public float maxSpeed;
	public float maxWalkSpeed = 10;
	public float maxSprintSpeed = 20;
	public float maxCrouchSpeed = 10;

	//Physics
	// private bool cancellingGrounded;


	//Input
	// public PlayerInput playerInput;
	public PlayerActions playerActions;

	private float x, z, pitchRotation, yawRotation, camClamp = -60;
	public bool canJump, isSprinting, isCrouching, isShooting, ADS, inputReload, inputShoot;

	//Sliding
	private Vector3 normalVector = Vector3.up;

	//movement variables
	private Rigidbody rb;

	// private Vector3 movement;
	private Vector2 inputMovement;
	private Vector2 inputLook;

	//animation ref 
	public BodyAnimation fullBodyAnimation;

	//collider variables
	private CapsuleCollider collider;
	private float colliderCenterScale, colliderHeight;
	private float crouchValue = 0.25f;

	public enum MovementAction
	{
		Walking,
		Crouching,
		Running,
	}

	public MovementAction movementAction;


	void Awake() {
		rb = GetComponent<Rigidbody>();
		playerActions = new PlayerActions();     
	}

	void Start() {
		fullBodyAnimation = GetComponent<BodyAnimation>();
		collider = GetComponent<CapsuleCollider>();
		colliderCenterScale = collider.center.y;
		colliderHeight = collider.height;

		if (fullBodyAnimation == null) fullBodyAnimation = gameObject.AddComponent<BodyAnimation>();
	}

    // public void OnMovement(InputAction.CallbackContext value) {
    // 	Vector2 inputMovement = value.ReadValue<Vector2>();
    // 	movement = new Vector3(inputMovement.x, 0, inputMovement.y).normalized;
    // }

    public override void OnStartAuthority()
    {
		gameObject.GetComponent<NetworkAnimator>().enabled = false;

		playerActions.PlayerControls.Move.performed += ctx => inputMovement = ctx.ReadValue<Vector2>();
		playerActions.PlayerControls.Move.canceled += ctx => inputMovement = Vector2.zero;

		playerActions.PlayerControls.Look.performed += ctx => inputLook = ctx.ReadValue<Vector2>();
		playerActions.PlayerControls.Look.canceled += ctx => inputLook = Vector2.zero;

		playerActions.PlayerControls.Reload.performed += ctx => inputReload = ctx.ReadValue<bool>();
		playerActions.PlayerControls.Reload.performed += ctx => inputReload = false;

		base.OnStartAuthority();
    }

    private void FixedUpdate() {
		//Apply forces to move player
		Movement();
		Shoot();
		Reload();
	}

	private void Shoot()
    {
		if (Mouse.current.leftButton.isPressed) playerShoot.Shoot();
    }

	private void Reload()
	{
		if (Keyboard.current.rKey.isPressed) playerShoot.Reload();
	}

	private void LateUpdate() {
		pitchRotation += inputLook.x * sensitivityY;
		yawRotation += inputLook.y   * sensitivityX;
		yawRotation = Mathf.Clamp(yawRotation, camClamp, Mathf.Abs(camClamp));
		gameObject.transform.localEulerAngles = new Vector3(0, pitchRotation, 0);
		// If inverse positive Y
		camera.transform.localEulerAngles = new Vector3(-yawRotation, 0, 0);
	}

	public void AnimationHandler() {
		fullBodyAnimation.MovementAnim(inputMovement.x, inputMovement.y);
		fullBodyAnimation.CrouchAnim(isCrouching);
		fullBodyAnimation.InAirAnim(grounded);
		fullBodyAnimation.SprintAnim((grounded && !ADS) ? isSprinting : false); //cancels sprint while in air and aiming
		fullBodyAnimation.AimDownAnim(ADS);
		// fullBodyAnimation.ReloadAnim(reloa);
	}

	private void Movement() {
		//AnimationHandler();
		AddGravity();
		SpeedHandler();
		CrouchColliderHandler();
		grounded = GroundCheck();

		//Find actual velocity relative to where player is looking
		Vector2 mag = FindVelRelativeToLook();
		float xMag = mag.x, yMag = mag.y;

		ApplyFriction(x, z, mag);
		if (readyToJump && canJump) Jump();

		//If sliding down a ramp, add force down so player stays grounded and builds speed
		if (isCrouching && grounded && readyToJump) {
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

		// Reduce Movement in air
		if (!grounded) {
			multiplier = airSpeedMult;
			multiplierV = airSpeedMult;
		}

		//Increased Movement if Sprinting 
		if (grounded && isSprinting && !isCrouching && !ADS) {
			multiplierV = sprintMult;
		}

		// Movement while sliding
		if (grounded && isCrouching) multiplierV = 0f;

		//Apply forces to move player
		
		rb.AddForce(transform.forward * inputMovement.y * moveSpeed * Time.deltaTime * multiplier * multiplierV);
		rb.AddForce(transform.right   * inputMovement.x * moveSpeed * Time.deltaTime * multiplier);
	}


	private void AddGravity() => rb.AddForce(Vector3.down * Time.deltaTime * gravityForce);
	private bool GroundCheck() => (Physics.CheckSphere(transform.position, .5f, layerMask));

	private void Jump() {
		if (grounded && readyToJump) {
			//Add jump forces
			rb.AddForce(Vector2.up   * jumpForce * 1.5f);
			rb.AddForce(normalVector * jumpForce * 0.5f);
		}
	}

	private void ApplyFriction(float x, float y, Vector2 mag) {
		if (!grounded || canJump) return;

		//Slow down sliding
		if (isCrouching) {
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

	/// <summary>
	/// Find the velocity relative to where the player is looking
	/// Useful for vectors calculations regarding movement and limiting movement
	/// </summary>
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


	private void SpeedHandler() {
		switch (movementAction) {
			case MovementAction.Crouching:
				maxSpeed = maxCrouchSpeed;
				break;
			case MovementAction.Walking:
				maxSpeed = maxWalkSpeed;
				break;
			case MovementAction.Running:
				maxSpeed = maxSprintSpeed;
				break;
		}
	}

	private void CrouchColliderHandler() {
		if (isCrouching) {
			camera.localPosition = new Vector3(0, -.4f, 0);
			collider.center = new Vector3(0, colliderCenterScale - crouchValue, 0);
			collider.height = colliderHeight - (crouchValue * 2);
		}
		else {
			camera.localPosition = Vector3.zero;
			collider.center = new Vector3(0, colliderCenterScale, 0);
			collider.height = colliderHeight;
		}
	}

	private void OnEnable() {
		playerActions.PlayerControls.Enable();
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	private void OnDisable() {
		playerActions.PlayerControls.Disable();
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
	}

	private void OnDrawGizmosSelected() {
		if (collider != null) Gizmos.DrawSphere(collider.gameObject.transform.position, .5f);
	}
}