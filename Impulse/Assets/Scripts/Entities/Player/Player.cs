using System.Collections;
using UnityEngine;
using Managers;

namespace Entities.Player
{
	[RequireComponent(typeof(PlayerAnimationHandler), typeof(PlayerShoot))]
	[RequireComponent(typeof(CapsuleCollider))]
	public class Player : GameEntity
	{
		#region Fields

		[Header("Player Information")] public Vector2 sensitivity;
		[SerializeField] private Transform _cameraTransform;

		[Header("Jump Settings")] [SerializeField]
		private float jumpCooldown = 0.25f;

		[SerializeField] private float jumpForce = 550f;
		[SerializeField] private float gravityForce = 20;
		[SerializeField] private LayerMask layerMask;
		[SerializeField] private bool isGrounded;

		[Header("Speed Settings")] [SerializeField]
		private float maxWalkSpeed = 10;

		[SerializeField] private float maxSprintSpeed = 20;
		[SerializeField] private float maxCrouchSpeed = 10;
		[ReadOnly] [SerializeField] private float currentSpeed;
		[ReadOnly] [SerializeField] private MovementAction movementAction;

		private enum MovementAction
		{
			Walking,
			Crouching,
			Running,
		}

		private float airSpeed = 0.75f;

		//Input
		private PlayerActions playerActions;
		private float pitchRotation, yawRotation, camClamp = -60;

		[ReadOnly] [SerializeField]
		private bool canJump, isSprinting, isCrouching, canFire, ADS, inputShoot, isReloading;

		private int _weaponSelected;
		private int _currentWeapon;

		[SerializeField] private bool reloadEnabled;

		//movement variables
		private Vector2 inputMovement;
		private Vector2 inputLook;

		//Sliding
		private Vector3 normalVector = Vector3.up;

		//collider variables
		private CapsuleCollider _Collider;
		private float colliderCenterScale, colliderHeight;
		private float crouchValue = 0.25f;

		//references 
		private PlayerShoot _playerShoot;
		private PlayerAnimationHandler _animationHandler;
		private WaitForSeconds waitTime;

		public PlayerShoot PlayerShoot {
			get => _playerShoot;
			set => _playerShoot = value;
		}

		//Sound Garbage
		private float footstepTimer = 0;

		#endregion

		#region On Start Up

		protected override void Awake() {
			_animationHandler = GetComponent<PlayerAnimationHandler>();
			_playerShoot = GetComponent<PlayerShoot>();
			_Collider = GetComponent<CapsuleCollider>();
			playerActions = new PlayerActions();
			InputActionsCall();
			base.Awake();
		}

		void Start() {
			colliderCenterScale = _Collider.center.y;
			colliderHeight = _Collider.height;
		}

		private void OnEnable() {
			playerActions.PlayerControls.Enable();
			playerActions.UI.Disable();
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}

		private void InputActionsCall() {
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

			playerActions.PlayerControls.Reload.performed += context => ReloadHandler();
			// playerActions.PlayerControls.Reload.performed += context => isReloading = true;
			// playerActions.PlayerControls.SwitchWeapon.performed += context => ++_weaponSelected;
			playerActions.PlayerControls.Aim.performed += context => ADS = true;
			playerActions.PlayerControls.Aim.canceled += context => ADS = false;

			playerActions.PlayerControls.TogglePause.performed += context => EnablePauseMenuControls();
			playerActions.UI.TogglePause.performed += context => EnableGameplayControls();
		}

		void EnableGameplayControls() {
			GameManager.Instance.TogglePauseMenu(false);
			playerActions.PlayerControls.Enable();
			playerActions.UI.Disable();
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}

		void EnablePauseMenuControls() {
			print("Pause Triggered");
			GameManager.Instance.TogglePauseMenu(true);
			playerActions.PlayerControls.Disable();
			playerActions.UI.Enable();
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}

		#endregion

		#region Update Information

		private void FixedUpdate() {
			UpdatePlayer();
		}

		private void LateUpdate() {
			UpdateCamera();
		}

		void UpdatePlayer() {
			Movement();
			AnimationHandler();
			WeaponHandler();
		}

		#endregion

		#region Movement & Camera Management

		private void Movement() {
			AddGravity();
			SpeedHandler();
			CrouchColliderHandler();
			isGrounded = GroundCheck();
			entityPosition = transform.position;

			// Reduce Movement in air
			if (!isGrounded) currentSpeed *= airSpeed;

			//Increased Movement if Sprinting 
			if (isGrounded && isSprinting && !isCrouching && !ADS) movementAction = MovementAction.Running;

			// Sprint forward if the input is above the threshold 
			if (isSprinting) isSprinting = (inputMovement.y >= 0.7);

			// Movement while sliding
			if (isGrounded && isCrouching) movementAction = MovementAction.Crouching;

			if (isGrounded && !isCrouching && !isSprinting) movementAction = MovementAction.Walking;

			var movement = (inputMovement.x * transform.right + inputMovement.y * transform.forward).normalized;

			rb.MovePosition(entityPosition + (movement * (currentSpeed * Time.deltaTime)));

			if (footstepTimer > 0)
				footstepTimer -= Time.deltaTime;
			else if (Mathf.Abs(inputMovement.x) + Mathf.Abs(inputMovement.y) > 0.1) {
				SoundManager.Instance.PlayAudio(AudioTypes.SFX_FOOTSTEP1);
				footstepTimer = 0.5f;
			}
			else {
				SoundManager.Instance.StopAudio(AudioTypes.SFX_FOOTSTEP1);
			}
		}

