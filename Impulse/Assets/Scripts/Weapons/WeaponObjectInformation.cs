using UnityEngine;

namespace Weapons
{
	[CreateAssetMenu(fileName = "Weapon Info", menuName = "ScriptableObjects/Weapon")]
	public class WeaponObjectInformation : ScriptableObject
	{
		[Header("Stats")] public bool isFullAuto;
		public float fireRate, damage, bulletSpeed;
		public int magSize = 10, maxAmmoCapacity = 120;

		[Header("Current Weapon Information")]
		[ReadOnly]public int reserveAmmo;
		[ReadOnly]public int currentAmmoInMag;
		private void OnEnable() {
			reserveAmmo = maxAmmoCapacity;
			currentAmmoInMag = magSize;
		}
	}
}