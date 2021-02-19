using UnityEngine;

public class WeaponADS : MonoBehaviour
{
	public bool toggleAimSights;
	public Transform Hip;
	public Transform Aim;
	public HandIKHandler weaponSelected;
	public float smooth;

	
	private void Update() {
		if (Input.GetMouseButton(1) || toggleAimSights) {
			LerpPosition(Aim.position, smooth);
		}
		else LerpPosition(Hip.position, smooth);
	}

	void LerpPosition(Vector3 targetLoc, float duration) {
			var holdingPosition = weaponSelected.weaponHolding.position;
			weaponSelected.weaponHolding.position = Vector3.Lerp(holdingPosition, targetLoc,  duration * Time.deltaTime);
	}
}