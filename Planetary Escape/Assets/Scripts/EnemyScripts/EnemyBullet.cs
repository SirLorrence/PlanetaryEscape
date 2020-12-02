using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] float bulletDuration = 5;

    private float time;
    public float bulletSpeed = 10;
    [HideInInspector] public int bulletDamage = 1;

    private void Start()
    {
        time = Time.time;
    }

    private void Update()
    {
        if (time + bulletDuration < Time.time)
        {
            GameManager.Instance.SetObjectInPool(gameObject);
        }

        transform.Translate(Vector3.forward * Time.deltaTime * bulletSpeed);
    }

    private void OnEnable()
    {
        //Saves time when enabled for lifetime of bullet
        time = Time.time;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.TakeDamage(collision.gameObject, bulletDamage);
        }

        GameManager.Instance.SetObjectInPool(gameObject);
    }
}