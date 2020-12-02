using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;

    private Vector3 movement;
    private PlayerStats playerStats;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerStats = GetComponent<PlayerStats>();
    }

    // Update is called once per frame
    void Update()
    {
        var x = Input.GetAxisRaw("Horizontal");
        var y = Input.GetAxisRaw("Vertical");

        movement = (x * transform.right + y * transform.forward).normalized;
    }

    private void FixedUpdate()
    {
        rb.MovePosition(transform.position + (movement * (playerStats.MovementSpeed * Time.deltaTime)));
    }
}
