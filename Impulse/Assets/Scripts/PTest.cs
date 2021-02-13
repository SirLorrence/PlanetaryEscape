using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PTest : MonoBehaviour
{
    public float moveSpeed;

    private Rigidbody rb;
    private Animator anim;

    private Vector3 movement;
    
    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody>();
        if (rb == null) {
            Debug.LogError("No rigidbody");
        }

        anim = GetComponentInChildren<Animator>();
        if(anim == null) Debug.LogError("No animator found");
    }

    // Update is called once per frame
    void Update() {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        movement = (x * transform.right + z * transform.forward).normalized; 
        
        anim.SetFloat("xInput",x,0.1f, Time.deltaTime);
        anim.SetFloat("zInput",z,0.1f, Time.deltaTime);
    }

    private void FixedUpdate() {
        rb.MovePosition(transform.position + (movement * moveSpeed * Time.deltaTime));
      
        
    }
}
