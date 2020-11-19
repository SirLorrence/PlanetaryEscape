using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float maxSpeed;

    private Vector3 startPos;

    private Vector3 playerVelocity;
    private float gravityValue = -9.81f;
    
    private Rigidbody rb;
    [SerializeField] GameObject playerModel;
    public Animator animator;

    //public Texture2D cursorTexture;
    //public CursorMode cursorMode = CursorMode.Auto;
    
    public PlayerStats playerStats;

    void Awake()
    {
        print("PlayerMovement");
    }
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        //Cursor.SetCursor(cursorTexture, new Vector2(cursorTexture.width/2f, cursorTexture.height/2f), cursorMode);
        rb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
    }
    
    public void Move(float Horizontal, float Vertical)
    {
        // Movement & Limiting the speed of the rigidbody
        float curSpeed = playerStats.MovementSpeed * Vertical;
        float leftSpeed = playerStats.MovementSpeed * Horizontal;
        rb.velocity = Vector3.ClampMagnitude(new Vector3(leftSpeed, rb.velocity.y, curSpeed), playerStats.MovementSpeed);

        //Sets the speed variable to allow for player movement animations to play
        animator.SetFloat("speed", Mathf.Abs(Horizontal) + Mathf.Abs(Vertical));

        //Rotates the playerModel to look at the mouse
        RaycastHit raycastHit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out raycastHit))
        {
            Vector3 playertoMouse = raycastHit.point - transform.position;
            playertoMouse.y = 0f;

            // Create a quaternion (rotation) based on looking down the vector from the player to the mouse.
            playerModel.transform.rotation = Quaternion.LookRotation(playertoMouse);
        }
    }

    public void ResetPosition()
    {
        gameObject.transform.position = startPos;
    }

}
