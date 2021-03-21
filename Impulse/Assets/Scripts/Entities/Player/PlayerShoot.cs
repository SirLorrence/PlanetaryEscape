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

		public void Shoot() {
			var gun = currentWeapon.weaponInfo;

			if (timer < Time.realtimeSinceStartup) {
				if (gun.currentAmmoInMag > 0) {
					gun.currentAmmoInMag--;

					timer = Time.realtimeSinceStartup + (1 / gun.fireRate);

					RaycastHit hit;
					if (Physics.Raycast(transform.position, transform.forward, out hit, 100)) {
						if (hit.transform.CompareTag("Enemy")) {
							hit.transform.GetComponent<DamageableBodyPart>().TakeDamage(gun.damage);
						}
					}
				}
				else {
					//Play Click Sound Effect
				}
			}
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