		private void AddGravity() => rb.AddForce(Vector3.down * (Time.deltaTime * gravityForce));
		private bool GroundCheck() => (Physics.CheckSphere(entityPosition, .5f, layerMask));

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
					currentSpeed = maxCrouchSpeed;
					break;
				case MovementAction.Walking:
					currentSpeed = maxWalkSpeed;
					break;
				case MovementAction.Running:
					currentSpeed = maxSprintSpeed;
					break;
			}
		}

		private void CrouchColliderHandler() {
			if (isCrouching) {
				_cameraTransform.localPosition = new Vector3(0, -.4f, 0);
				_Collider.center = new Vector3(0, colliderCenterScale - crouchValue, 0);
				_Collider.height = colliderHeight - (crouchValue * 2);
			}
			else {
				_cameraTransform.localPosition = Vector3.zero;
				_Collider.center = new Vector3(0, colliderCenterScale, 0);
				_Collider.height = colliderHeight;
			}
		}

		void UpdateCamera() {
			float aimSpeed = (ADS) ? 2 : 1f;

			var sensitivityY = sensitivity.y / aimSpeed;
			var sensitivityX = sensitivity.x / aimSpeed;

			pitchRotation += inputLook.x * sensitivityY;
			yawRotation += inputLook.y   * sensitivityX;

			yawRotation = Mathf.Clamp(yawRotation, camClamp, Mathf.Abs(camClamp));
			gameObject.transform.localEulerAngles = new Vector3(0, pitchRotation, 0);
			// If inverse positive Y
			_cameraTransform.transform.localEulerAngles = new Vector3(-yawRotation, 0, 0);
		}

		#endregion

		#region Weapon Management

		void ReloadHandler() {
			if (!isReloading && !_playerShoot.AmmoCheck()) {
				SoundManager.Instance.PlayAudio(AudioTypes.SFX_RIFLE_RELOAD);
				StartCoroutine(ReloadCall());
			}
			else if (_playerShoot.AmmoCheck()) {
				//no ammo 
			}
		}


		IEnumerator ReloadCall() {
			isReloading = true;
			_animationHandler.ReloadAnim(out waitTime);
			print("Reload");
			yield return waitTime;
			_playerShoot.Reload();
			isReloading = false;
			yield return null;
		}

		void WeaponHandler() {
			if (inputShoot) {
				_playerShoot.Shoot(out canFire);
				if (canFire) {
					_animationHandler.ShootingAnim(canFire);
					canFire = false;
				}
			}

			// //for keyboard, can be used along side on the input system key
			// if (Keyboard.current.digit1Key.isPressed)
			// 	_weaponSelected = 0;
			// if (Keyboard.current.digit2Key.isPressed) _weaponSelected = 1;
			// var weaponSelected = (_weaponSelected % _playerShoot.weapons.Count);
			// if (weaponSelected != _currentWeapon) {
			// 	switch (weaponSelected) {
			// 		case 0:
			// 			_playerShoot.StartCoroutine(_playerShoot.SetWeapon());
			// 			break;
			// 		case 1:
			// 			_playerShoot.StartCoroutine(_playerShoot.SetWeapon(1));
			// 			break;
			// 	}
			//
			// 	_currentWeapon = weaponSelected;
			// }
		}

		#endregion

		#region Animation Management

		public void AnimationHandler() {
			_animationHandler.MovementAnim(inputMovement.x, inputMovement.y);
			_animationHandler.SprintAnim((isGrounded && !ADS)
				? isSprinting
				: false); //cancels sprint while in air and aiming
			_animationHandler.AimDownAnim(ADS);
			// _animationHandler.ShootingAnim(inputShoot);
		}

        #endregion

        #region Health 
        public override void TakeDamage(int damage)
        {
	        SoundManager.Instance.PlayAudio(AudioTypes.SFX_PLAYER_TAKING_DAMAGE);
            base.TakeDamage(damage);

            if (health <= 0)
				GameManager.Instance.playerDeath?.Invoke();
        }
        #endregion

        private void OnDisable() {
			playerActions.PlayerControls.Disable();
			playerActions.UI.Disable();
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}

		private void OnDrawGizmosSelected() {
			if (_Collider != null) Gizmos.DrawSphere(_Collider.gameObject.transform.position, .5f);
		}

		private void OnGUI() {
			GUILayout.BeginArea(new Rect(10, 10, 100, 250));
			GUILayout.Box("Stats");
			GUILayout.TextField("Armor: " + armor, 50);
			GUILayout.TextField("Heath: " + health, 50);
			GUILayout.TextField("Mag: "   + _playerShoot.currentWeapon.weaponInfo.currentAmmoInMag, 100);
			GUILayout.TextField("Ammo: "  + _playerShoot.currentWeapon.weaponInfo.reserveAmmo, 100);
			GUILayout.EndArea();
		}
	}
}