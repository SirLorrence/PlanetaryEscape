using Mirror;
using UnityEngine;

public class InputManager : NetworkBehaviour
{
    [Header("Required References")]
    public PlayerController playerController;
    public PlayerShoot playerShoot;
    public CameraFollow cameraFollow;
    public Camera cam;

    private bool pause;
    private bool freeze = false;
    private float time = 0;

    void Start()
    {
        pause = false;
    }

    private void Update()
    {
        if (this.isLocalPlayer)
        {
            if (freeze)
                return;

            CheckForPause();
            Shoot();
            PlayerMove();
            UpdateCamera();
        }
    }

    #region Input

    private void CheckForPause()
    {

    }

    private void PlayerMove()
    {
        playerController.GetInput(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical"),
            Input.GetKey(KeyCode.LeftShift),
            Input.GetKeyDown(KeyCode.Space),
            Input.GetKey(KeyCode.LeftControl),
            Input.GetKeyDown(KeyCode.LeftControl),
            Input.GetKeyUp(KeyCode.LeftControl));
    }
    private void Shoot()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            playerShoot.Shoot();
        }
    }

    private void Interact()
    {
        if (Input.GetKey(KeyCode.Mouse1))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100))
            {
                Interactable interactable = hit.collider.GetComponent<Interactable>();
                if (interactable != null)
                {
                    
                }
            }
        }
    }

    private void UpdateCamera()
    {
        cameraFollow.CameraMove(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
    }

    #endregion

    public void UnfreezeInput() => freeze = false;
    public void FreezeInput() => freeze = true;
}
