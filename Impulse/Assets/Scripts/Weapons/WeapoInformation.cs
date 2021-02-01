using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeapoInformation : MonoBehaviour
{
    [Header("Gun Info")]
    public int reserveAmmo;
    public int currentAmmoInMag;

    public GlobalShootingSystem.Guns gunType;

    public void OnEnable()
    {
        WeaponObjects wo = GlobalShootingSystem.Instance.weaponList[(int)gunType];
        reserveAmmo = wo.reserveAmmo;
        currentAmmoInMag = wo.magasineSize;
}
}
