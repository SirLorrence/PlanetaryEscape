using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Windows.Input;

public class PlayerShoot : MonoBehaviour
{
    [Header("Assignable")] 
    public Transform gunTipTransform;
    public Transform cameraTransform;
    public LayerMask shootingLayerMask;

    public enum Guns{Pistol}

    [Header("Debug")]
    public Guns currentWeapon;


    private float cooldown = 0.2f;
    private float shootTimer = 0f;

    void OnEnable()
    {
        shootTimer = Time.realtimeSinceStartup + cooldown;
    }

    public void Shoot()
    {
        if (shootTimer < Time.realtimeSinceStartup)
        {
            shootTimer = Time.realtimeSinceStartup + cooldown;
            Ray ray = new Ray(cameraTransform.position, cameraTransform.forward * 50);
            if (Physics.Raycast(ray, out RaycastHit hit, 200, shootingLayerMask))
            {
                ShootBullet(gunTipTransform.position, hit.point);
            }
        }
    }

    public void ChangeWeapon()
    {

    }

    void ShootBullet(Vector3 position, Vector3 rotation)
    {
        GameObject go = ObjectPooler.Instance.GetGameObject(1);
        go.transform.position = position;
        go.transform.LookAt(rotation);
        go.SetActive(true);
    }
}
