using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Player : MonoBehaviour
{
	public PlayerController playerController;
	public Vector3 currentRespawnPointPosition;
	public Quaternion currentRespawnPointRotation;

	[Header("Crosshair Settings")] public Texture2D crosshairImage;
	// void OnGUI()
	// {
	//     float xMin = (Screen.width / 2) - (crosshairImage.width / 2);
	//     float yMin = (Screen.height / 2) - (crosshairImage.height / 2);
	//     GUI.DrawTexture(new Rect(xMin, yMin, crosshairImage.width, crosshairImage.height), crosshairImage);
	// }

	// Start is called before the first frame update
	void Start() {
		currentRespawnPointPosition = gameObject.transform.position;
		currentRespawnPointRotation = gameObject.transform.rotation;
		float xMin = (Screen.width  / 2) - (crosshairImage.width  / 2);
		float yMin = (Screen.height / 2) - (crosshairImage.height / 2);
		//GUI.DrawTexture(new Rect(xMin, yMin, crosshairImage.width, crosshairImage.height), crosshairImage);
	}

	private void Update() {
		playerController.UpdatePlayer(
			Input.GetAxis("Horizontal"),
			Input.GetAxis("Vertical"),
			Input.GetKeyDown(KeyCode.Space),
			Input.GetKey(KeyCode.LeftControl),
			Input.GetKey(KeyCode.LeftShift),
			Input.GetKey(KeyCode.Mouse0),
			Input.GetKeyDown(KeyCode.LeftControl),
			Input.GetKeyUp(KeyCode.LeftControl),
			Input.GetKeyDown(KeyCode.R),
			Input.GetKey(KeyCode.Mouse1));
	}

	public void ReturnToCheckpoint() {
		gameObject.transform.position = currentRespawnPointPosition;
		gameObject.transform.rotation = currentRespawnPointRotation;
		gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
	}
}