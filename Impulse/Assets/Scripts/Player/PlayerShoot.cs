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
    public LayerMask shootingLayerMask;
    public GameObject bulletPrefab;
    public WeaponObjects[] weaponList;
    public Transform gunTip;

    [Header("Crosshair Settings")]
    public Texture2D crosshairImage;

    public enum Guns { Pistol }
    public Guns currentWeapon;

    private float timer;

    private void Start()
    {
        ChangeWeapon(0);
    }

    void OnEnable()
    {
        timer = Time.realtimeSinceStartup + (1 / weaponList[(int)currentWeapon].firerate);
    }

    public void Shoot()
    {
        print("shoot");
        if (timer < Time.realtimeSinceStartup)
        {
            print("Shooting");
            timer = Time.realtimeSinceStartup + (1 / weaponList[(int)currentWeapon].firerate);
            Ray ray = new Ray(cameraTransform.position, cameraTransform.forward * 50);
            if (Physics.Raycast(ray, out RaycastHit hit, 200))
            {
                gunTip.LookAt(hit.point);
            }
            CmdShootOnServer();
        }
    }

    public void StartADS()
    {

    }

    public void StopADS()
    {

    }

    public void ChangeWeapon(int i)
    {
        currentWeapon = (Guns)i;

        Transform parentTransform = weaponList[(int)currentWeapon].model.transform;
        foreach (Transform child in parentTransform) 
        {
            if (child.CompareTag("GunTip")) 
            { 
                weaponList[(int)currentWeapon].gunTipTransform = child;
                print(child.name);
                break;
            }
        }

    }

    [Command]
    public void CmdShootOnServer()
    {
        //GameObject bullet = ObjectPooler.Instance.GetGameObject(1);
        //bullet.transform.position = gunTip.position;
        //bullet.transform.rotation = gunTip.rotation;
        //bullet.GetComponent<Bullet>().StartBullet(weaponList[(int)currentWeapon].bulletSpeed, weaponList[(int)currentWeapon].damage, false, true);
        //bullet.SetActive(true);


        GameObject bullet = Instantiate(bulletPrefab, gunTip.position, gunTip.rotation);
        bullet.GetComponent<Bullet>().StartBullet(weaponList[(int)currentWeapon].bulletSpeed, weaponList[(int)currentWeapon].damage, false, true);
        Destroy(bullet, 5);
        NetworkServer.Spawn(bullet);
    }
}
