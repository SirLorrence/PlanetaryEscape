using System;
using UnityEngine;
using Mirror;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : GameEntity
{
	#region Fields

	[Header("Player Information")] public int playerNum = 0;
	public float sensitivityY, sensitivityX;
	public Transform camera;
	public Transform camPivot;
	public PlayerShoot playerShoot;
	public WeaponSelect weaponSelect;
	public BodyAnimation fullBodyAnimation; // animation ref
	public GameObject head;
	public GameObject body;
	public bool localDebug;

	[Header("Jump Settings")] public float jumpCooldown = 0.25f;
	public float jumpForce = 550f;
	public float gravityForce = 20;
	public LayerMask layerMask;
	[SerializeField] private bool isGrounded;


	[Header("Speed Settings")] public float setSpeed;
	public float maxWalkSpeed = 10;
	public float maxSprintSpeed = 20;
	public float maxCrouchSpeed = 10;
	private float airSpeed = 0.75f;

	//Input
	private PlayerActions playerActions;
	private float pitchRotation, yawRotation, camClamp = -60;
	public bool canJump, isSprinting, isCrouching, isShooting, ADS, inputShoot;
	private int wSwitch;

	//movement variables
	private Rigidbody rb;
	private Vector2 inputMovement;
	private Vector2 inputLook;

	//Sliding
	private Vector3 normalVector = Vector3.up;


	//collider variables
	private CapsuleCollider mCollider;
	private float colliderCenterScale, colliderHeight;
	private float crouchValue = 0.25f;

	public enum MovementAction
	{
		Walking,
		Crouching,
		Running,
	}

	public MovementAction movementAction;

	#endregion


	void Awake() {
		rb = GetComponent<Rigidbody>();
		if (localDebug) {
			playerActions = new PlayerActions();
			//movement input
			playerActions.PlayerControls.Move.performed += context => inputMovement = context.ReadValue<Vector2>();
			playerActions.PlayerControls.Move.canceled += context => inputMovement = Vector2.zero;
			playerActions.PlayerControls.Look.performed += context => inputLook = context.ReadValue<Vector2>();
			playerActions.PlayerControls.Look.canceled += context => inputLook = Vector2.zero;
			playerActions.PlayerControls.Sprint.performed += context => isSprinting = true;
			playerActions.PlayerControls.Sprint.canceled += context => isSprinting = false;
			playerActions.PlayerControls.Crouch.performed += context => isCrouching = !isCrouching;
			playerActions.PlayerControls.Jump.performed += context => Jump();
			//Weapon input
			playerActions.PlayerControls.Shoot.performed += context => inputShoot = true;
			playerActions.PlayerControls.Shoot.canceled += context => inputShoot = false;
			playerActions.PlayerControls.Reload.performed += context => playerShoot.Reload();
			playerActions.PlayerControls.SwitchWeapon.performed += context => ++wSwitch;
		}
	}

	public override void OnStartAuthority() {
		gameObject.GetComponent<NetworkAnimator>().enabled = false;

		playerActions = new PlayerActions();

		//movement input
		playerActions.PlayerControls.Move.performed += context => inputMovement = context.ReadValue<Vector2>();
		playerActions.PlayerControls.Move.canceled += context => inputMovement = Vector2.zero;
		playerActions.PlayerControls.Look.performed += context => inputLook = context.ReadValue<Vector2>();
		playerActions.PlayerControls.Look.canceled += context => inputLook = Vector2.zero;
		playerActions.PlayerControls.Sprint.performed += context => isSprinting = true;
		playerActions.PlayerControls.Sprint.canceled += context => isSprinting = false;
		playerActions.PlayerControls.Crouch.performed += context => isCrouching = !isCrouching;
		playerActions.PlayerControls.Jump.performed += context => Jump();
		//Weapon input
		playerActions.PlayerControls.Shoot.performed += context => inputShoot = true;
		playerActions.PlayerControls.Shoot.canceled += context => inputShoot = false;
		playerActions.PlayerControls.Reload.performed += context => playerShoot.Reload();
		playerActions.PlayerControls.SwitchWeapon.performed += context => ++wSwitch;

		base.OnStartAuthority();
	}


	void Start() {
		SetHealth(100);
		SetArmor(100);
		fullBodyAnimation = GetComponent<BodyAnimation>();
		weaponSelect = GetComponent<WeaponSelect>();

		mCollider = GetComponent<CapsuleCollider>();
		colliderCenterScale = mCollider.center.y;
		colliderHeight = mCollider.height;
		if (fullBodyAnimation == null) fullBodyAnimation = gameObject.AddComponent<BodyAnimation>();
	}

	private void FixedUpdate() {
		UpdatePlayer();
	}

	private void LateUpdate() {
		UpdateCamera();
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

	void UpdatePlayer() {
		Movement();
		WeaponSwitch();
		if (inputShoot) playerShoot.Shoot();
	}

	void UpdateCamera() {
		pitchRotation += inputLook.x * sensitivityY;
		yawRotation += inputLook.y   * sensitivityX;
		yawRotation = Mathf.Clamp(yawRotation, camClamp, Mathf.Abs(camClamp));
		gameObject.transform.localEulerAngles = new Vector3(0, pitchRotation, 0);
		//something is wrong with this line prevent the player to look up and down 
		// body.transform.localEulerAngles = new Vector3(0, pitchRotation, 0); 
		
		// If inverse positive Y
		camera.transform.localEulerAngles = new Vector3(-yawRotation, 0, 0);
		head.transform.localEulerAngles = new Vector3(-yawRotation, 0, 0);
	}

	void WeaponSwitch() {
		//for keyboard, can be used along side on the input system key
		if (Keyboard.current.digit1Key.isPressed) wSwitch = 0;
		if (Keyboard.current.digit2Key.isPressed) wSwitch = 1;

		var weaponSelected = (wSwitch % weaponSelect.weaponCount);

		switch (weaponSelected) {
			case 0:
				weaponSelect.WeaponSelected = 0;
				break;
			case 1:
				weaponSelect.WeaponSelected = 1;
				break;
		}
	}

	private void Movement() {
		AnimationHandler();
		AddGravity();
		SpeedHandler();
		CrouchColliderHandler();
		isGrounded = GroundCheck();

		// Reduce Movement in air
		if (!isGrounded) setSpeed *= airSpeed;

		//Increased Movement if Sprinting 
		if (isGrounded && isSprinting && !isCrouching && !ADS) movementAction = MovementAction.Running;

		// Sprint forward if the input is above the threshold 
		if (isSprinting) isSprinting = (inputMovement.y >= 0.7);

		// Movement while sliding
		if (isGrounded && isCrouching) movementAction = MovementAction.Crouching;

		if (isGrounded && !isCrouching && !isSprinting) movementAction = MovementAction.Walking;

		var movement = (inputMovement.x * transform.right + inputMovement.y * transform.forward).normalized;

		rb.MovePosition(transform.position + (movement * (setSpeed * Time.deltaTime)));
	}

	public void AnimationHandler() {
		fullBodyAnimation.MovementAnim(inputMovement.x, inputMovement.y);
		fullBodyAnimation.CrouchAnim(isCrouching);
		fullBodyAnimation.InAirAnim(isGrounded);
		fullBodyAnimation.SprintAnim((isGrounded && !ADS)
			? isSprinting
			: false); //cancels sprint while in air and aiming
		fullBodyAnimation.AimDownAnim(ADS);
		// fullBodyAnimation.ReloadAnim(reloa);
	}

	private void AddGravity() => rb.AddForce(Vector3.down * Time.deltaTime * gravityForce);
	private bool GroundCheck() => (Physics.CheckSphere(transform.position, .5f, layerMask));

	private void Jump() {
		if (isGrounded) {
			//Add jump forces
			rb.AddForce(Vector2.up   * jumpForce * 1.5f);
			rb.AddForce(normalVector * jumpForce * 0.5f);
		}
	}

	private void SpeedHandler() {
		switch (movementAction) {
			case MovementAction.Crouching:
				setSpeed = maxCrouchSpeed;
				break;
			case MovementAction.Walking:
				setSpeed = maxWalkSpeed;
				break;
			case MovementAction.Running:
				setSpeed = maxSprintSpeed;
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


	private void OnDrawGizmosSelected() {
		if (mCollider != null) Gizmos.DrawSphere(mCollider.gameObject.transform.position, .5f);
	}


	private void OnGUI() {
		GUILayout.BeginArea(new Rect(10, 10, 100, 250));
		GUILayout.Box("Stats");
		GUILayout.TextField("Armor: " + armor, 50);
		GUILayout.TextField("Heath: " + health, 50);
		GUILayout.TextField("Mag: "   + playerShoot.weapons[0].currentAmmoInMag, 100);
		GUILayout.TextField("Ammo: "  + playerShoot.weapons[0].reserveAmmo, 100);

		GUILayout.EndArea();
	}
}