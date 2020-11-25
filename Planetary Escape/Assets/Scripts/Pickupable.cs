using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickupable : MonoBehaviour
{
    public bool hasLifeSpan;

    public float LifeSpan = 10;
    public float time;

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
    }

    void Update()
    {
        //ObjectSpin();
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
            GameManager.Instance.Heal(1);
            GameManager.Instance.SetObjectInPool(gameObject);
        }
    }

    void ObjectSpin() //gives the object a nice animation
    {
        transform.Rotate(Vector3.down * (.01f * Time.time));

        float waveAmplitude = 0.5f;
        float speed = default;
        var offset = waveAmplitude * Mathf.Sin(Time.time * speed);
        var position = transform.position;

        transform.position = new Vector3(position.x, offset, position.z);
    }
}