using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    private GameObject GunBarrel;
    private PlayerStats playerStats;

    void Start()
    {
        playerStats = gameObject.GetComponent<PlayerStats>();
        GunBarrel = GameObject.FindGameObjectWithTag("GunTip");
    }
    public void Shoot()
    {
        GameObject go = GameManager.Instance.GetPlayerBullet();
        if (go != null)
        {
            go.transform.position = GunBarrel.transform.position;
            go.transform.rotation = GunBarrel.transform.rotation;
            go.GetComponent<Bullet>().OnStart(playerStats.BulletSpeed, playerStats.Damage);
            go.SetActive(true);
            SoundManager.NotifyAudio(SoundManager.Instance.laser);
        }
        else
            Debug.LogWarning("Bullet Is Null");
    }
}
