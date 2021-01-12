using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class Bullet : MonoBehaviour
{
    [Header("Bullet Stats")]
    public int speed = 5;
    public int damage = 1;
    public float lifetime = 2f;
    public float lifetimeTimer = 0;

    public LayerMask layerMask = -1; //make sure we aren't in this layer 
    public float skinWidth = 0.1f; //probably doesn't need to be changed 

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
                if (!hitInfo.collider)
                    return;

                if (!hitInfo.collider.isTrigger)
                    myRigidbody.position = hitInfo.point - (movementThisStep / movementMagnitude) * partialExtent;

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

    void OnTriggerEnter(Collider c)
    {
        //print(c.name);
        if (c.CompareTag("Enemy"))
        {
            DamageableBodyPart dummy = c.gameObject.GetComponent<DamageableBodyPart>();
            dummy.TakeDamage(101);
            ObjectPooler.Instance.SetObjectInPool(this.gameObject);
        }
        else if(!c.CompareTag("Player"))
        {
            ObjectPooler.Instance.SetObjectInPool(this.gameObject);
        }

    }
}
