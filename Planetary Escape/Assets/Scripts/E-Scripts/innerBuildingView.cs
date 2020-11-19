using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class innerBuildingView : MonoBehaviour
{
    public GameObject roof;
    Color newColorB;
    // Start is called before the first frame update
    void Start()
    {
        newColorB = roof.GetComponent<Renderer>().material.color;
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            newColorB.a = 0.1f;
            roof.GetComponent<Renderer>().material.color = newColorB;
        }


    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            newColorB.a = 1.0f;
            roof.GetComponent<Renderer>().material.color = newColorB;
        }

    }


}
