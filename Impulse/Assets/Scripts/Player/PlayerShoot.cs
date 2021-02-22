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
        timer = Time.realtimeSinceStartup + (1 / weapons[weaponSelect.weaponSelected].firerate);
    }

    public void Shoot()
    {
        if (timer < Time.realtimeSinceStartup)
        {
            if (weapons[weaponSelect.weaponSelected].currentAmmoInMag > 0)
            {
                weapons[weaponSelect.weaponSelected].currentAmmoInMag--;

                timer = Time.realtimeSinceStartup + (1 / weapons[weaponSelect.weaponSelected].firerate);

                //Hipfire Calculations
                //var randomPosition = new Vector3(UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360));
                //randomPosition = randomPosition.normalized * weaponList[(int)wo.gunType].bulletSpread;
                //gunTip.rotation = new Quaternion(gunTip.rotation.x + randomPosition.x, gunTip.rotation.y + randomPosition.y, gunTip.rotation.z + randomPosition.z, 0);

                GameObject bullet = ObjectPooler.Instance.GetGameObject(0);
                bullet.transform.position = gunTip.transform.position;
                bullet.transform.rotation = gunTip.transform.rotation;
                bullet.GetComponent<Bullet>().StartBullet(weapons[weaponSelect.weaponSelected].bulletSpeed, weapons[weaponSelect.weaponSelected].damage, true);

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
        if (weapons[weaponSelect.weaponSelected].reserveAmmo <= 0)
        {
            //No Ammo
        }
        else if (weapons[weaponSelect.weaponSelected].reserveAmmo > weapons[weaponSelect.weaponSelected].maxAmmoInMag)
        {
            weapons[weaponSelect.weaponSelected].reserveAmmo += weapons[weaponSelect.weaponSelected].currentAmmoInMag;

            weapons[weaponSelect.weaponSelected].reserveAmmo -= weapons[weaponSelect.weaponSelected].maxAmmoInMag;
            weapons[weaponSelect.weaponSelected].currentAmmoInMag = weapons[weaponSelect.weaponSelected].maxAmmoInMag;
        }
        else
        {
            weapons[weaponSelect.weaponSelected].reserveAmmo += weapons[weaponSelect.weaponSelected].currentAmmoInMag;
            if (weapons[weaponSelect.weaponSelected].reserveAmmo > weapons[weaponSelect.weaponSelected].maxAmmoInMag)
            {
                weapons[weaponSelect.weaponSelected].reserveAmmo -= weapons[weaponSelect.weaponSelected].maxAmmoInMag;
                weapons[weaponSelect.weaponSelected].currentAmmoInMag = weapons[weaponSelect.weaponSelected].maxAmmoInMag;
            }
            else
            {
                weapons[weaponSelect.weaponSelected].currentAmmoInMag = weapons[weaponSelect.weaponSelected].reserveAmmo;
                weapons[weaponSelect.weaponSelected].reserveAmmo = 0;
            }
        }
    }
}
