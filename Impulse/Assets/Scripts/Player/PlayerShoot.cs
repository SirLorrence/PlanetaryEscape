using System;
using System.Collections;
using System.Collections.Generic;
using Enemy;
using UnityEngine;
using Mirror;

public class PlayerShoot : NetworkBehaviour
{
    [Header("Assignable")] 
    public Transform cameraTransform;
    public Transform gunTip;
    public WeapoInformation[] weapons;
    public WeaponSelect weaponSelect;

    [Header("Crosshair Settings")]
    public Texture2D crosshairImage;

    private float timer;

    void OnEnable()
    {
        timer = 0;
    }

    public void Shoot(WeapoInformation gun)
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

    public void Reload(WeapoInformation gun)
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
