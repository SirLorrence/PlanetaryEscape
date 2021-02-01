using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDrop : MonoBehaviour
{
    public bool canDrop = false;

    public Rigidbody rigidbody;

    private void OnEnable() {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.isKinematic = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (canDrop) {
            transform.SetParent(null);
            rigidbody.isKinematic = false;
        }
        
    }
}
