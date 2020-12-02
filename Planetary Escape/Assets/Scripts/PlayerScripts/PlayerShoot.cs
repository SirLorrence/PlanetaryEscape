using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    private GameObject GunBarrel;
    private PlayerStats playerStats;

    // Start is called before the first frame update
    void Start()
    {
        playerStats = gameObject.GetComponent<PlayerStats>();
        GunBarrel = GameObject.FindGameObjectWithTag("GunTip");
    }

    // Update is called once per frame
    public void Shoot()
    {
        GameObject go = GameManager.Instance.GetPlayerBullet();
        if (go != null)
        {
            go.transform.position = GunBarrel.transform.position;
            go.transform.rotation = GunBarrel.transform.rotation;
            go.GetComponent<Bullet>().OnStart(playerStats.BulletSpeed, playerStats.Damage);
            go.SetActive(true);
        }
        else
            Debug.LogWarning("Bullet Is Null");
    }
}
