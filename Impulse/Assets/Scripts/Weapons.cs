using System;
using System.Collections;
using Enemy.Enemy_Types;
using UnityEngine;
public enum WeaponType
{
	Pistol,
	AR,
	Shotgun,
	Turret,
}

public class Weapons : MonoBehaviour
{
	public Transform barrelTip;
	public WeaponType wType;

	protected int gunDamage;
	protected float rateOfFire;
	protected float weaponRange;
	protected float hitForce;

	public GameObject cube;

	public Transform lineOfSight;

	public bool isTurret;

	[SerializeField] private WaitForSeconds shotDuration = new WaitForSeconds(0.1f);
	private float nextFire;

	public LineRenderer line; //visual fire-line
	public bool testFire;

	private void OnEnable() {
		line = GetComponent<LineRenderer>();
		WeaponStats();
	}

	private void WeaponStats() {
		switch (wType) {
			case WeaponType.Pistol:
				gunDamage = 5;
				rateOfFire = 5;
				weaponRange = 5;
				hitForce = 5;
				isTurret = false;
				break;
			case WeaponType.AR:
				gunDamage = 15;
				rateOfFire = .5f;
				weaponRange = 15;
				hitForce = 15;
				isTurret = false;
				break;
			case WeaponType.Shotgun:
				gunDamage = 10;
				rateOfFire = 10;
				weaponRange = 10;
				hitForce = 10;
				isTurret = false;
				break;
			case WeaponType.Turret:
				gunDamage = 10;
				rateOfFire = 0.15f;
				weaponRange = 10;
				hitForce = 10;
				isTurret = true;
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}
	}


	public void FireWeapon() {
		if (Time.time > nextFire) {
			Debug.Log("Is Shooting");

			nextFire = Time.time + rateOfFire;

			StartCoroutine(Shoot());

			Vector3 rayOrigin =
				new Vector3(lineOfSight.position.x, lineOfSight.position.y, lineOfSight.position.z); // euler angle
			RaycastHit hit;

			if (isTurret) {
				line.SetPosition(0, barrelTip.position);
				
				var hitRegister = (Physics.Raycast(rayOrigin, lineOfSight.forward * weaponRange, out hit, weaponRange)
					? hit.point
					: rayOrigin + (lineOfSight.forward * weaponRange));
				
				line.SetPosition(1, hitRegister);
			}

			if (Physics.Raycast(rayOrigin, lineOfSight.forward, out hit, weaponRange)) {
				// add the damage here
				Debug.Log(hit.collider.name);
				// if (hit.rigidbody != null) {
				// 	hit.rigidbody.AddForce(-hit.normal * hitForce);
				// }
			}
		}
	}

	IEnumerator Shoot() {
		Debug.Log("Coroutine Stated");
		// intercept object 
		line.enabled = true;
		yield return shotDuration;
		line.enabled = false;
	}


	private void OnDrawGizmosSelected() {
		if (lineOfSight != null) {
			var origin = new Vector3(lineOfSight.position.x, lineOfSight.position.y, lineOfSight.position.z);
			Debug.DrawRay(origin, lineOfSight.forward * weaponRange, Color.magenta);
		}
	}
}