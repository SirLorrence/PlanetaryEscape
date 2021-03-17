using Entities.Enemy;
using Mirror;
using UnityEngine;
using Weapons;

namespace Entities.Player
{
    public class PlayerShoot : MonoBehaviour
    {
        [Header("Assignable")] 
        public Transform cameraTransform;
        public Transform gunTip;
        public WeaponInformation[] weapons;
        public WeaponSelect weaponSelect;

        [Header("Crosshair Settings")]
        public Texture2D crosshairImage;

        private float timer;

        void OnEnable()
        {
            timer = 0;
        }

        public void Shoot(WeaponInformation gun)
        {
            if (timer < Time.realtimeSinceStartup)
            {
                if (gun.currentAmmoInMag > 0)
                {
                    gun.currentAmmoInMag--;

                    timer = Time.realtimeSinceStartup + (1 / gun.firerate);

                    RaycastHit hit;
                    if (Physics.Raycast(transform.position, transform.forward, out hit, 100))
                    {
                        if (hit.transform.CompareTag("Enemy"))
                        {
                            hit.transform.GetComponent<DamageableBodyPart>().TakeDamage(gun.damage);
                        }
                    }
                }
                else
                {
                    //Play Click Sound Effect
                }
            }
        }

        public void Reload(WeaponInformation gun)
        {
            if (gun.reserveAmmo <= 0)
            {
                //No Ammo
            }
            else if (gun.reserveAmmo > gun.maxAmmoInMag)
            {
                weapons[weaponSelect.WeaponSelected].reserveAmmo += gun.currentAmmoInMag;

                gun.reserveAmmo -= gun.maxAmmoInMag;
                gun.currentAmmoInMag = gun.maxAmmoInMag;
            }
            else
            {
                gun.reserveAmmo += gun.currentAmmoInMag;
                if (gun.reserveAmmo > gun.maxAmmoInMag)
                {
                    gun.reserveAmmo -= gun.maxAmmoInMag;
                    gun.currentAmmoInMag = gun.maxAmmoInMag;
                }
                else
                {
                    gun.currentAmmoInMag = gun.reserveAmmo;
                    gun.reserveAmmo = 0;
                }
            }
        }
    }
}
