using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeapoInformation : MonoBehaviour
{
    [Header("Gun Info")]
    public int reserveAmmo;
    public int currentAmmoInMag;

    [Header("Gun Stats")]
    public bool isFullAuto = true;
    public int maxAmmoInMag = 10;
    public int maxReserveAmmo = 50;
    public float bulletSpeed = 10;
    public float damage = 1;
    public float firerate = 2;

    public void OnEnable()
    {
        reserveAmmo = maxReserveAmmo;
        currentAmmoInMag = maxAmmoInMag;
    }
}
