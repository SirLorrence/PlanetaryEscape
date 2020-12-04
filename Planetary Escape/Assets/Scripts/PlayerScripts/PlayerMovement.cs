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
        
        //rb.velocity = Vector3.ClampMagnitude(new Vector3(leftSpeed, rb.velocity.y, curSpeed) * transform.forward.magnitude, playerStats.MovementSpeed);
        var move = new Vector3(leftSpeed, 0, curSpeed);
        rb.velocity =  playerModel.transform.forward.magnitude * move;

        //Sets the speed variable to allow for player movement animations to play
        animator.SetFloat("speed", Mathf.Abs(Horizontal) + Mathf.Abs(Vertical));
    }

    public void ResetPosition()
    {
        gameObject.transform.position = startPos;
    }

}
