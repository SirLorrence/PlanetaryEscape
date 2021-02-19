using UnityEngine;
using Mirror;

public class GlobalShootingSystem : MonoBehaviour
{
    [Header("Assignable")] 
    public WeaponObjects[] weaponList;
    public Transform gunTip;

    public enum Guns { Pistol }

    private float timer;

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

    public void Shoot(GameObject gun, Vector3 position, Vector3 direction, bool fromPlayer, bool bulletHasTracer)
    {
        if (timer < Time.realtimeSinceStartup)
        {
            WeapoInformation wo = gun.GetComponent<WeapoInformation>();
            if (wo.currentAmmoInMag > 0)
            {
                wo.currentAmmoInMag--;

                timer = Time.realtimeSinceStartup + (1 / weaponList[(int)wo.gunType].firerate);
                
                gunTip.transform.position = position;
                gunTip.transform.forward = direction;

                //Hipfire Calculations
                //var randomPosition = new Vector3(UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360));
                //randomPosition = randomPosition.normalized * weaponList[(int)wo.gunType].bulletSpread;
                //gunTip.rotation = new Quaternion(gunTip.rotation.x + randomPosition.x, gunTip.rotation.y + randomPosition.y, gunTip.rotation.z + randomPosition.z, 0);
                
                CmdShootOnServer((int)wo.gunType, fromPlayer, bulletHasTracer);
            }
            else
            {
                //Play Click Sound Effect
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

    public void CmdShootOnServer(int weapon, bool fromPlayer, bool bulletHasTracer)
    {
        GameObject bullet = ObjectPooler.Instance.GetGameObject(1);
        bullet.transform.position = gunTip.position;
        bullet.transform.rotation = gunTip.rotation;
        bullet.GetComponent<Bullet>().StartBullet(weaponList[weapon].bulletSpeed, weaponList[weapon].damage, fromPlayer, bulletHasTracer);

        BulletMessage msg = new BulletMessage() 
        {
            bulletHasTracer = bulletHasTracer,
            bulletPosition = gunTip.position,
            rotation = gunTip.rotation
        };
        
        NetworkServer.SendToReady<BulletMessage>(msg);
        bullet.SetActive(true);

        
    }
}

public struct BulletMessage : NetworkMessage
{
    public Vector3 bulletPosition;
    public Quaternion rotation;
    public bool bulletHasTracer;
}
