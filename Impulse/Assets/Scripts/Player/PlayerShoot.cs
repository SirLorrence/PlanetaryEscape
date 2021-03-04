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

    
    void OnEnable() {
        timer = Time.realtimeSinceStartup + (1 / weapons[weaponSelect.WeaponSelected].firerate);
    }

    public void Shoot()
    {
        if (timer < Time.realtimeSinceStartup)
        {
            if (weapons[weaponSelect.WeaponSelected].currentAmmoInMag > 0)
            {
                weapons[weaponSelect.WeaponSelected].currentAmmoInMag--;

                timer = Time.realtimeSinceStartup + (1 / weapons[weaponSelect.WeaponSelected].firerate);

                //Hipfire Calculations
                //var randomPosition = new Vector3(UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360));
                //randomPosition = randomPosition.normalized * weaponList[(int)wo.gunType].bulletSpread;
                //gunTip.rotation = new Quaternion(gunTip.rotation.x + randomPosition.x, gunTip.rotation.y + randomPosition.y, gunTip.rotation.z + randomPosition.z, 0);

                GameObject bullet = ObjectPooler.Instance.GetGameObject(0);
                bullet.transform.position = gunTip.transform.position;
                bullet.transform.rotation = gunTip.transform.rotation;
                bullet.GetComponent<Bullet>().StartBullet(weapons[weaponSelect.WeaponSelected].bulletSpeed, weapons[weaponSelect.WeaponSelected].damage, true);

                bullet.SetActive(true);
            }
            else
            {
                //Play Click Sound Effect
            }
        }
    }

    public void Reload()
    {
        if (weapons[weaponSelect.WeaponSelected].reserveAmmo <= 0)
        {
            //No Ammo
        }
        else if (weapons[weaponSelect.WeaponSelected].reserveAmmo > weapons[weaponSelect.WeaponSelected].maxAmmoInMag)
        {
            weapons[weaponSelect.WeaponSelected].reserveAmmo += weapons[weaponSelect.WeaponSelected].currentAmmoInMag;

            weapons[weaponSelect.WeaponSelected].reserveAmmo -= weapons[weaponSelect.WeaponSelected].maxAmmoInMag;
            weapons[weaponSelect.WeaponSelected].currentAmmoInMag = weapons[weaponSelect.WeaponSelected].maxAmmoInMag;
        }
        else
        {
            weapons[weaponSelect.WeaponSelected].reserveAmmo += weapons[weaponSelect.WeaponSelected].currentAmmoInMag;
            if (weapons[weaponSelect.WeaponSelected].reserveAmmo > weapons[weaponSelect.WeaponSelected].maxAmmoInMag)
            {
                weapons[weaponSelect.WeaponSelected].reserveAmmo -= weapons[weaponSelect.WeaponSelected].maxAmmoInMag;
                weapons[weaponSelect.WeaponSelected].currentAmmoInMag = weapons[weaponSelect.WeaponSelected].maxAmmoInMag;
            }
            else
            {
                weapons[weaponSelect.WeaponSelected].currentAmmoInMag = weapons[weaponSelect.WeaponSelected].reserveAmmo;
                weapons[weaponSelect.WeaponSelected].reserveAmmo = 0;
            }
        }
    }
}
