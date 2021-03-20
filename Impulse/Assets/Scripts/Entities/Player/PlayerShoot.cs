using System;
using System.Collections;
using System.Collections.Generic;
using Entities.Enemy;
using UnityEngine;
using Weapons;

namespace Entities.Player
{
	public class PlayerShoot : MonoBehaviour
	{
		[Header("Assignable")] public Transform cameraTransform;
		public Transform gunTip;
		public GameObject[] weaponHolders;
		public List<Weapon> weapons;
		[ReadOnly] public Weapon currentWeapon;

		[Header("Cross-hair Settings")] public Texture2D crossHairImage;
		private float timer;

		private void Awake() {
			weaponHolders = GameObject.FindGameObjectsWithTag("Arms");
			foreach (var gun in weaponHolders) {
				var weapon = gun.GetComponentInChildren<Weapon>();
				weapons.Add(weapon);
			}
		}

		void OnEnable() {
			timer = 0;
			StartCoroutine(SetWeapon());
		}

		public IEnumerator SetWeapon(int num = default) {
			for (int i = 0; i < weapons.Count; i++) {
				if (i == num) {
					weaponHolders[i].gameObject.SetActive(true);
					currentWeapon = weapons[i];
				}
				else {
					weaponHolders[i].gameObject.SetActive(false);
				}
			}

			yield return null;
		}

		public void Shoot(out bool canFireValue) {
			var gun = currentWeapon.weaponInfo;
			bool boolValue = false;

			if (timer < Time.realtimeSinceStartup) {
				boolValue = true;
				if (gun.currentAmmoInMag > 0) {
					gun.currentAmmoInMag--;

					timer = Time.realtimeSinceStartup + (1 / gun.fireRate);
					RaycastHit hit;
					var origin = cameraTransform.position;
					var dir = cameraTransform.forward;
					var range = 50f;
					if (Physics.Raycast(origin, dir, out hit, range)) {
						if (hit.transform.CompareTag("Enemy")) {
							hit.transform.GetComponent<DamageableBodyPart>().TakeDamage(gun.damage);
						}

						Debug.Log($"Bullet hit: {hit.transform.name}");
						Debug.DrawRay(origin, dir * range, Color.blue, 2.5f);
					}
				}
				else {
					boolValue = false;
					//Play Click Sound Effect
				}
			}


			canFireValue = boolValue;
		}

		public bool AmmoCheck() => currentWeapon.weaponInfo.reserveAmmo <= 0;

		public void Reload() {
			var gun = currentWeapon.weaponInfo;

			if (gun.reserveAmmo > gun.magSize) {
				gun.reserveAmmo += gun.currentAmmoInMag;

				gun.reserveAmmo -= gun.magSize;
				gun.currentAmmoInMag = gun.magSize;
			}
			else {
				gun.reserveAmmo += gun.currentAmmoInMag;
				if (gun.reserveAmmo > gun.magSize) {
					gun.reserveAmmo -= gun.magSize;
					gun.currentAmmoInMag = gun.magSize;
				}
				else {
					gun.currentAmmoInMag = gun.reserveAmmo;
					gun.reserveAmmo = 0;
				}
			}
		}
	}
}