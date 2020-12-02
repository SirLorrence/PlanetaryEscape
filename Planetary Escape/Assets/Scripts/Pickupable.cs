using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;

public class Pickupable : MonoBehaviour
{
    public bool hasLifeSpan;
    public bool useSpin;
    public bool useLevatate;

    public float LifeSpan = 10;
    public float time;

    private float originalPos;

    public enum PickUp
    {
        health,
        skill,
        satellite,
        power
    }

    public PickUp type;

    void OnEnable()
    {
        time = Time.time;
        originalPos = transform.position.y;
    }

    void Update()
    {
        if (useSpin)
            ObjectSpin();
        if (useLevatate)
            ObjectLevitate();

        if (hasLifeSpan)
        {
            if (time + LifeSpan < Time.time)
            {
                GameManager.Instance.SetObjectInPool(gameObject);
            }
        }
    }

    void OnTriggerEnter(Collider c)
    {
        if (c.CompareTag("Player"))
        {
            if (type == PickUp.health)
            {
                GameManager.Instance.Heal(1);
                GameManager.Instance.SetObjectInPool(gameObject);
            }
            else if(type == PickUp.skill)
            {
                GameManager.Instance.IncreaseUpgradeScore(1);
                GameManager.Instance.SetObjectInPool(gameObject);
            }

        }
    }

    void ObjectSpin() //gives the object a nice animation
    {
        transform.Rotate(Vector3.down * (.01f * Time.time));
    }

    void ObjectLevitate()
    {
        float waveAmplitude = 0.5f;
        float speed = 1f;
        var offset = waveAmplitude * Mathf.Sin(Time.time * speed);
        

        transform.position = new Vector3(transform.position.x, originalPos + offset, transform.position.z);
    }
}