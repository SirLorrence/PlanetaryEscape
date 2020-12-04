using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 initPos;
    private Vector3 movement;
    private PlayerStats playerStats;

    // Start is called before the first frame update
    void Start()
    {
        initPos = transform.position;
        rb = GetComponent<Rigidbody>();
        playerStats = GetComponent<PlayerStats>();
    }

    // Update is called once per frame
    public void Move(float horizontal, float vertical)
    {
        var x = horizontal;
        var y = vertical;

        movement = (x * transform.right + y * transform.forward).normalized;
    }

    private void FixedUpdate()
    {
        rb.MovePosition(transform.position + (movement * (playerStats.MovementSpeed * Time.deltaTime)));
    }

    public void ResetPosition()
    {
        transform.position = initPos;
    }

}
