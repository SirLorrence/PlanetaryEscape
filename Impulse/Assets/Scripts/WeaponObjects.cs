using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "weapon", menuName = "ScriptableObjects/WeaponScriptableObject")]
public class WeaponObjects : ScriptableObject
{
    [Header("Assignables")]
    public GameObject model;
    public Transform gunTipTransform;

    [Header("Stats")]
    public float damage;
    public float firerate;
    public float magasineSize;
    public float reserveAmmo;
    public float bulletSpeed;
    

    [Header("Settings")]
    public bool canAds;
    public bool isFullauto;
}
