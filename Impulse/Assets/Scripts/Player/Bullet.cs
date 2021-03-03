using System.Collections;
using System.Collections.Generic;
using Enemy;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Bullet Stats")]
    [SerializeField]private float lifetime = 2f;
    [SerializeField] private float tracerDelay= 0.2f;

    private bool hasTracer = false;


    private float lifetimeTimer = 0;
    private float tracerTimer = 0;

    private float speed, damage;
    private bool canDamagePlayer, canDamageEnemy;
    public LayerMask layerMask = -1; //make sure we aren't in this layer 
    public float skinWidth = 0.1f;

    private float minimumExtent;
    private float partialExtent;
    private float sqrMinimumExtent;
    private Vector3 previousPosition;
    private Rigidbody myRigidbody;
    private Collider myCollider;


    void Awake()
    {
        myRigidbody = GetComponent<Rigidbody>();
        myCollider = GetComponent<Collider>();
        previousPosition = myRigidbody.position;
        minimumExtent = Mathf.Min(Mathf.Min(myCollider.bounds.extents.x, myCollider.bounds.extents.y), myCollider.bounds.extents.z);
        partialExtent = minimumExtent * (1.0f - skinWidth);
        sqrMinimumExtent = minimumExtent * minimumExtent;
    }
    public void StartBullet(float speed, float damage, bool canDamagePlayer) 
    {
        this.speed = speed;
        this.damage = damage;
        this.canDamagePlayer = !canDamagePlayer;
        this.canDamageEnemy = canDamagePlayer;
    }

    void FixedUpdate()
    {
        //have we moved more than our minimum extent? 
        Vector3 movementThisStep = myRigidbody.position - previousPosition;
        float movementSqrMagnitude = movementThisStep.sqrMagnitude;

        if (movementSqrMagnitude > sqrMinimumExtent)
        {
            float movementMagnitude = Mathf.Sqrt(movementSqrMagnitude);
            RaycastHit hitInfo;

            //check for obstructions we might have missed 
            if (Physics.Raycast(previousPosition, movementThisStep, out hitInfo, movementMagnitude, layerMask.value))
            {
                if (!hitInfo.collider) { print("No Collider"); return; }

                if (!hitInfo.collider.isTrigger)
                { myRigidbody.position = hitInfo.point - (movementThisStep / movementMagnitude) * partialExtent; print("Calculating Position"); }
                
                if (canDamageEnemy && hitInfo.collider.CompareTag("Enemy"))
                {
                    print("Hit Dummy");
                    DamageableBodyPart dummy = hitInfo.collider.gameObject.GetComponent<DamageableBodyPart>();
                    dummy.TakeDamage(101);
                    ObjectPooler.Instance.SetObjectInPool(this.gameObject);
                }
                else if (canDamagePlayer && hitInfo.collider.CompareTag("Player"))
                {
                    print("Hit Player");
                    print(hitInfo.collider.gameObject.name);
                    ObjectPooler.Instance.SetObjectInPool(this.gameObject);
                }
                else if (hitInfo.collider.gameObject.layer == 10)
                {
                    print("Hit Nothing");
                    print(hitInfo.collider.gameObject.name);
                    ObjectPooler.Instance.SetObjectInPool(this.gameObject);
                }
            }
        }

        previousPosition = myRigidbody.position;
    }

    void OnEnable()
    {
        lifetimeTimer = lifetime;
        previousPosition = myRigidbody.position;
    }

    void Update()
    {
        Move();
        CheckLifetime();
        CheckTracer();
    }

    void Move()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }
    void CheckLifetime()
    {
        if (lifetimeTimer <= 0) ObjectPooler.Instance.SetObjectInPool(this.gameObject);
        else lifetimeTimer -= Time.deltaTime;
    }
    void CheckTracer()
    {
        //if (tracerTimer <= 0) //enable tracer
        //else tracerTimer -= Time.deltaTime;
    }
}
