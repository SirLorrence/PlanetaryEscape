using System;
using System.Collections;
using System.Collections.Generic;
using Enemy;
using UnityEngine;
using Mirror;

public class GlobalShootingSystem : NetworkBehaviour
{
    [Header("Assignable")] 
    public WeaponObjects[] weaponList;
    public Transform gunTip;

    public enum Guns { Pistol }

    private float timer;
    private bool isADS = false;

    #region Singleton

    //Singleton Instantiation
    public static GlobalShootingSystem Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this.gameObject);
        else
            Instance = this;

        DontDestroyOnLoad(this);
        timer = 0;
    }

    #endregion

    public void Shoot(GameObject gun, Vector3 position, Vector3 direction)
    {
        print("shoot");
        if (timer < Time.realtimeSinceStartup)
        {
            WeapoInformation wo = gun.GetComponent<WeapoInformation>();
            if (wo.currentAmmoInMag > 0)
            {
                print("Shooting");
                timer = Time.realtimeSinceStartup + (1 / weaponList[(int)wo.gunType].firerate);
                gunTip.transform.position = position;
                gunTip.transform.forward = direction;

                if (isADS)
                {

                }
                else
                {
                    //Hipfire Calculations
                }

                wo.currentAmmoInMag--;
                CmdShootOnServer((int)wo.gunType);
            }
            else
            {
                //Click sound effect
            }
        }
    }

    public void Reload(GameObject gun)
    {
        WeapoInformation wo = gun.GetComponent<WeapoInformation>();
        if (wo.reserveAmmo <= 0)
        {
            //No Ammo
        }
        else if (wo.reserveAmmo > weaponList[(int)wo.gunType].magasineSize)
        {
            wo.reserveAmmo += wo.currentAmmoInMag;

            wo.reserveAmmo -= weaponList[(int)wo.gunType].magasineSize;
            wo.currentAmmoInMag = weaponList[(int)wo.gunType].magasineSize;
        }
        else
        {
            wo.reserveAmmo += wo.currentAmmoInMag;
            if (wo.reserveAmmo > weaponList[(int)wo.gunType].magasineSize)
            {
                wo.reserveAmmo -= weaponList[(int)wo.gunType].magasineSize;
                wo.currentAmmoInMag = weaponList[(int)wo.gunType].magasineSize;
            }
            else
            {
                wo.currentAmmoInMag = wo.reserveAmmo;
                wo.reserveAmmo = 0;
            }
        }
    }

    public void StartADS()
    {
        isADS = true;
    }

    public void StopADS()
    {
        isADS = false;
    }

    public void CmdShootOnServer(int weapon)
    {
        GameObject bullet = ObjectPooler.Instance.GetGameObject(1);
        bullet.transform.position = gunTip.position;
        bullet.transform.rotation = gunTip.rotation;
        bullet.GetComponent<Bullet>().StartBullet(weaponList[weapon].bulletSpeed, weaponList[weapon].damage, false, true);
        bullet.SetActive(true);
    }
}
