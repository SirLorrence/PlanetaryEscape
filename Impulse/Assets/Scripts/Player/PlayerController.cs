using System;
using UnityEngine;
using Mirror;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
	#region Fields

	[Header("Player Information")] public int playerNum = 0;
	public float sensitivityY, sensitivityX;
	public Transform camera;
	public Transform camPivot;
	public PlayerShoot playerShoot;

	[Header("Jump Settings")] public float jumpCooldown = 0.25f;
	public float jumpForce = 550f;
	public float gravityForce = 20;
	public int amountOfAirJumps = 1;
	public LayerMask layerMask;
	[SerializeField] private bool readyToJump = true;
	[SerializeField] private bool grounded;


	[Header("Speed Settings")] public float moveSpeed = 5000;
	public float maxSpeed;
	public float maxWalkSpeed = 10;
	public float maxSprintSpeed = 20;
	public float maxCrouchSpeed = 10;

	//Input
	private PlayerActions playerActions;
	private float pitchRotation, yawRotation, camClamp = -60;
	public bool canJump, isSprinting, isCrouching, isShooting, ADS, inputReload, inputShoot;

	//Sliding
	private Vector3 normalVector = Vector3.up;

	//movement variables
	private Rigidbody rb;

	private Vector2 inputMovement;
	private Vector2 inputLook;

	//animation ref 
	public BodyAnimation fullBodyAnimation;

	//collider variables
	private CapsuleCollider mCollider;
	private float colliderCenterScale, colliderHeight;
	private float crouchValue = 0.25f;

	public enum MovementAction
	{
		Walking,
		Crouching,
		Running,
		AirSpeed,
	}

	public MovementAction movementAction;

	#endregion


	void Awake() {
		rb = GetComponent<Rigidbody>();
		playerActions = new PlayerActions();
		playerActions.PlayerControls.Move.performed += ctx => inputMovement = ctx.ReadValue<Vector2>();
		playerActions.PlayerControls.Move.canceled += ctx => inputMovement = Vector2.zero;

		playerActions.PlayerControls.Look.performed += ctx => inputLook = ctx.ReadValue<Vector2>();
		playerActions.PlayerControls.Look.canceled += ctx => inputLook = Vector2.zero;

		playerActions.PlayerControls.Shoot.performed += ctx => inputShoot = true;
		playerActions.PlayerControls.Shoot.canceled += ctx => inputShoot = false;

		playerActions.PlayerControls.Reload.performed += ctx => playerShoot.Reload();

		playerActions.PlayerControls.Crouch.performed += context => isCrouching = !isCrouching;
		// playerActions.PlayerControls.Jump.performed += context => ;
	}

	void Start() {
		fullBodyAnimation = GetComponent<BodyAnimation>();
		mCollider = GetComponent<CapsuleCollider>();
		colliderCenterScale = mCollider.center.y;
		colliderHeight = mCollider.height;

		if (fullBodyAnimation == null) fullBodyAnimation = gameObject.AddComponent<BodyAnimation>();
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}


	private void FixedUpdate() {
		UpdatePlayer();
	}


	private void LateUpdate() {
		UpdateCamera();
	}

	void UpdatePlayer() {
		Movement();
		if (inputShoot) playerShoot.Shoot();
		if (inputReload) playerShoot.Reload();
	}

	void UpdateCamera() {
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
		AnimationHandler();
		AddGravity();
		SpeedHandler();
		CrouchColliderHandler();
		grounded = GroundCheck();

		// Find actual velocity relative to where player is looking
		// Vector2 mag = FindVelRelativeToLook();
		// float xMag = mag.x, yMag = mag.y;

		// ApplyFriction(x, z, mag);
		if (grounded && canJump) Jump();

		// Reduce Movement in air
		if (!grounded) movementAction = MovementAction.AirSpeed;

		//Increased Movement if Sprinting 
		if (grounded && isSprinting && !isCrouching && !ADS) movementAction = MovementAction.Running;

		// Movement while sliding
		if (grounded && isCrouching) movementAction = MovementAction.Crouching;

		var movement = (inputMovement.x * transform.right + inputMovement.y * transform.forward).normalized;

		rb.MovePosition(transform.position + (movement * (maxSpeed * Time.deltaTime)));
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
			case MovementAction.AirSpeed:
				maxSpeed = 0;
				break;
		}
	}

	private void CrouchColliderHandler() {
		if (isCrouching) {
			camera.localPosition = new Vector3(0, -.4f, 0);
			mCollider.center = new Vector3(0, colliderCenterScale - crouchValue, 0);
			mCollider.height = colliderHeight - (crouchValue * 2);
		}
		else {
			camera.localPosition = Vector3.zero;
			mCollider.center = new Vector3(0, colliderCenterScale, 0);
			mCollider.height = colliderHeight;
		}
	}

	private void OnEnable() {
		playerActions.PlayerControls.Enable();
	}

	private void OnDisable() {
		playerActions.PlayerControls.Disable();
	}

	private void OnDrawGizmosSelected() {
		if (mCollider != null) Gizmos.DrawSphere(mCollider.gameObject.transform.position, .5f);
	}
}