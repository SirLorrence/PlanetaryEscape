using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float bulletDuration = 5;
    [SerializeField] private string targetTag;
    [HideInInspector] public int damage = 1;
    

    private float speed;
    private float time;

    private void Start()
    {
        time = Time.time;
    }

    public void OnStart(float speed, int damage)
    {
        //Saves time when enabled for lifetime of bullet
        time = Time.time;
        this.speed = speed;
        this.damage = damage;
    }

    private void Update()
    {
        if (time + bulletDuration < Time.time)
        {
            GameManager.Instance.SetObjectInPool(gameObject);
        }

        transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }

    void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.CompareTag(targetTag))
            GameManager.Instance.TakeDamage(c.gameObject, damage);
        else if (c.gameObject.CompareTag("Bullet"))
        {
            
        }
        else
        {
            GameManager.Instance.SetObjectInPool(gameObject);
        }
    }
}