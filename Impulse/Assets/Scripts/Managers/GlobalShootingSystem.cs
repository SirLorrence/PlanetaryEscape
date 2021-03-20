using UnityEngine;
// using Weapons;

namespace Managers
{
    public class GlobalShootingSystem : MonoBehaviour
    {
        // [Header("Assignable")] 
        // public WeaponObjectInformation[] weaponList;
        // public Transform gunTip;
        //
        // public enum Guns { Pistol }
        //
        // private float timer;
        //
        // #region Singleton
        //
        // //Singleton Instantiation
        // public static GlobalShootingSystem Instance { get; private set; }
        //
        // private void Awake()
        // {
        //     if (Instance != null && Instance != this)
        //         Destroy(this.gameObject);
        //     else
        //         Instance = this;
        //
        //     DontDestroyOnLoad(this);
        //     timer = 0;
        // }

        // #endregion

        //public void Shoot(GameObject gun, Vector3 position, Vector3 direction, bool fromPlayer, bool bulletHasTracer)
        //{
        //    if (timer < Time.realtimeSinceStartup)
        //    {
        //        WeapoInformation wo = gun.GetComponent<WeapoInformation>();
        //        if (wo.currentAmmoInMag > 0)
        //        {
        //            wo.currentAmmoInMag--;

        //            timer = Time.realtimeSinceStartup + (1 / weaponList[(int)wo.gunType].firerate);
                
        //            gunTip.transform.position = position;   
        //            gunTip.transform.forward = direction;

        //            //Hipfire Calculations
        //            //var randomPosition = new Vector3(UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360));
        //            //randomPosition = randomPosition.normalized * weaponList[(int)wo.gunType].bulletSpread;
        //            //gunTip.rotation = new Quaternion(gunTip.rotation.x + randomPosition.x, gunTip.rotation.y + randomPosition.y, gunTip.rotation.z + randomPosition.z, 0);

                
        //            BulletMessage msg = new BulletMessage()
        //            {
        //                bulletHasTracer = bulletHasTracer,
        //                position = gunTip.position,
        //                rotation = gunTip.rotation,
        //                speed = weaponList[0].bulletSpeed,
        //                damage = weaponList[0].damage,
        //                fromPlayer = fromPlayer
        //            };

        //            //if (isServer)
        //            //    RpcShootOnAll(msg);
        //            //else
        //            //    CmdShootOnServer(msg);
                
        //        }
        //        else
        //        {
        //            //Play Click Sound Effect
        //        }
        //    }
        //}

        //public void Reload(GameObject gun)
        //{
        //    WeapoInformation wo = gun.GetComponent<WeapoInformation>();
        //    if (wo.reserveAmmo <= 0)
        //    {
        //        //No Ammo
        //    }
        //    else if (wo.reserveAmmo > weaponList[(int)wo.gunType].magasineSize)
        //    {
        //        wo.reserveAmmo += wo.currentAmmoInMag;

        //        wo.reserveAmmo -= weaponList[(int)wo.gunType].magasineSize;
        //        wo.currentAmmoInMag = weaponList[(int)wo.gunType].magasineSize;
        //    }
        //    else
        //    {
        //        wo.reserveAmmo += wo.currentAmmoInMag;
        //        if (wo.reserveAmmo > weaponList[(int)wo.gunType].magasineSize)
        //        {
        //            wo.reserveAmmo -= weaponList[(int)wo.gunType].magasineSize;
        //            wo.currentAmmoInMag = weaponList[(int)wo.gunType].magasineSize;
        //        }
        //        else
        //        {
        //            wo.currentAmmoInMag = wo.reserveAmmo;
        //            wo.reserveAmmo = 0;
        //        }
        //    }
        //}

    
        //public void CmdShootOnServer(BulletMessage msg)
        //{
        //    print("Shoot");
        //    GameObject bullet = ObjectPooler.Instance.GetGameObject(1);
        //    bullet.transform.position = msg.position;
        //    bullet.transform.rotation = msg.rotation;
        //    bullet.GetComponent<Bullet>().StartBullet(msg.speed, msg.damage, msg.fromPlayer, msg.bulletHasTracer);

        //    bullet.SetActive(true);

        //    RpcShootOnAll(msg);
        //}

  
        //public void RpcShootOnAll(BulletMessage msg)
        //{
        //    GameObject bullet = ObjectPooler.Instance.GetGameObject(1);
        //    bullet.transform.position = msg.position;
        //    bullet.transform.rotation = msg.rotation;
        //    bullet.GetComponent<Bullet>().StartBullet(msg.speed, msg.damage, msg.fromPlayer, msg.bulletHasTracer);

        //    bullet.SetActive(true);
        //}
    }

    public struct BulletMessage
    {
        public Vector3 position;
        public Quaternion rotation;
        public bool bulletHasTracer;
        public float speed;
        public float damage;
        public bool fromPlayer;
    }
}