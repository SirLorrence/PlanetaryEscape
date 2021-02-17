using System;
using UnityEngine;
using Mirror;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : NetworkBehaviour
{
    #region Variables
    [Header("Player Information")]
    public GameObject playerGO;
    public Transform orientation;
    public CapsuleCollider capsuleCollider;
    public int playerNum = 0;
    public CameraFollow cameraFollow;
    public PlayerShoot playerShoot;
    public GameObject camera;

    [Header("Speed Settings")]
    public float moveSpeed = 4500;
    public float sprintMult = 1.5f;
    public float maxWalkSpeed = 10;
    public float maxSprintSpeed = 20;
    public float maxSpeed = 10;

    [Header("Speed Settings")]
    public float airSpeedMult = 0.25f;

    [Header("Friction Settings")]
    public float counterMovement = 0.10f;
    private float threshold = 0.01f;
    public float maxSlopeAngle = 35f;

    [Header("Layer Masks")]
    public LayerMask whatIsGround;
    public LayerMask whatIsWall;

    [Header("Jump Settings")]
    public float jumpCooldown = 0.25f;
    public float jumpForce = 550f;
    public float gravityForce = 20;
    public bool airJumps = true;
    public int amountOfAirJumps = 2;

    [Header("Crouch Settings")]
    public float slideForce = 400;
    public float slideCounterMovement = 0.2f;

    [Header("Wall Running Settings")] 
    public float wallRunForce;
    public float maxWallRunTime;
    public float maxWallSpeed;
    public bool isLeftWall;
    public bool isRightWall;
    public bool isWallRunning;
    public float maxCameraTilt;
    public float cameraTilt;


    //Physics
    private Rigidbody rb;
    private bool cancellingGrounded;

    //Scale
    private Vector3 crouchScale = new Vector3(1, 0.5f, 1);
    private Vector3 playerScale;

    //Jumping
    private bool readyToJump = true;
    private bool grounded;
    private int currentJumpsRemaining = 1;

    //Input
    private float x, y;
    private bool jumping, sprinting, crouching, shooting;

    //Sliding
    private Vector3 normalVector = Vector3.up;
    #endregion

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        playerScale = transform.localScale;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        currentJumpsRemaining = amountOfAirJumps;
        if (isLocalPlayer)
        {
            camera.SetActive(true);
        }
    }

    //Applies Movement 
    private void FixedUpdate()
    {
        //Movement();
    }

    // Gets User Input from the Input Manager
    public void Update()
    {
        if (isLocalPlayer)
        {

            this.x = Input.GetAxis("Horizontal");
            this.y = Input.GetAxis("Vertical");
            jumping = Input.GetKeyDown(KeyCode.Space);
            crouching = Input.GetKey(KeyCode.LeftControl);
            sprinting = Input.GetKey(KeyCode.LeftShift);
            shooting = Input.GetKey(KeyCode.Mouse0);

            //Crouching
            if (Input.GetKeyDown(KeyCode.LeftControl))
                StartCrouch();
            if (Input.GetKeyUp(KeyCode.LeftControl))
                StopCrouch();
            if (shooting)
                playerShoot.Shoot();

            cameraFollow.UpdateCamera();
            Movement();
        }
    }

    private void StartCrouch()
    {
        playerGO.transform.localScale = crouchScale;
        capsuleCollider.height = crouchScale.y * 2;
        playerGO.transform.position = new Vector3(playerGO.transform.position.x, playerGO.transform.position.y - 0.5f, playerGO.transform.position.z);

        if (grounded && rb.velocity.magnitude > 0.5f)
        {
            rb.AddForce(orientation.transform.forward * slideForce);
        }
    }

    private void StopCrouch()
    {
        playerGO.transform.localScale = playerScale;
        capsuleCollider.height = playerScale.y * 2;
        playerGO.transform.position = new Vector3(playerGO.transform.position.x, playerGO.transform.position.y + 0.5f, playerGO.transform.position.z);
    }

    private void Movement()
    {
        AddGravity();

        //Find actual velocity relative to where player is looking
        Vector2 mag = FindVelRelativeToLook();
        float xMag = mag.x, yMag = mag.y;

        ApplyFriction(x, y, mag);
        if (readyToJump && jumping) Jump();

        //Set Max Speed
        maxSpeed = sprinting && !shooting && x == 0 && y >= 0.9f? maxSprintSpeed : maxWalkSpeed;
        //maxSpeed = maxSprintSpeed;


        //If sliding down a ramp, add force down so player stays grounded and builds speed
        if (crouching && grounded && readyToJump)
        {
            rb.AddForce(Vector3.down * Time.deltaTime * 3000);
            return;
        }

        //If speed is larger than maxSpeed, overwrite the input
        if (x > 0 && xMag > maxSpeed) x = 0;
        if (x < 0 && xMag < -maxSpeed) x = 0;
        if (y > 0 && yMag > maxSpeed) y = 0;
        if (y < 0 && yMag < -maxSpeed) y = 0;

        //For Change In Movement
        float multiplier = 1f, multiplierV = 1f;

        // Reduce Movement in air
        if (!grounded)
        {
            multiplier = airSpeedMult;
            multiplierV = airSpeedMult;
        }
        //Increased Movement if Sprinting 
        if (grounded && sprinting && !crouching)
        {
            multiplierV = sprintMult;
        }
        // Movement while sliding
        if (grounded && crouching) multiplierV = 0f;

        //Apply forces to move player
        rb.AddForce(orientation.transform.forward * y * moveSpeed * Time.deltaTime * multiplier * multiplierV);
        rb.AddForce(orientation.transform.right * x * moveSpeed * Time.deltaTime * multiplier);
    }

    private void AddGravity()
    {
        rb.AddForce(Vector3.down * Time.deltaTime * gravityForce);
    }
    private void Jump()
    {
        if (grounded && readyToJump)
        {
            currentJumpsRemaining = amountOfAirJumps;
            //If jumping while falling, reset y velocity.
            Vector3 vel = rb.velocity;
            if (rb.velocity.y < 0.5f)
                rb.velocity = new Vector3(vel.x, 0, vel.z);
            else if (rb.velocity.y > 0)
                rb.velocity = new Vector3(vel.x, vel.y / 2, vel.z);

            //Add jump forces
            rb.AddForce(Vector2.up * jumpForce * 1.5f);
            rb.AddForce(normalVector * jumpForce * 0.5f);

            if (!airJumps)
            {
                readyToJump = false;
                Invoke(nameof(ResetJump), jumpCooldown);
            }
        }
        else if (airJumps && currentJumpsRemaining > 0)
        {
            --currentJumpsRemaining;

            //If jumping while falling, reset y velocity.
            Vector3 vel = rb.velocity;
            if (rb.velocity.y < 0.5f)
                rb.velocity = new Vector3(vel.x, 0, vel.z);
            else if (rb.velocity.y > 0)
                rb.velocity = new Vector3(vel.x, vel.y / 2, vel.z);

            //Add jump forces
            rb.AddForce(Vector2.up * jumpForce * 1.5f);
            rb.AddForce(normalVector * jumpForce * 0.5f);
        }
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    private void WaitForGrounded()
    {
        currentJumpsRemaining = amountOfAirJumps;
    }

    private void ApplyFriction(float x, float y, Vector2 mag)
    {
        if (!grounded || jumping) return;

        //Slow down sliding
        if (crouching)
        {
            rb.AddForce(moveSpeed * Time.deltaTime * -rb.velocity.normalized * slideCounterMovement);
            return;
        }

        //Counter movement
        if (Math.Abs(mag.x) > threshold && Math.Abs(x) < 0.05f || (mag.x < -threshold && x > 0) || (mag.x > threshold && x < 0))
        {
            rb.AddForce(moveSpeed * orientation.transform.right * Time.deltaTime * -mag.x * counterMovement);
        }
        if (Math.Abs(mag.y) > threshold && Math.Abs(y) < 0.05f || (mag.y < -threshold && y > 0) || (mag.y > threshold && y < 0))
        {
            rb.AddForce(moveSpeed * orientation.transform.forward * Time.deltaTime * -mag.y * counterMovement);
        }

        //Limit diagonal running. This will also cause a full stop if sliding fast and un-crouching, so not optimal.
        if (Mathf.Sqrt((Mathf.Pow(rb.velocity.x, 2) + Mathf.Pow(rb.velocity.z, 2))) > maxSpeed)
        {
            float fallSpeed = rb.velocity.y;
            Vector3 n = rb.velocity.normalized * maxSpeed;
            rb.velocity = new Vector3(n.x, fallSpeed, n.z);
        }
    }

    //Find the velocity relative to where the player is looking
    //Useful for vectors calculations regarding movement and limiting movement
    public Vector2 FindVelRelativeToLook()
    {
        float lookAngle = orientation.transform.eulerAngles.y;
        float moveAngle = Mathf.Atan2(rb.velocity.x, rb.velocity.z) * Mathf.Rad2Deg;

        float u = Mathf.DeltaAngle(lookAngle, moveAngle);
        float v = 90 - u;

        float magnitude = rb.velocity.magnitude;
        float yMag = magnitude * Mathf.Cos(u * Mathf.Deg2Rad);
        float xMag = magnitude * Mathf.Cos(v * Mathf.Deg2Rad);

        return new Vector2(xMag, yMag);
    }

    private bool IsFloor(Vector3 v) => Vector3.Angle(Vector3.up, v) < maxSlopeAngle;

    //Handle ground detection
    private void OnCollisionStay(Collision other)
    {
        //Make sure we are only checking for walkable layers
        int layer = other.gameObject.layer;
        if (whatIsGround != (whatIsGround | (1 << layer))) return;
        
        //Iterate through every collision in a physics update
        for (int i = 0; i < other.contactCount; i++)
        {
            Vector3 normal = other.contacts[i].normal;
            if (IsFloor(normal))
            {
                currentJumpsRemaining = amountOfAirJumps;
                grounded = true;
                cancellingGrounded = false;
                normalVector = normal;
                CancelInvoke(nameof(StopGrounded));
            }
        }
        

        //Invoke ground cancel, since we can't check normals with CollisionExit
        float delay = 3f;
        if (!cancellingGrounded)
        {
            cancellingGrounded = true;
            Invoke(nameof(StopGrounded), Time.deltaTime * delay);
        }

        //if (whatIsWall != (whatIsWall | (1 << layer)))
        //{
        //    if (Mathf.Abs(Vector3.Dot(other.GetContact(0).normal, Vector3.up)) < 0.1f)
        //    {

        //    }

        //}
    }

    private void StopGrounded()
    {
        grounded = false;
    }

}